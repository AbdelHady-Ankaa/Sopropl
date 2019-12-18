import { Project } from '../_models/project.model';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { ProjectService } from '../_services/project.service';
import { map } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProjResolver implements Resolve<Project> {
  constructor(private projectService: ProjectService) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Project | Observable<Project> | Promise<Project> {
    return this.projectService
      .getOne(route.parent.parent.params.orgName as string, route.parent.params.projName as string)
      .pipe(map(res => {
        return res;
      }));
  }
}
