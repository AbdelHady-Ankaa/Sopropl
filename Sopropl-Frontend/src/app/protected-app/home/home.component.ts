import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { OrganizationService } from 'src/app/_services/organization.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { Organization } from 'src/app/_models/organization.model';
import { AlertifyService } from 'src/app/_services/Alertify.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private titleService: Title,
    private organizationService: OrganizationService,
    private notificationService: NotificationService,
    private alertifyService: AlertifyService
  ) { }

  organizations: Organization[];
  ngOnInit() {
    this.route.data.subscribe(data => {
      this.titleService.setTitle(data.title);
      this.organizations = data.orgs as Organization[];
    });
    // this.organizationService.getAll().subscribe(res => {
    //   this.organizations = res;
    // }, err => {
    //   this.alertifyService.error(err);
    // });
    this.notificationService.startConnection();
  }
}
