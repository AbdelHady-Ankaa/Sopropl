import { Routes, RouterModule } from '@angular/router';
import { OrgSettingsModule } from './org-settings/org-settings.module';
import { OrgSettingsComponent } from './org-settings/org-settings.component';
import { OrgHomeComponent } from './org-home/org-home.component';
import { ProjectsComponent } from './projects/projects.component';
import { ProjectsModule } from './projects/projects.module';
import { TeamsComponent } from './teams/teams.component';
import { TeamsModule } from './teams/teams.module';
import { MembersComponent } from './members/members.component';
import { NewTeamComponent } from './new-team/new-team.component';
import { OrgTeamsComponent } from './org-teams/org-teams.component';
import { ProjectsResolver } from 'src/app/_resolvers/projects.resolver';
import { OrgResolver } from 'src/app/_resolvers/org.resolver';
import { NewProjComponent } from './new-proj/new-proj.component';
import { InviteComponent } from './invite/invite.component';
// import { OrgHomeResolver } from 'src/app/_resolvers/org-home.resolver';
const routes: Routes = [
  {
    path: 'settings', component: OrgSettingsComponent,
    loadChildren: './org-settings/org-settings.module#OrgSettingsModule', // () => OrgSettingsModule,
    resolve: { org: OrgResolver }
  },
  { path: 'projects', component: OrgHomeComponent, resolve: { projects: ProjectsResolver } },
  {
    path: 'projects/:projName', component: ProjectsComponent,
    loadChildren: './projects/projects.module#ProjectsModule' // () => ProjectsModule
  },
  { path: 'teams', component: OrgTeamsComponent },
  { path: 'teams/:teamName', component: TeamsComponent, loadChildren: () => TeamsModule },
  { path: 'members', component: MembersComponent },
  { path: 'new-team', component: NewTeamComponent },
  { path: 'new-project', component: NewProjComponent },
  { path: 'invite', component: InviteComponent }
];

export const OrganizationsRoutes = RouterModule.forChild(routes);
