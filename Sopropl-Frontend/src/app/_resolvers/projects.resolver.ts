import { Project } from '../_models/project.model';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { ProjectService } from '../_services/project.service';
import { map } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProjectsResolver implements Resolve<Project[]> {
  constructor(private projectService: ProjectService) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Project[] | Observable<Project[]> | Promise<Project[]> {

    return this.projectService.getAll(route.parent.params.orgName as string).pipe(map(res => {
      return res;
    }));
  }
}
