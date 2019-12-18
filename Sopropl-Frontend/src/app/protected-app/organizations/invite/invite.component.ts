import { Component, OnInit } from '@angular/core';
import { OrganizationService } from 'src/app/_services/organization.service';
import { AlertifyService } from 'src/app/_services/Alertify.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { UsersService } from 'src/app/_services/users.service';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { User } from 'src/app/_models/User';
import { ActivatedRoute } from '@angular/router';
import { SpinnerService } from 'src/app/_services/spinner.service';

@Component({
  selector: 'app-invite',
  templateUrl: './invite.component.html',
  styleUrls: ['./invite.component.scss']
})
export class InviteComponent implements OnInit {
  inviteMemebersForm: FormGroup;
  users: Observable<User[]>;
  invitedUsers: User[] = [];
  selectedUser: User;
  constructor(private orgService: OrganizationService,
    private aletifyService: AlertifyService,
    private usersService: UsersService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    public spinnerService: SpinnerService) { }

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
    this.inviteMemebersForm = this.formBuilder.group({
      userName: ['', Validators.required]
    });
  }

  addInvitedUser(user: User) {
    if (this.inviteMemebersForm.valid) {
      this.invitedUsers.push(user);
    }
  }

  removeInvitedUser(user: User) {
    this.orgService
      .removeInvitaion(this.route.snapshot.parent.params.orgName, user.userName).subscribe(res => {
        this.aletifyService.success(res.message);
      }, err => {
        this.aletifyService.error(err);
      });
    this.invitedUsers = this.invitedUsers.filter(
      u => u.userName !== user.userName
    );
  }

  invite(user?: User) {
    this.orgService
      .invite(
        user ? user.userName : this.selectedUser.userName,
        this.route.snapshot.parent.params.orgName
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
