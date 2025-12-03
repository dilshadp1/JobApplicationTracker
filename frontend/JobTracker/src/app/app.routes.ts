import { Routes } from '@angular/router';
import { authGuard } from './core/auth/guards/auth.guard';
import { MainLayoutComponent } from './core/layout/main-layout/main-layout.component';
import { roleGuard } from './core/auth/guards/role.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
    import('./core/auth/pages/login/login.component').then((m) => m.LoginComponent),
  },
  {
    path: 'register',
    loadComponent: () =>
    import('./core/auth/pages/register-user/register-user.component').then( (m) => m.RegisterUserComponent),
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
        loadComponent: () =>
        import('./features/home/home.component').then((m) => m.HomeComponent),
        pathMatch:'full'
      },
      {
        path: 'home',
        loadComponent: () =>
        import('./features/home/home.component').then((m) => m.HomeComponent)
      },
      {
        path: 'profile',
        canActivate:[authGuard,roleGuard],
        data: { roles: ['User','Admin'] },
        loadComponent: () =>
        import('./features/profile/profile/profile.component').then((m) => m.ProfileComponent),
        title: 'My Profile',
      },
    ]
  }
];
