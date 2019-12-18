import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ResponseOk } from '../_extentions/server-ok.response';
import { environment } from 'src/environments/environment';
import { Project } from '../_models/project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  create(orgName: string, model: Project) {
    return this.http
      .post<ResponseOk>(this.baseUrl + orgName + '/projects', model);
  }

  getOne(orgName: string, projName: string) {
    return this.http
      .get<Project>(this.baseUrl + orgName + '/projects/' + projName);
  }

  getAll(orgName: string) {
    return this.http.get<Project[]>(this.baseUrl + orgName + '/projects');
  }

  update(orgName: string, projName: string, model: Project) {
    return this.http
      .put(this.baseUrl + orgName + '/projects/' + projName, model);
  }

  delelte(orgName: string, projName: string) {
    return this.http.delete<ResponseOk>(this.baseUrl + orgName + '/projects/' + projName);
  }

  changeAccess(orgName: string, projName: string, type: number, teamName?: string, userName?: string) {
    let query = '?';
    if (teamName) {
      query = query + 'teamName=' + teamName;
    } else {
      if (userName) {
        query = query + 'userName=' + userName;
      }
    }
    return this.http.post<ResponseOk>(this.baseUrl + orgName + '/projects/' + projName + query, []);
  }
}
