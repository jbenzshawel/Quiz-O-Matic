import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthenticationService } from './services/authentication.service';
 
@Injectable()
export class AuthGuard implements CanActivate {
 
    constructor(private router: Router, private authenticationService: AuthenticationService) { }
 
    canActivate() {
        let authSession = sessionStorage.getItem('currentUser');
        if (authSession != null) {
            // logged in so return true
            return this.authenticationService.authenticated();
        }
        
        // to do: add return url
        // not logged in so redirect to login page
        this.router.navigate(['/login']);
        return false;
    }
}