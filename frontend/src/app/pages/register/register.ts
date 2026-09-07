import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';

import {AuthService} from '../../services/auth-service';
import { RegisterRequest} from '../../models/auth.models';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  errorMessage = '';
  isSubmitting = false;

  registerForm = new FormGroup({
    username: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(50)
      ]
    }),
    password: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(100)
      ]
    })
  });

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {

  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const request: RegisterRequest = this.registerForm.getRawValue();

    this.errorMessage = '';
    this.isSubmitting = true;

    this.authService.register(request).subscribe({
      next: () => {
        void this.router.navigate(['/login']);
      },
      error: (error) => {
        this.isSubmitting = false;

        if (error.status == 409) {
          this.errorMessage = 'Användarnamnet är redan registrerat.';
        } else {
          this.errorMessage = 'Registreringen misslyckades. Försök igen.'
        }
      }
    });
  }
}
