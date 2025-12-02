import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProfileService } from '../../../core/services/profile/profile.service';
import { take } from 'rxjs';
import { UserProfile } from '../../models/profile-model';

export interface ProfileForm {
  firstName: FormControl<string | null>;
  lastName: FormControl<string | null>;
  email: FormControl<string | null>;
  phone: FormControl<string | null>;
}

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})

export class ProfileComponent  implements OnInit {
  profileForm: FormGroup<ProfileForm>;
  
  isEditing = signal(false); 
  isLoading = signal(false);
  
  errorMessage = '';
  successMessage = '';

  private fb = inject(FormBuilder);
  private profileService = inject(ProfileService);

  constructor() {
    this.profileForm = this.fb.group({
      firstName: [{ value: '', disabled: true }],
      lastName: [{ value: '', disabled: true }],
      email: [{ value: '', disabled: true }], 
      phone: [{ value: '', disabled: true }, [Validators.required, Validators.pattern(/^\d{10}$/)]]
    });
  }

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
    this.isLoading.set(true);
    
    this.profileService.getProfile().pipe(take(1)).subscribe({
      next: (user) => {
        this.patchForm(user);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load profile', err);
        this.errorMessage = 'Failed to load profile data.';
        this.isLoading.set(false);
      }
    });
  }

  patchForm(user: UserProfile) {
    this.profileForm.patchValue(user);
  }

  toggleEdit() {
    this.isEditing.update(v => !v);
    this.successMessage = '';
    this.errorMessage = '';

    if (this.isEditing()) {
      this.profileForm.enable();
      
      this.profileForm.get('firstName')?.disable(); 
      this.profileForm.get('lastName')?.disable(); 
      this.profileForm.get('email')?.disable(); 
    } else {
      this.profileForm.disable();
      this.loadProfile(); 
    }
  }

  cancelEdit() {
    this.toggleEdit();
  }

  onSubmit() {
    if (this.profileForm.invalid) return;
    
    this.isLoading.set(true);
    this.errorMessage = '';
    this.successMessage = '';

    const rawValues = this.profileForm.getRawValue();

    const updateData: Partial<UserProfile> = {
      phone: rawValues.phone ?? ''
    };

    this.profileService.updateProfile(updateData).pipe(take(1)).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.successMessage = 'Phone number updated successfully!';
        
        this.isEditing.set(false);
        this.profileForm.disable();
        
        this.profileForm.patchValue({ phone: updateData.phone });
      },
      error: (err) => {
        console.error('Update failed', err);
        this.errorMessage = 'Failed to update profile. Please try again.';
        this.isLoading.set(false);
      }
    });
  }
}