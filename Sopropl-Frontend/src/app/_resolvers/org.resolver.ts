import {
  Resolve,
  ActivatedRouteSnapshot,
  Router
} from '@angular/router';
import { AlertifyService } from '../_services/Alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { OrganizationService } from '../_services/organization.service';
import { Organization } from '../_models/organization.model';

@Injectable({
  providedIn: 'root'
})
export class OrgResolver implements Resolve<Organization> {
  resolve(route: ActivatedRouteSnapshot): Observable<Organization> {
    console.log(route.params.orgName);
    return this.orgServie.getOne(route.params.orgName as string);
  }
  /**
   *
   */
  constructor(
    private router: Router,
    private alertifyService: AlertifyService,
    private orgServie: OrganizationService
  ) { }
}
