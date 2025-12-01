import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { JobApplicationService } from '../../../core/services/job-application/job-application.service';

@Component({
  selector: 'app-job-add',
  imports: [ReactiveFormsModule],
  templateUrl: './job-add.component.html',
  styleUrl: './job-add.component.scss',
})
export class JobAddComponent {
  jobForm = new FormGroup({
    company: new FormControl(''),
    position: new FormControl(''),
    jobUrl: new FormControl(''),
    salaryExpectation: new FormControl(''),
    notes: new FormControl(''),
  });

  constructor(private jobApplicationService: JobApplicationService, private router : Router) {}

  public onSubmit(){
    const jobData=this.jobForm.value;
    // this.jobApplicationService.addJob(jobData).subscribe(()=>
    // {
    //   this.router.navigate(['/jobs']);
    // });
    
  }
}
