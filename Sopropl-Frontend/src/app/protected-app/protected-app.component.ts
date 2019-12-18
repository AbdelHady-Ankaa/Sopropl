import { Component, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { OrganizationService } from 'src/app/_services/organization.service';
import { MatSidenav } from '@angular/material';
import { AuthService } from '../_services/auth.service';
import { ThemeService } from '../_services/theme.service';
import { MediaMatcher } from '@angular/cdk/layout';
import { AlertifyService } from '../_services/Alertify.service';
import { NotificationService } from '../_services/notification.service';

@Component({
  selector: 'app-protected-app',
  templateUrl: './protected-app.component.html',
  styleUrls: ['./protected-app.component.scss']
})
export class ProtectedAppComponent implements OnInit {
  public isDarkTheme: Observable<boolean>;
  @ViewChild('sidenav') sideNav: MatSidenav;

  mobileQuery: MediaQueryList;

  constructor(
    private auth: AuthService,
    private themeService: ThemeService,
    media: MediaMatcher,
    public alertifyService: AlertifyService,
    private notifyService: NotificationService
  ) {
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
  }

  ngOnInit(): void {
    this.auth.reloadToken();
    this.notifyService.startConnection();
    this.isDarkTheme = this.themeService.isDarkTheme;
  }
  loggedIn() {
    return this.auth.isUserLoggedIn();
  }
  togglerSideNav(toggleSideNav: boolean) {
    this.sideNav.toggle();
  }
}
