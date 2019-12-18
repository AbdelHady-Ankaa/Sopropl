import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Team } from '../_models/team.model';
import { ResponseOk } from '../_extentions/server-ok.response';
import { User } from '../_models/User';
import { Access } from '../_models/access.model';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAll(orgName: string) {
    return this.http.get<Team[]>(this.baseUrl + orgName + '/teams');
  }

  getOne(orgName: string, teamName: string) {
    return this.http.get<Team>(this.baseUrl + orgName + '/teams/' + teamName);
  }

  Create(orgName: string, model: Team) {
    return this.http.post<ResponseOk>(this.baseUrl + orgName + '/teams', model);
  }

  delete(orgName: string, teamName: string) {
    return this.http
      .delete<ResponseOk>(this.baseUrl + orgName + '/teams/' + teamName);
  }

  allMembers(orgName: string, teamName: string) {
    return this.http.get<User[]>(this.baseUrl + orgName + '/teams/' + teamName + '/allMembers');
  }

  addMember(orgName: string, teamName: string, userName: string) {
    return this.http
      .post<ResponseOk>(this.baseUrl + orgName + '/teams/' +
        teamName + '/addMember' + '?userName=' + userName, []);
  }

  deleteMember(orgName: string, teamName: string, userName: string) {
    return this.http
      .delete<ResponseOk>(this.baseUrl + orgName + '/teams/' +
        teamName + '/deleteMember' + '?userName=' + userName);
  }

  allAccess(orgName: string, teamName: string, type?: number) {
    let query = '';
    if (type) {
      query = query + '?type=' + type;
    }
    return this.http
      .get<Access[]>(this.baseUrl + orgName + '/temas/' + teamName + query);
  }
}
