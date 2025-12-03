import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { authGuard } from './core/auth/guards/auth.guard';
import { MainLayoutComponent } from './core/layout/main-layout/main-layout.component';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./core/auth/pages/login/login.component').then(
        (m) => m.LoginComponent
      ),
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./core/auth/pages/register-user/register-user.component').then(
        (m) => m.RegisterUserComponent
      ),
  },
  {
    path: '',
    component: MainLayoutComponent, 
    children: [
  
    {
      path: 'user',
      canActivate:[authGuard],
      loadChildren: () =>
        import('./features/user/user.routes').then((m) => m.userRoutes),
    },
    {
      path: '',
      component:HomeComponent,
      pathMatch:'full'
    },
    {
      path: 'home',
      component:HomeComponent,
      pathMatch:'full'
    },
  ]
  }
];
