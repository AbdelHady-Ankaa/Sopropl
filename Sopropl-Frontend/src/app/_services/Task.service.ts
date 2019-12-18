import { Injectable } from '@angular/core';
import { Task } from '../_models/task';
import { Graph } from '../_models/graph.model';
import { Link } from '../_models/Link';
import { Activity } from '../_models/activity.model';
import { Arrow, ArrowType } from '../_models/arrow.model';

export class GanttGraph {
  public Tasks: Promise<Task[]>;
  public Links: Promise<Link[]>;
}
const FAKE_START_ACTIVITY_NAME = 'FakeStartActivity';
const FAKE_END_ACTIVITY_NAME = 'FakeEndActivity';
@Injectable({
  providedIn: 'root'
})
export class TaskService {
  get(graph: Graph): Promise<GanttGraph> {
    return Promise.resolve(this.convertToGanttGraph(graph));
  }
  private convertToGanttGraph(graph: Graph) {

    const tasks: Task[] = [];
    const links: Link[] = [];
    const startNode = graph.startNode;
    // Date.

    // Date.UTC(Date.parse(''));
    if (startNode.outArrows.length === 0 && startNode.name === FAKE_START_ACTIVITY_NAME) {
      return { Tasks: Promise.resolve([]), Links: Promise.resolve([]) } as GanttGraph;
    }
    // debugger;P
    const startTask = new Task(startNode.name, startNode.earlyStart, startNode.name, startNode.duration, 0);
    tasks.push(startTask);


    let lns = startNode.outArrows
      .map(oa =>
        new Link(oa.toActivity.name + startNode.name, startNode.name, oa.toActivity.name, oa.type.toString())
      );

    lns.forEach(l => links.push(l));
    // links.then(ls => ls.concat(linksToNextTask));

    let nextNodes = startNode.outArrows.map(oa => oa.toActivity);
    let ts = nextNodes.map(nn => new Task(nn.name, nn.earlyStart, nn.name, nn.duration, 0));
    ts.forEach(t => tasks.push(t));
    let beforeNodes = nextNodes;
    // debugger;
    while (nextNodes.length > 0) {
      beforeNodes.forEach(bn => {
        lns = bn.outArrows.map(oa =>
          new Link(oa.toActivity.name + startNode.name, startNode.name, oa.toActivity.name, oa.type.toString()));
        lns.forEach(l => links.push(l));

        bn.outArrows.map(oa => nextNodes.push(oa.toActivity));

        ts = nextNodes.map(nn => new Task(nn.name, nn.earlyStart, nn.name, nn.duration, 0));
        ts.forEach(t => tasks.push(t));
      });
      beforeNodes = nextNodes;
      nextNodes = [];
      // links.then(ls => ls.concat(linksToNextTask));
      // tasks.concat(nextTasks);
    }
    console.log(tasks);
    console.log(links);
    // links.then(l => console.log(l));
    console.log(graph);
    return { Tasks: Promise.resolve(tasks), Links: Promise.resolve(links) } as GanttGraph;
  }

  public convertToGraph(tasks: Task[], links: Link[]) {
    const graph: Graph = new Graph();
    const startNodes = tasks.filter(t => links.filter(l => l.target === t.id).length === 0).map(t => {
      const actv = new Activity();
      actv.name = t.text;
      actv.duration = t.duration;
      actv.earlyStart = t.start_date;
      actv.hoursSpent = t.progress;
      return actv;
    });
    // debugger;
    let earlyStart: Date = new Date('2031-01-01T00:00:00');
    console.log(earlyStart);
    if (startNodes.length > 1) {
      const fakeActv = new Activity();
      fakeActv.name = FAKE_START_ACTIVITY_NAME;
      fakeActv.outArrows = [];
      startNodes.forEach(a => {
        const arr = new Arrow();
        arr.toActivity = a;
        arr.type = 0;
        // arr.fromActivity = fakeActv;
        fakeActv.outArrows.push(arr);
      });

      startNodes.forEach(
        sn => {
          if (earlyStart > new Date(sn.earlyStart)) {
            earlyStart = new Date(sn.earlyStart);
          }
        }
      );
      fakeActv.earlyStart = earlyStart.toString();
      graph.startNode = fakeActv;

    } else {
      graph.startNode = startNodes.pop();
      startNodes.push(graph.startNode);
      earlyStart = new Date(startNodes.map(a => a.earlyStart).pop());
    }
    graph.earlyStart = new Date(earlyStart).toUTCString();

    // console.log(graph.earlyStart.toUTCString());
    let beforeNodes: Activity[] = [];
    let nextNodes: Activity[] = [];
    beforeNodes = startNodes;
    do {
      beforeNodes.forEach(bn => {
        bn.outArrows = links.filter(l => l.source === bn.name).map(l => {
          const arr = new Arrow();
          arr.toActivity = tasks.filter(t => t.id === l.target).map(t => {
            const actv = new Activity();
            actv.name = t.text;
            actv.duration = t.duration;
            actv.earlyStart = t.start_date;
            actv.hoursSpent = t.progress;
            return actv;
          }).pop();
          // arr.fromActivity = bn;

// tslint:disable-next-line: radix
          arr.type = Number.parseInt(l.type);
          nextNodes.push(arr.toActivity);
          return arr;
        });
        // bn.outArrows.push(arr);

        beforeNodes = nextNodes;
        nextNodes = [];
      });
    } while (nextNodes.length > 0);
    console.log(graph);
    return graph;
    // const activities: Activity[] = [];
    // tasks.forEach(t => {
    //   const actv: Activity = {};
    //   // const outArrows: Arrow[] = [];
    //   // const inArrows: Arrow[] = [];
    //   actv.name = t.text;
    //   actv.inArrows = links.filter(l => l.target === t.id).map(il => {
    //     const arr = new Arrow();
    //     arr.type = ArrowType[il.type];
    //     arr.fromActivity;
    //   });
    //   activities.push(t);
    // });
  }
}
