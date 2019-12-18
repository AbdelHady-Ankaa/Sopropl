import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Project } from 'src/app/_models/project.model';
import { AlertifyService } from 'src/app/_services/Alertify.service';

@Component({
  selector: 'app-org-home',
  templateUrl: './org-home.component.html',
  styleUrls: ['./org-home.component.scss']
})
export class OrgHomeComponent implements OnInit {
  projects: Project[] = [];

  constructor(private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(res => {
      this.projects = res.projects as Project[];
    }, err => {
      this.alertify.error(err);
    });
  }

}
