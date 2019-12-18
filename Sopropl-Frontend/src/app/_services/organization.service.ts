import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Organization } from '../_models/organization.model';
import { BehaviorSubject, Observable, forkJoin } from 'rxjs';
import { AlertifyService } from './Alertify.service';
import { ResponseOk } from '../_extentions/server-ok.response';
import { map } from 'rxjs/operators';
import { User } from '../_models/User';
import { UploadService } from './upload.service';

@Injectable({
  providedIn: 'root'
})
export class OrganizationService {
  private baseUrl = environment.apiUrl;
  public orgs = new BehaviorSubject<Organization[]>(null);
  public org = new BehaviorSubject<Organization>(null);
  public invitedUsers = new BehaviorSubject<User[]>(null);
  constructor(private http: HttpClient, private uploadService: UploadService, private alertifyService: AlertifyService) { }

  getOne(orgaizationName: string) {
    const result = this.http.get<Organization>(this.baseUrl + 'organizations/' + orgaizationName);

    return result;
  }
  getAll() {
    const result = this.http.get<Organization[]>(this.baseUrl + 'organizations');

    return result;
  }
  create(org: Organization) {
    const result = this.http.post<ResponseOk>(this.baseUrl + 'organizations', org);


    return result;
  }

  update(oldOrgName: string, organization: Organization) {
    const result = this.http.put<ResponseOk>(this.baseUrl + 'organizations/' + oldOrgName, organization);

    return result;
  }

  remove(orgName: string) {
    const result = this.http.delete<ResponseOk>(this.baseUrl + 'organizations');


    return result;
  }

  allInvitedUsers(orgaizationName: string) {
    const result = this.http.get<User[]>(this.baseUrl + 'Organizations/' + orgaizationName + '/allInvitedUsers');

    return result;
  }

  invite(invitedUserName: string, orgaizationName: string) {
    const result = this.http.post<ResponseOk>(
      this.baseUrl +
      'Organizations/' +
      orgaizationName +
      '/invite?invitedUserName=' +
      invitedUserName,
      []
    );
    return result;
  }


  removeInvitaion(orgaizationName: string, invitedUserName) {
    const result = this.http.delete<ResponseOk>(
      this.baseUrl +
      'Organizations/'
      + orgaizationName
      + '/removeInvitation?invitedUserName='
      + invitedUserName);

    return result;
  }

  removeMember(orgName: string, userName: string) {
    const result = this.http.delete<ResponseOk>(this.baseUrl + 'Organizations/' + orgName + '?userName=' + userName);

    return result;
  }

  allMembers(orgName: string, role?: number) {
    let query = '';
    if (role) {
      query = query + '?role=' + role;
    }
    return this.http.get<User[]>(this.baseUrl + 'Organizations/' + orgName + query);
  }

  setMemberRole(orgName: string, userName: string, role: number) {
    return this.http.post<ResponseOk>(this.baseUrl + 'Organizations' + orgName +
      '?userName=' + userName + '&role=' + role, []);
  }
  uploadLogo(orgName: string, croppedImage: any) {
    const files: Set<Blob> = new Set();
    files.add(croppedImage);
    // When all progress-observables are completed...
    const progress = this.uploadService
      .upload(files, 'Organizations/' + orgName + '/setLogo');
    const allProgressObservables = [];
    for (const key in progress) {
      if (progress.hasOwnProperty(key)) {
        allProgressObservables.push(progress[key].progress);
      }
    }
    forkJoin(allProgressObservables).subscribe(res => {
      this.reloadOrg();
    });
  }

  deleteLogo(orgName: string) {
    return this.http.delete<ResponseOk>(this.baseUrl + 'Organizations/' + orgName + '/deleteLogo');
  }

  reloadOrg() {
    this.org.subscribe(org => {
      if (org != null) {
        if (org.name != null) {
          this.getOne(org.name);
        }
      }
    });
  }
}
