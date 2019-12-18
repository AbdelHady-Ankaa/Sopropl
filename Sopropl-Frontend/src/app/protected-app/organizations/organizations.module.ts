import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrganizationsRoutes } from './organizations.routing';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/material.module';
import { SoproplCloudinaryModule } from 'src/app/sopropl-cloudinary.module';
import { OrgSettingsComponent } from './org-settings/org-settings.component';
import { ProjectsComponent } from './projects/projects.component';
import { OrgHomeComponent } from './org-home/org-home.component';
import { TeamsComponent } from './teams/teams.component';
import { MembersComponent } from './members/members.component';
import { NewTeamComponent } from './new-team/new-team.component';
import { OrgTeamsComponent } from './org-teams/org-teams.component';
import { NewProjComponent } from './new-proj/new-proj.component';
import { InviteComponent } from './invite/invite.component';


@NgModule({
  imports: [
    CommonModule,
    OrganizationsRoutes,
    MaterialModule,
    ReactiveFormsModule,
    SoproplCloudinaryModule,
  ],
  declarations: [
    OrgSettingsComponent,
    ProjectsComponent,
    OrgHomeComponent,
    TeamsComponent,
    MembersComponent,
    NewTeamComponent,
    OrgTeamsComponent,
    NewProjComponent,
    InviteComponent
  ],
  providers: []
})
export class OrganizationsModule { }
