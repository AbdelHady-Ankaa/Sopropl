import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Graph } from '../_models/graph.model';
import { ResponseOk } from '../_extentions/server-ok.response';
import { Activity } from '../_models/activity.model';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  assignToTeam(orgName: string, projName: string, actvName: string, teamName: string) {
    return this.http
      .post<ResponseOk>(this.baseUrl + orgName + '/' + projName +
        '/graph/' + actvName + '?teamName=' + teamName, []);
  }

  getOne(orgName: string, projName: string, actvName: string, teamName: string) {
    return this.http.get<Activity>(this.baseUrl + orgName + '/' + projName + '/graph/' + actvName);
  }

  getGraph(orgName: string, projName: string) {
    return this.http.get<Graph>(this.baseUrl + orgName + '/' + projName + '/graph');
  }

  create(orgName: string, projName: string, model: Graph) {
    console.log(model);
    return this.http
      .post<Graph>(this.baseUrl + orgName + '/' + projName + '/graph', model);
  }

  delete(orgName: string, projName: string) {
    return this.http
      .delete<ResponseOk>(this.baseUrl + orgName + '/' + projName + '/graph');
  }
}
