import { Routes, RouterModule } from '@angular/router';
import { ActHomeComponent } from './act-home/act-home.component';
import { ResourcesComponent } from './resources/resources.component';

const routes: Routes = [
  { path: 'home', component: ActHomeComponent },
  { path: 'resources', component: ResourcesComponent }
];

export const ActivitiesRoutes = RouterModule.forChild(routes);
