import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { catchError, map, Observable, tap, throwError } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest, TokenPayload } from '../models/auth.models';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `https://localhost:7126/api/Auth`; 
  private readonly tokenKey = 'access_token';

  currentUser = signal<boolean>(false);
  userData = signal<TokenPayload | null>(null);


  constructor(private http: HttpClient) 
  {
    this.restoreSession();
  }

  register(request: RegisterRequest): Observable<any> {
    return this.http.post(`https://localhost:7126/api/User/register`, request);
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request,{withCredentials:true}).pipe(
      tap(response => {
        if (response.accessToken) {
          this.setSession(response.accessToken);
        }
      }),
      catchError(error => {
        console.error('Login Failed:', error);
        this.logout();
        return throwError(() => error); 
      })
    );
  }
  

  logout(): void {
    this.http.post(`${this.apiUrl}/logout`, {}, { withCredentials: true }).subscribe({
      error: (err) => console.error('Logout API failed', err)
    });
    
    localStorage.removeItem(this.tokenKey);
    this.currentUser.set(false);
    this.userData.set(null);
  }

  refreshToken(): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/refresh-token`, {}, { 
      withCredentials: true 
    }).pipe(
      tap(response => {
        this.setSession(response.accessToken);
      }),
      catchError(error => {
        this.logout();
        return throwError(() => error);
      })
    );
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private setSession(token: string): void {
    localStorage.setItem(this.tokenKey, token);
    
    const payload = this.decodeToken(token);
    
    this.userData.set(payload);
    this.currentUser.set(true);
  }

  private restoreSession(): void {
    const token = this.getToken();
    if (!token) return;

    const payload = this.decodeToken(token);
    
    if (payload && this.isTokenExpired(payload.exp)) {
      console.warn('Token expired. Logging out.');
      this.logout();
      return;
    }

    if (payload) {
      this.userData.set(payload);
      this.currentUser.set(true);
    }
  }

  private decodeToken(token: string): TokenPayload | null {
    try {
      return jwtDecode<TokenPayload>(token);
    } catch (error) {
      console.error('Failed to decode token', error);
      return null;
    }
  }


  private isTokenExpired(exp: number): boolean {
    return (Math.floor(Date.now() / 1000)) >= exp;
  }

}
