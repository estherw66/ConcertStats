import { inject, Injectable } from '@angular/core';
import { AuthService } from './auth-service';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  private authService = inject(AuthService);

  init(): Observable<null> {
    const userString = localStorage.getItem('user');
    if (!userString) return of(null);

    const user = JSON.parse(userString);
    this.authService.setCurrentUser(user);
    
    return of(null);
  }
}
