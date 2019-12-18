import { Component, OnInit, ViewChild } from '@angular/core';
import { ImgCropperComponent } from './img-cropper/img-cropper.component';
import { MatDialog } from '@angular/material';
import { User } from 'src/app/_models/User';
import { AuthService } from 'src/app/_services/auth.service';
import { AccountService } from 'src/app/_services/account.service';
import { Title } from '@angular/platform-browser';
import { Router, ActivatedRoute } from '@angular/router';
import { UserProfile } from 'src/app/_models/user-profile.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/Alertify.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  constructor(
    public dialog: MatDialog,
    private auth: AuthService,
    private accountService: AccountService,
    private titleService: Title,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private alertify: AlertifyService
  ) {}
  user: User;
  userProfile: UserProfile;
  profileForm: FormGroup;

  createProfileForm() {
    this.profileForm = this.formBuilder.group(
      {
        name: [
          '',
          [
            Validators.maxLength(40),
            Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9_ ]+$')
          ]
        ],
        address: ['', []],
        city: [
          '',
          [
            Validators.minLength(3),
            Validators.maxLength(28),
            Validators.pattern('^[a-zA-Z]+$')
          ]
        ],
        country: [
          '',
          [
            Validators.minLength(3),
            Validators.maxLength(50),
            Validators.pattern('^[a-zA-Z]+$')
          ]
        ],
        bio: ['', [Validators.maxLength(1500)]],
        postalCode: [
          '',
          [
            Validators.pattern('^[0-9]+$'),
            Validators.minLength(5),
            Validators.maxLength(5)
          ]
        ]
      },
      {
        validator: (form: FormGroup) => {
          return null;
        }
      }
    );
  }
  ngOnInit() {
    this.route.data.subscribe(data => {
      this.titleService.setTitle(data.title);
    });
    this.auth.loggedInUser.subscribe(u => {
      this.user = u;
    });
    this.createProfileForm();
    this.loadProfile();
  }
  public openUploadDialog() {
    this.alertify.dialog(ImgCropperComponent);
  }

  public removePhoto() {
    this.accountService.removePhoto();
  }

  public updateProfile() {
    if (this.profileForm.valid) {
      this.userProfile = Object.assign({}, this.profileForm.value);
      this.accountService.updateProfile(this.userProfile).subscribe(
        () => {
          this.alertify.success('Profile updated successfully');
        },
        error => {
          this.alertify.error(error);
        }
      );
    } else {
      this.alertify.warning('Please check your input and verify it');
    }
  }

  public loadProfile() {
    this.accountService.getProfile().subscribe(userProfile => {
      this.userProfile = userProfile;
      this.profileForm.get('name').setValue(this.userProfile.name);
      this.profileForm.get('address').setValue(this.userProfile.address);
      this.profileForm.get('postalCode').setValue(this.userProfile.postalCode);
      this.profileForm.get('bio').setValue(this.userProfile.bio);
      this.profileForm.get('city').setValue(this.userProfile.city);
      this.profileForm.get('country').setValue(this.userProfile.country);
    });
  }
}
