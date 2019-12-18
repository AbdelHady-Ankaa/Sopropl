import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { User } from '../_models/User';
import { BehaviorSubject } from 'rxjs';
import { AlertifyService } from './Alertify.service';
import { ResponseOk } from '../_extentions/server-ok.response';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private baseUrl = environment.apiUrl;
  private usersSearchListSubject = new BehaviorSubject<User[]>([]);
  public usersSearchList = this.usersSearchListSubject.asObservable();

  public updateUsersSearchList(users: User[]) {
    this.usersSearchListSubject.next(users);
  }

  constructor(private http: HttpClient, private alertifyService: AlertifyService) { }

  search(text: string) {
    return this.http
      .get<User[]>(this.baseUrl + 'users?' + 'userName=' + text).subscribe(res => { this.updateUsersSearchList(res); });
  }

  acceptInvitation(organizationId: string) {
    return this.http.post<ResponseOk>(
      this.baseUrl + 'users/acceptInvitation?organizationId=' + organizationId,
      null
    ).subscribe(res => {
      this.alertifyService.success(res.message);
    }, err => {
      this.alertifyService.error(err);
    });
  }
}
