import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register-user',
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './register-user.component.html',
  styleUrl: './register-user.component.scss'
})
export class RegisterUserComponent {
  registerForm: FormGroup;
  isLoading = false;
  errorMessage = '';

  private authService = inject(AuthService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  constructor() {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      password: ['', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W]).{6,}$/)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (!password || !confirmPassword || !password.value || !confirmPassword.value) {
      return null;
    }

    return password.value !== confirmPassword.value ? { mismatch: true } : null;
  };

  get passwordValue(): string {
    return this.registerForm.get('password')?.value || '';
  }

  get hasMinLength() { return this.passwordValue.length >= 6; }
  get hasUpperCase() { return /[A-Z]/.test(this.passwordValue); }
  get hasLowerCase() { return /[a-z]/.test(this.passwordValue); }
  get hasNumber() { return /\d/.test(this.passwordValue); }
  get hasSymbol() { return /[\W]/.test(this.passwordValue); }

  get isPasswordValid() {
    return this.hasMinLength && 
           this.hasUpperCase && 
           this.hasLowerCase && 
           this.hasNumber && 
           this.hasSymbol;
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.registerForm.get(fieldName);
    return field ? (field.invalid && (field.dirty || field.touched)) : false;
  }

  onSubmit() {
    if (this.registerForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    const { confirmPassword, ...requestData } = this.registerForm.value;

    this.authService.register(requestData).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/login']);
      },
      error: (error: HttpErrorResponse) => {
        this.isLoading = false;
        console.error('Registration error:', error);
        
        const backendMsg = error.error?.error || error.error?.Error || error.error?.message;

        let validationMsg = '';
        if (error.error?.errors) {
          const firstKey = Object.keys(error.error.errors)[0];
          validationMsg = error.error.errors[firstKey]?.[0];
        }

        this.errorMessage = backendMsg || validationMsg || 'Registration failed. Please try again.';
      }
    });
  }
}
