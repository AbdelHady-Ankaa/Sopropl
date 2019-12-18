import { Routes, RouterModule } from '@angular/router';
import { TeamHomeComponent } from './team-home/team-home.component';
import { TeamMembersComponent } from './team-members/team-members.component';

const routes: Routes = [
  { path: 'home', component: TeamHomeComponent },
  { path: 'members', component: TeamMembersComponent }
];

export const TeamsRoutes = RouterModule.forChild(routes);
