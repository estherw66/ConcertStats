import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../core/services/user-service';
import { CreateUserRequest } from '../../../types/user';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  private userService = inject(UserService);
  private authService = inject(AuthService);
  private router = inject(Router);
  protected request = {} as CreateUserRequest;


  register() {
    console.log(this.request);
    this.userService.register(this.request).subscribe({
      next: res => {
        console.log('Server response:', res);
        this.router.navigateByUrl(`profile/${this.authService.currentUser()?.id}`);
        this.request = {} as CreateUserRequest;
      },
      error: err => {
        console.error('Registration failed:', err);
      }
    });
  }
}
