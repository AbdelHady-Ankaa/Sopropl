import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { User } from 'src/app/_models/User';
import { OrganizationService } from 'src/app/_services/organization.service';
import { AlertifyService } from 'src/app/_services/Alertify.service';
import { UsersService } from 'src/app/_services/users.service';
import { SpinnerService } from 'src/app/_services/spinner.service';
import { map } from 'rxjs/operators';
import { MatStepper } from '@angular/material';
import { Organization } from 'src/app/_models/organization.model';

@Component({
  selector: 'app-new-org',
  templateUrl: './new-org.component.html',
  styleUrls: ['./new-org.component.scss']
})
export class NewOrgComponent implements OnInit {
  isLinear = false;
  createOrganizationForm: FormGroup;
  inviteMemebersForm: FormGroup;
  users: Observable<User[]>;
  invitedUsers: User[] = [];
  selectedUser: User;

  constructor(
    private formBuilder: FormBuilder,
    private organizationService: OrganizationService,
    private aletifyService: AlertifyService,
    private usersService: UsersService,
    public spinnerService: SpinnerService
  ) { }

  searchInUsersByUserName($event) {
    if (this.inviteMemebersForm.get('userName').valid) {
      const userName: string = this.inviteMemebersForm.get('userName').value;
      this.usersService.search(userName);
    } else {
      this.usersService.updateUsersSearchList([]);
    }
  }

  notExist() {
    return this.users.pipe(
      map(users => {
        if (
          users.length === 0 ||
          this.inviteMemebersForm.get('userName').value === 'notExist'
        ) {
          return true;
        }
        return false;
      })
    );
  }

  clearUsersList() {
    this.usersService.updateUsersSearchList([]);
  }

  disableInvite() {
    return this.users.pipe(
      map(users => {
        if (
          users.find(
            u => u.userName === this.inviteMemebersForm.get('userName').value
          ) === undefined
        ) {
          return true;
        }
        return false;
      })
    );
  }

  ngOnInit() {
    this.users = this.usersService.usersSearchList;
    let usersList: User[];
    this.users.subscribe(res => {
      usersList = res;
    });
    this.createOrganizationForm = this.formBuilder.group({
      name: ['', Validators.required]
    });
    this.inviteMemebersForm = this.formBuilder.group({
      userName: ['', Validators.required]
    });
  }

  createOrganization(stepper: MatStepper) {
    if (this.createOrganizationForm.valid) {
      const model: Organization = Object.assign(
        {},
        this.createOrganizationForm.value
      );
      this.organizationService.create(model).subscribe(
        res => {
          this.aletifyService.success(res.message);
          this.goForward(stepper);
        }, err => {
          this.aletifyService.error(err);
        }
      );
    }
  }

  goBack(stepper: MatStepper) {
    stepper.previous();
  }

  goForward(stepper: MatStepper) {
    stepper.next();
  }

  addInvitedUser(user: User) {
    if (this.inviteMemebersForm.valid) {
      this.invitedUsers.push(user);
    }
  }

  removeInvitedUser(user: User) {
    this.organizationService
      .removeInvitaion(this.createOrganizationForm.get('name').value, user.userName).subscribe(res => {
        this.aletifyService.success(res.message);
      }, err => {
        this.aletifyService.error(err);
      });
    this.invitedUsers = this.invitedUsers.filter(
      u => u.userName !== user.userName
    );
  }

  invite(user?: User) {
    this.organizationService
      .invite(
        user ? user.userName : this.selectedUser.userName,
        this.createOrganizationForm.get('name').value
      )
      .subscribe(
        res => {
          this.addInvitedUser(user ? user : this.selectedUser);
        },
        err => {
          this.aletifyService.error(err);
        }
      );
  }
}
