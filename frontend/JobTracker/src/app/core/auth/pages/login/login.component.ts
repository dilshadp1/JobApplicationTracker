import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth-services/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;
  errorMessage = '';

  constructor(
    private fb : FormBuilder,
    private authService: AuthService,
    private router:Router
  ) 
  {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required,Validators.minLength(6)]]
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.loginForm.get(fieldName);
    return field ? (field.invalid && (field.dirty || field.touched)) : false;
  }

  onSubmit() {
    if (this.loginForm.invalid) 
      return;

    this.isLoading = true;
    this.errorMessage = '';

    const loginData = this.loginForm.value;

    this.authService.login(loginData).subscribe({
      next: (response) => {
        this.isLoading = false;

        const user = this.authService.userData();
        const role = user?.role
        if (role === 'Admin') {
          this.router.navigate(['/admin/dashboard']);
        } 
        else if (role === 'User') 
        { 
          this.router.navigate(['/user/dashboard']); 
        }
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Login error:', error);
        
        if (error.status === 400 || error.status === 401) {
          this.errorMessage = 'Invalid email or password.';
        } 
        else 
        {
          this.errorMessage = 'Something went wrong. Please try again later.';
        }
      }
    });
  }
}
