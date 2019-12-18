import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UnprotectedAppModule } from './unprotected-app/unprotected-app.module';
import { ProtectedAppModule } from './protected-app/protected-app.module';
import { AuthGuard } from './_guards/auth.guard';
import { UnprotectedAppComponent } from './unprotected-app/unprotected-app.component';
import { ProtectedAppComponent } from './protected-app/protected-app.component';
import { UnauthGuard } from './_guards/unauth.guard';
import { LoginComponent } from './unprotected-app/login/login.component';
import { RegisterComponent } from './unprotected-app/register/register.component';

const routes: Routes = [
  {
    path: 'auth',
    component: UnprotectedAppComponent,
    data: { title: 'Authentication' },
    canActivate: [UnauthGuard],
    loadChildren: './unprotected-app/unprotected-app.module#UnprotectedAppModule' // () => UnprotectedAppModule
  },
  {
    path: '',
    component: ProtectedAppComponent,
    canLoad: [AuthGuard],
    loadChildren: './protected-app/protected-app.module#ProtectedAppModule' , // () => ProtectedAppModule,
    data: { title: 'Sopropl' }
  },

  // children: [
  //   { path: 'login', component: LoginComponent, data: { title: 'login' } },
  //   {
  //     path: 'register',
  //     component: RegisterComponent,
  //     data: { title: 'Register' }
  //   },
  //  {
  //   path: '**',
  //   redirectTo: '/auth/login',
  //   pathMatch: 'full'
  // }

  // ]
  // 'src/app/unprotected-app/unprotected-app.module#UnprotectedAppModule' // 'src/' // () => UnprotectedAppModule
  // }
  // , {
  //   path: '**',
  //   redirectTo: '/auth/login',
  //   pathMatch: 'full'
  // }
];

@NgModule({
  imports: [RouterModule.forRoot(routes  /*, { enableTracing: true }*/)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
