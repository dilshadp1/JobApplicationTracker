import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth-services/auth.service';
import { inject } from '@angular/core';
import { BehaviorSubject, catchError, filter, switchMap, take, throwError } from 'rxjs';
import { Router } from '@angular/router';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router =inject(Router)
  const token = authService.getToken();

  let authReq = req;
  if (token) {
      authReq = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {   
      if (error.status === 401 && !authReq.url.includes('/refresh-token') && !authReq.url.includes('/login')) { 
         if (!isRefreshing) {
          isRefreshing = true;
          refreshTokenSubject.next(null);   
        
          return authService.refreshToken().pipe(
          switchMap((response) => {
             isRefreshing = false;
             const newToken = response.accessToken;
             refreshTokenSubject.next(newToken);


             return next(req.clone({
                setHeaders: { Authorization: `Bearer ${newToken}` }
              }));
            }),
          catchError((refreshErr) => {
            isRefreshing = false;
            authService.logout();
            router.navigate(['/login']);
            return throwError(() => refreshErr);
          })
        );
      }
      else {
          return refreshTokenSubject.pipe(
            filter(token => token !== null), 
            take(1), 
            switchMap(token => {
              return next(req.clone({
                setHeaders: { Authorization: `Bearer ${token}` }
              }));
            })
          );
        }
      }

      return throwError(() => error);
    })
  );
};