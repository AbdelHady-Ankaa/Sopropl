import {
  Component,
  OnInit,
  ViewChild,
  ChangeDetectorRef,
  OnDestroy
} from '@angular/core';
import { AuthService } from './_services/auth.service';
import { ThemeService } from './_services/theme.service';
import { Observable } from 'rxjs';
import { MatSidenav } from '@angular/material';
import { MediaMatcher } from '@angular/cdk/layout';
import { OrganizationService } from './_services/organization.service';
import { AlertifyService } from './_services/Alertify.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public isDarkTheme: Observable<boolean>;
  @ViewChild('sidenav') sideNav: MatSidenav;

  mobileQuery: MediaQueryList;

  constructor(
    private auth: AuthService,
    private themeService: ThemeService,
    media: MediaMatcher,
    private organizationService: OrganizationService,
    public alertifyService: AlertifyService
  ) {
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
  }

  ngOnInit(): void {
    this.auth.reloadToken();
    this.isDarkTheme = this.themeService.isDarkTheme;
  }
  loggedIn() {
    return this.auth.isUserLoggedIn();
  }
  togglerSideNav(toggleSideNav: boolean) {
    this.sideNav.toggle();
  }
}
