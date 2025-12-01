import { Component, inject, OnInit, signal } from '@angular/core';
import { MainLayoutComponent } from "../../../core/layout/main-layout/main-layout.component";
import { RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  isEditing = signal(false); // Controls View vs Edit mode
  isLoading = false;
  initials = signal('');

  private fb = inject(FormBuilder);
  // private profileService = inject(ProfileService);

  constructor() {
    this.profileForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required]],
      email: [{ value: '', disabled: true }], // Locked forever
      phone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]]
    });
  }

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
    this.isLoading = true;
    // For demo purposes, we can mock if backend isn't ready
    // Replace this with actual service call: this.profileService.getProfile().subscribe(...)
    
    // MOCK DATA (Remove this block when backend is ready)
    // setTimeout(() => {
    //   const mockData: UserProfile = {
    //     firstName: 'Tinu',
    //     lastName: 'Clara',
    //     email: 'tinu@gmail.com',
    //     phone: '9876543210'
    //   };
    //   this.patchForm(mockData);
    //   this.isLoading = false;
    // }, 1000);
  }

  // patchForm(user: UserProfile) {
  //   this.profileForm.patchValue(user);
  //   this.initials.set(`${user.firstName[0]}${user.lastName[0]}`.toUpperCase());
  // }

  toggleEdit() {
    this.isEditing.update(v => !v);
    if (this.isEditing()) {
      this.profileForm.enable();
      this.profileForm.get('email')?.disable(); 
    } else {
      this.profileForm.disable(); 
    }
  }

  onSubmit() {
    if (this.profileForm.invalid) return;
    
    this.isLoading = true;
    const updateData = this.profileForm.getRawValue(); // Include disabled fields if needed

    console.log('Saving Profile:', updateData);
    
    // Simulate API Call
    setTimeout(() => {
      this.isLoading = false;
      this.toggleEdit(); // Exit edit mode
      // Show success toast here
    }, 1500);
  }

  cancelEdit() {
    this.toggleEdit();
    this.loadProfile(); // Reset data
  }
}