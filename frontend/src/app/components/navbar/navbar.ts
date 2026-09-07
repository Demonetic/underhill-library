import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  constructor(
    protected readonly authService: AuthService,
    private readonly router: Router
  ) {

  }

  logout(): void {
    this.authService.logout();
    void this.router.navigate(['/login']);
  }

}
