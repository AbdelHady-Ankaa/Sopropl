import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { Injectable } from '@angular/core';
import { tokenGetter } from './auth.service';
import { AlertifyService } from './Alertify.service';
import { environment } from 'src/environments/environment';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { NotificationM } from '../_models/notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private baseUrl = environment.apiUrl;
  private hubConnection: HubConnection;
  public notificationsSubject = new BehaviorSubject<NotificationM[]>([]);
  private notificationsList: NotificationM[] = [];

  public updateNotifications(notifications: NotificationM[]) {
    this.notificationsSubject.next(notifications);
  }

  constructor(
    private alertifyService: AlertifyService,
    private http: HttpClient
  ) { }

  getNotifications() {
    return this.http.get<NotificationM[]>(this.baseUrl + 'account/getNotifications');
  }
  public startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.baseUrl + 'Notification', {
        accessTokenFactory: tokenGetter,
        transport: signalR.HttpTransportType.ServerSentEvents
      })
      .build();

    this.hubConnection
      .start()
      .then(() => { })
      .catch(err =>
        this.alertifyService.error('Error while starting connection: ' + err)
      );
    this.hubConnection.on('notify', (res: NotificationM) => {
      console.log(res.data);
      this.alertifyService.message(res.body, res.title, res.sentDate);
      this.notificationsList.push(res);
      this.updateNotifications(this.notificationsList);
    });
  }
}
