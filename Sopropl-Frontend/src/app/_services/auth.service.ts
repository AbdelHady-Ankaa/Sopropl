import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { StorageService } from './storage.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../_models/User';
import { AsyncPipe } from '@angular/common';
import { async } from '@angular/core/testing';

export function tokenGetter() {
  return JSON.parse(localStorage.getItem('access_token'));
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private http: HttpClient,
    private storage: StorageService,
    private jwtHelper: JwtHelperService,
    private router: Router
  ) {
    this.loggedInUser.subscribe(res => {
      this.user = res;
    });
  }
  private user: User;
  private baseUrl = environment.apiUrl;

  decodedToken: any;
  private userBehaviorSubject = new BehaviorSubject<User>(null);
  loggedInUser: Observable<User> = this.userBehaviorSubject.asObservable();
  private redirectUrl = '/';
  private loginUrl = '/auth/login';

  updateCurrentUser(user: User) {
    this.userBehaviorSubject.next(user);
  }

  login(model: any) {
    this.http
      .post(this.baseUrl + 'auth/' + 'login', model)
      .subscribe((res: any) => {
        this.storage.setItem('access_token', res.tokenString);
        this.decodedToken = this.jwtHelper.decodeToken(res.tokenString) as {
          nameid: string;
          unique_name: string;
        };
        this.updateCurrentUser(res.user as User);
        this.router.navigate(['/home']);
      });
  }

  logoutUser() {
    this.storage.clear();
    this.router.navigate(['/auth/login']);
  }

  isUserLoggedIn() {
    return !this.jwtHelper.isTokenExpired(this.storage.getItem('access_token'));
  }

  loggedIn() {
    return this.loggedInUser.pipe(
      map(res => {
        if (
          res &&
          !this.jwtHelper.isTokenExpired(this.storage.getItem('access_token'))
        ) {
          return true;
        }
        return false;
      })
    );
  }

  reigster(model: any) {
    return this.http.post(this.baseUrl + 'auth/' + 'register', model);
  }

  async reloadToken() {
    if (await this.isUserLoggedIn()) {
      this.decodedToken = this.jwtHelper.decodeToken(
        this.storage.getItem('access_token')
      );
      this.getLoggedInUser();
    }
  }

  private getLoggedInUser() {
    return this.http.get(this.baseUrl + 'account').subscribe(res => {
      this.updateCurrentUser(res as User);
    });
  }
  getRedirectUrl(): string {
    return this.redirectUrl;
  }
  setRedirectUrl(url: string): void {
    this.redirectUrl = url;
  }
  getLoginUrl(): string {
    return this.loginUrl;
  }
}
