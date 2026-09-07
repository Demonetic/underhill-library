import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthService } from '../../services/auth-service';
import { LoginRequest } from '../../models/auth.models';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  errorMessage = '';
  isSubmitting = false;
  sessionExpired = false;

  loginForm = new FormGroup({
    username: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(50),
      ]
    }),
    password: new FormControl ('', {
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
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {
    this.sessionExpired = this.route.snapshot.queryParamMap.get('reason') === 'session-expired';
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const request: LoginRequest = this.loginForm.getRawValue();

    this.errorMessage = '';
    this.isSubmitting = true;

    this.authService.login(request).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token);
        void this.router.navigate(['/books']);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Felaktigt användarnamn eller lösenord.';
      }
    })
  }
}
