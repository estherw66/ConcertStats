import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { UserService } from '../../../core/services/user-service';
import { CreateUserRequest } from '../../../types/user';

@Component({
  selector: 'app-register',
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  private userService = inject(UserService);
  protected request = {} as CreateUserRequest;


  register() {
    console.log(this.request);
    this.userService.register(this.request).subscribe({
      next: res => {
        console.log('Server response:', res);
        // Optionally navigate or show a success message
        // Example: this.router.navigate(['/login']);
      },
      error: err => {
        console.error('Registration failed:', err);
        // Optionally show an error message to the user
      }
    });
  }
}
