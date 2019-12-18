import { Project } from '../_models/project.model';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { ProjectService } from '../_services/project.service';
import { map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Graph } from '../_models/graph.model';
import { ActivityService } from '../_services/activity.service';
import { Activity } from '../_models/activity.model';

@Injectable({
  providedIn: 'root'
})
export class GraphResolver implements Resolve<Graph> {
  constructor(private graphService: ActivityService) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Graph | Observable<Graph> | Promise<Graph> {
    return this.graphService
      .getGraph(route.parent.parent.params.orgName as string, route.parent.params.projName as string)
      .pipe(map(res => {
        return res;
      }));
  }
}
