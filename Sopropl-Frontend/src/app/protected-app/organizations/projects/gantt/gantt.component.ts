import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation, Input } from '@angular/core';
import 'dhtmlx-gantt';
import { TaskService } from 'src/app/_services/Task.service';
import { LinkService } from 'src/app/_services/link.service';
import { Graph } from 'src/app/_models/graph.model';
import { ActivityService } from 'src/app/_services/activity.service';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from 'src/app/_services/Alertify.service';
@Component({
  providers: [TaskService, LinkService],
  encapsulation: ViewEncapsulation.None,
  // tslint:disable-next-line: component-selector
  selector: 'gantt',
  templateUrl: './gantt.component.html',
  styleUrls: ['./gantt.component.scss'],
  // template: ``,
})
export class GanttComponent implements OnInit {
  orgName: string;
  projName: string;
  @Input() graph: Graph;
  constructor(
    private taskService: TaskService,
    private linkService: LinkService,
    private activityService: ActivityService,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService
  ) {
    this.projName = this.route.snapshot.parent.params.projName;
    this.orgName = this.route.snapshot.parent.parent.params.orgName;

    // this.route.queryParams
    //   .subscribe(params => {
    //     console.log(params);
    //     // if (params.get('id') !== this.myParam) {
    //     this.projName = params.projName;
    //     this.orgName = params.orgName;
    //     console.log(this.orgName);
    //     console.log(this.projName);
    //     // Do things with new parameter - e.g. reload data
    //     // }
    //   });
  }

  @ViewChild('gantt_here') ganttContainer: ElementRef;

  save() {
    const tasks = gantt.getTaskByTime();
    const links = gantt.getLinks();
    const grpa = this.taskService.convertToGraph(tasks, links);
    // console.log(this.orgName);
    // console.log(this.projName);
    // this.route.parent.params.subscribe(res => {
    //   console.log(res);

    // });
    // console.log(this.route.parent.parent.parent.params.orgName);
    this.activityService.create(this.orgName, this.projName, grpa).subscribe(res => {
      this.alertifyService.success('graph of @' + this.projName + ' has been updated successfuly');
    }, err => {
      this.alertifyService.error(err);
    });
  }

  ngOnInit() {
    gantt.config.xml_date = '%Y/%m/%d %H:%i';
    // gantt.config.start_date = this.graph.earlyStart;
    // gantt.config.server_utc = true;
    gantt.config.sort = true;
    // gantt.date

    // gantt.attachEvent('onError', function (errorMessage) {
    //   debugger;
    //   return true;
    // });
    // gantt.templates.grid_row_class = (start, end, task) => {
    //   return 'nested_task';
    // };
    gantt.attachEvent('onBeforeLinkAdd', (id, link) => {
      const sourceTask = gantt.getTask(link.source);
      const targetTask = gantt.getTask(link.target);
      if (link.type === '0') { // "finish_to_start":"0"
        if (sourceTask.end_date >= targetTask.start_date) {
          alert('This link is illegal');
          return false;
        }
      }

      if (link.type === '1') { // "start_to_start":"1"
        if (sourceTask.start_date >= targetTask.start_date) {
          alert('This link is illegal');
          return false;
        }
      }
      if (link.type === '2') { //  "finish_to_finish":"2"
        if (sourceTask.end_date >= targetTask.end_date) {
          alert('This link is illegal');
          return false;

        }
      }

      if (link.type === '3') { // "start_to_finish":"3"
        if (sourceTask.start_date >= targetTask.end_date) {
          alert('This link is illegal');
          return false;

        }
      }
      return true;
    });
    gantt.init(this.ganttContainer.nativeElement);
    const ganttGraph = this.taskService.get(this.graph);
    Promise.all([ganttGraph.then(g => g.Tasks), ganttGraph.finally().then(g => g.Links)])
      .then(([data, links]) => {
        gantt.parse({ data, links });
      });
  }

}
