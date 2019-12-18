import { Routes, RouterModule } from '@angular/router';
import { ProjHomeComponent } from './proj-home/proj-home.component';
import { GraphComponent } from './graph/graph.component';
import { AccessListComponent } from './access-list/access-list.component';
import { ActivitiesComponent } from './activities/activities.component';
import { ActivitiesModule } from './activities/activities.module';
import { ProjResolver } from 'src/app/_resolvers/proj.resolver';
import { GraphResolver } from 'src/app/_resolvers/graph.resolver';

const routes: Routes = [
  { path: 'home', component: ProjHomeComponent, resolve: { proj: ProjResolver } },
  { path: 'graph', component: GraphComponent, resolve: { graph: GraphResolver } },
  { path: 'access-list', component: AccessListComponent },
  {
    path: 'activities/:actName', component: ActivitiesComponent,
    loadChildren: './activities/activities.module#ActivitiesModule' // () => ActivitiesModule
  }
];

export const ProjectsRoutes = RouterModule.forChild(routes);
