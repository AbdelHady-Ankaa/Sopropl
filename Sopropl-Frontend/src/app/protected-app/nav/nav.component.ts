import {
  Component,
  OnInit,
  HostListener,
  EventEmitter,
  Output
} from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from 'src/app/_models/User';
import { NotificationM } from 'src/app/_models/notification.model';
import { AuthService } from 'src/app/_services/auth.service';
import { ThemeService } from 'src/app/_services/theme.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { UsersService } from 'src/app/_services/users.service';
import { NotificationType } from 'src/app/_models/notification.model';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  constructor(
    public auth: AuthService,
    private themeService: ThemeService,
    private notificationService: NotificationService,
    private usersService: UsersService
  ) { }
  public NotificaitonT = NotificationType;
  public menuItems: any[];
  public user: User;
  public isDarkTheme: Observable<boolean>;
  private isOpnedSideNav: boolean;

  public notifications: BehaviorSubject<NotificationM[]>;
  @Output() toggleSideNav = new EventEmitter(true);

  public innerWidth: any;
  notis: NotificationM[];

  ngOnInit() {
    this.notifications = this.notificationService.notificationsSubject;
    this.getNotifications();
    this.isOpnedSideNav = false;
    this.innerWidth = window.innerWidth;
    this.auth.loggedInUser.subscribe(u => {
      this.user = u;
    });

    this.isDarkTheme = this.themeService.isDarkTheme;
  }
  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.innerWidth = window.innerWidth;
  }

  isNotMobileMenu() {
    if (window.innerWidth > 575) {
      return true;
    }
    return false;
  }
  logout() {
    this.auth.logoutUser();
  }

  acceptInvitation(orgId: string) {
    this.usersService.acceptInvitation(orgId);
  }

  notificationsLength() {
    return this.notifications.pipe(
      map(res => {
        return res.length;
      })
    );
  }
  getNotifications() {
    this.notificationService.getNotifications().subscribe(res => {
      this.notis = res;
    });
  }
  toggleDarkTheme(checked: boolean) {
    this.themeService.setDarkTheme(checked);
  }

  togglerSideNav() {
    this.isOpnedSideNav = !this.isOpnedSideNav;
    this.toggleSideNav.emit(this.isOpnedSideNav);
  }
}
