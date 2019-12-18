import { Injectable } from '@angular/core';
import {
  CanLoad,
  Route,
  UrlSegment,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  Router,
  CanActivate
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanLoad, CanActivate {
  constructor(private auth: AuthService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    // return this.auth.loggedIn().pipe(
    //   map(res => {
    //     if (res) {
    //       return res;
    //     }
    //     this.router.navigate(['/auth/login']);
    //     return false;
    //   })
    // );
    if (this.auth.isUserLoggedIn()) {
      return true;
    }
    this.auth.setRedirectUrl(state.url);
    this.router.navigate([this.auth.getLoginUrl]);
    return false;
  }
  /**
   *
   */
  canLoad(
    route: Route,
    segments: UrlSegment[]
  ): Observable<boolean> | Promise<boolean> | boolean {
    if (this.auth.isUserLoggedIn()) {
      return true;
    }
    if (route.path === '') {
      // this.auth.setRedirectUrl('/home');

    }
    // this.auth.setRedirectUrl('/' + route.path);
    this.router.navigate(['/auth/login']);
    return false;
  }
}
