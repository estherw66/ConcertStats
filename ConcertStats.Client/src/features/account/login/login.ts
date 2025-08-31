import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';
import { LoginRequest } from '../../../types/auth';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  protected authService = inject(AuthService);
  private router = inject(Router);
  protected loginRequest = {} as LoginRequest;

  login() {
    this.authService.login(this.loginRequest).subscribe({
      next: () => {
        this.router.navigateByUrl(`profile/${this.authService.currentUser()?.id}`);
        this.loginRequest = {} as LoginRequest;
      }, error: err => {
        console.error('Login error:', err);
      }
    })
  }

  logout() {
    this.authService.logout();
    this.router.navigateByUrl('/');
  }
}
