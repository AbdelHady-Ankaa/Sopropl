import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AlertifyService } from './Alertify.service';
import { AuthService } from './auth.service';
import { UploadService } from './upload.service';
import { forkJoin, Observable } from 'rxjs';
import { UserProfile } from '../_models/user-profile.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  constructor(
    private http: HttpClient,
    private alertify: AlertifyService,
    private auth: AuthService,
    private uploadService: UploadService
  ) { }

  removePhoto() {
    this.http.delete(this.baseUrl + 'profile/deletePhoto').subscribe(
      res => {
        this.alertify.success('Photo successfully deleted');
        this.auth.reloadToken();
      },
      err => {
        this.alertify.error(err);
      }
    );
  }

  getProfile() {
    return this.http.get<UserProfile>(this.baseUrl + 'profile');
  }

  updateProfile(model: UserProfile) {
    return this.http.put<UserProfile>(this.baseUrl + 'profile', model);
  }

  uploadPhoto(croppedImage: any) {
    const files: Set<Blob> = new Set();
    files.add(croppedImage);
    // When all progress-observables are completed...
    const progress = this.uploadService.upload(files, 'profile/setPhoto');
    const allProgressObservables = [];
    for (const key in progress) {
      if (progress.hasOwnProperty(key)) {
        allProgressObservables.push(progress[key].progress);
      }
    }
    forkJoin(allProgressObservables).subscribe(end => {
      this.auth.reloadToken();
    });
  }
}
