import { Component, inject, OnInit, output, signal } from '@angular/core';
import { RegisterCreds } from '../../../types/user';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../../../core/services/account-service';
import { JsonPipe } from '@angular/common';
import { TextInput } from '../../../shared/text-input/text-input';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, TextInput],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register implements OnInit {
  
  protected creds = {} as RegisterCreds;
  cancelRegister = output<boolean>();
  private accountService = inject(AccountService);
  private router = inject(Router)
  
  protected credentialsForm: FormGroup = new FormGroup({})
  protected profileForm: FormGroup = new FormGroup({})
  protected currentStep = signal(1)

  protected validationErrors = signal<string[]>([])
  
  
  ngOnInit(): void {
    this.initializeForm()
  }

  initializeForm()
  {
    this.credentialsForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      displayName: new FormControl('', Validators.required),
      password: new FormControl('',[Validators.required, Validators.minLength(4), Validators.maxLength(10)]),
      confirmPassword: new FormControl('', [Validators.required, Validators.minLength(4),
      Validators.maxLength(10), this.matchPasswords('password')])
    })

    this.profileForm = new FormGroup({
      gender: new FormControl('male', Validators.required),
      dateOfBirth: new FormControl('', Validators.required),
      city: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required)
    })

    // za da imame non-stop matching pomegju passwords, bezrazlika koe prvin go editneme (pass ili confirm pass)
    this.credentialsForm.controls['password'].valueChanges.subscribe(() => {
      this.credentialsForm.controls['confirmPassword'].updateValueAndValidity()
    })

  }

  nextStep() {
    if(this.credentialsForm.valid)
    {
      this.currentStep.update(currStep => currStep + 1)
    }
  }

  prevStep()
  {
    this.currentStep.update(currStep => currStep - 1)
  }

  // Custom validator za matching pass
  matchPasswords(matchTo: string): ValidatorFn
  {
    return (control: AbstractControl): ValidationErrors | null => {

      const parent = control.parent       // control e input fieldot na koj sto se primenuva funkcijava
      if(!parent) return null             // control.parent e formGroup, so cel da najdeme vrednosta matchTo

      const matchValue = parent.get(matchTo)?.value   
      return control.value === matchValue ? null : {passwordMismatch: true}
    }
  }

  getMaxDate(){
    const today = new Date()
    today.setFullYear(today.getFullYear() - 18)
    return today.toISOString().split('T')[0]
  }

  register() {

    if(this.credentialsForm.valid && this.profileForm.valid)
    {
      const formData = {...this.credentialsForm.value, ...this.profileForm.value}
      
      this.accountService.register(formData).subscribe({
        next: () => {
          this.router.navigateByUrl('/members')
        },
        error: error => {
          console.log(error);
          this.validationErrors.set(error)
        }
      })
    }

  
  }

  cancel() {
    this.cancelRegister.emit(false); // emitnuva event
  }

}
