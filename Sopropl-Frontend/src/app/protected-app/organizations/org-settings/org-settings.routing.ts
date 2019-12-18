import { Routes, RouterModule } from '@angular/router';
import { OrgProfileComponent } from './org-profile/org-profile.component';
import { OrgResolver } from 'src/app/_resolvers/org.resolver';

const routes: Routes = [
  { path: 'profile', component: OrgProfileComponent },
];

export const OrgSettingsRoutes = RouterModule.forChild(routes);
