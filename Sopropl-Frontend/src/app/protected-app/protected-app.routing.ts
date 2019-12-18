import { Routes, RouterModule } from '@angular/router';
import { OrganizationsModule } from './organizations/organizations.module';
import { HomeComponent } from './home/home.component';
import { SettingsModule } from './settings/settings.module';
import { SettingsComponent } from './settings/settings.component';
import { OrganizationsComponent } from './organizations/organizations.component';
import { NewOrgComponent } from './new-org/new-org.component';
import { OrgResolver } from '../_resolvers/org.resolver';
import { OrgsResolver } from '../_resolvers/orgs.resolver';

const routes: Routes = [
  {
    path: 'settings', // for users
    component: SettingsComponent,
    loadChildren: /* () => SettingsModule */ './settings/settings.module#SettingsModule'
  },
  {
    path: 'organizations/:orgName',
    component: OrganizationsComponent,
    resolve: {},
    loadChildren: './organizations/organizations.module#OrganizationsModule' // () => OrganizationsModule,
  },
  { path: 'home', component: HomeComponent, resolve: { orgs: OrgsResolver } },
  { path: 'new-org', component: NewOrgComponent }


];

export const ProtectedAppRoutes = RouterModule.forChild(routes);
