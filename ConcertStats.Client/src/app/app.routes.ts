import { Routes } from '@angular/router';
import { Home } from '../features/home/home';
import { Login } from '../features/account/login/login';
import { Register } from '../features/account/register/register';
import { Profile } from '../features/account/profile/profile';
import { authGuard } from '../core/guards/auth-guard';

export const routes: Routes = [
    { path: '', component: Home },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [authGuard],
        children: [
            { path: 'profile/:id', component: Profile },
        ]
    },
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    { path: '**', component: Home }
];
