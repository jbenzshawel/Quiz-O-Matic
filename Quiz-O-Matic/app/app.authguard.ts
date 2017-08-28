import { Injectable }                            from '@angular/core';
import { Router, CanActivate, NavigationExtras } from '@angular/router';
import { AuthenticationService }                 from './services/authentication.service';
 
@Injectable()
export class AuthGuard implements CanActivate {
 
    constructor(private router: Router, private authenticationService: AuthenticationService) { }
 
    canActivate() {
        let authSession = sessionStorage.getItem('currentUser');
        let status: boolean = false;
        if (authSession != null) {
            // logged in so return true
            status = this.authenticationService.authenticated();
        }
        
        if (!status) {
            let returnUrl: string = encodeURIComponent(window.location.pathname);
            // that contains our global query params and fragment
            let navigationExtras: NavigationExtras = {
                queryParams: { 'returnUrl': returnUrl }
            };

            // not logged in so redirect to login page
            this.router.navigate(['/home'], navigationExtras);
        }
        
        return status;
    }
}