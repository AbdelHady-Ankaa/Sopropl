import {
  Resolve,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { OrganizationService } from '../_services/organization.service';
import { Organization } from '../_models/organization.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OrgsResolver implements Resolve<Organization[]> {
  resolve(route: ActivatedRouteSnapshot): Observable<Organization[]> {
    return this.orgServie.getAll().pipe(map(res => {
      return res;
    }));
  }
  /**
   *
   */
  constructor(
    private orgServie: OrganizationService
  ) { }
}
