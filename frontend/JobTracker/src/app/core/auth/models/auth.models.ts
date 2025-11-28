export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phone: string;
}

export interface AuthResponse {
  token: string;
}

export interface User {
  id: number;
  email: string;
  role: string;
}

export interface TokenPayload {
  sub: string;
  email: string;
  role: string;
  exp: number; 
}