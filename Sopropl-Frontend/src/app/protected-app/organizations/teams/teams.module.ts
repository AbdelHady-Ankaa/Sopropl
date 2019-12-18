import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TeamHomeComponent } from './team-home/team-home.component';
import { TeamsRoutes } from './teams.routing';
import { TeamMembersComponent } from './team-members/team-members.component';

@NgModule({
  imports: [
    CommonModule,
    TeamsRoutes
  ],
  declarations: [TeamHomeComponent, TeamMembersComponent]
})
export class TeamsModule { }
