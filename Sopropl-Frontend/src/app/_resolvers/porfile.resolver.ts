import { Injectable } from '@angular/core';

import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';

import { Observable, of } from 'rxjs';

import { catchError } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';
import { AlertifyService } from '../_services/Alertify.service';
import { UserProfile } from '../_models/user-profile.model';

@Injectable({
  providedIn: 'root'
})
export class ProfileResolver implements Resolve<UserProfile> {
  resolve(
    route: ActivatedRouteSnapshot
  ): UserProfile | Observable<UserProfile> | Promise<UserProfile> {
    return this.accountService.getProfile().pipe(
      catchError(() => {
        this.alertifyService.error('Problem retrieving data');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }

  /**
   *
   */
  constructor(
    private accountService: AccountService,
    private router: Router,
    private alertifyService: AlertifyService
  ) {}
}
