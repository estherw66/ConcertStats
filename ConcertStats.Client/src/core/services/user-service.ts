import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { CreateUserRequest, User } from '../../types/user';
import { tap } from 'rxjs';
import { AuthService } from './auth-service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  private authService = inject(AuthService);

  baseUrl = 'https://localhost:7207/api/users/';

  register(request: CreateUserRequest) {
    return this.http.post<User>(this.baseUrl, request).pipe(
      tap(user => {
        if (user) {
          this.authService.setCurrentUser(user);
        }
      })
    )
  }
}

