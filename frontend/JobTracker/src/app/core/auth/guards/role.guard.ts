import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  
  const user = authService.userData(); 
  const expectedRoles: string[] = route.data['roles'];

  if (!user) {
    return router.createUrlTree(['/login']);
  }

  const hasRole = expectedRoles.includes(user.role);

  if (hasRole) {
    return true;
  }

  return router.createUrlTree(['/home']);
};
