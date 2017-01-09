import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'

// auth service modified from http://jasonwatmore.com/post/2016/08/16/angular-2-jwt-authentication-example-tutorial 
// to rely on Identity Authenticaiton Cookie instead of local storage 
@Injectable()
export class AuthenticationService {
    public token: string;
    
    private authKey: string; 

    constructor(private http: Http) {
        // set token if saved in session storage
        var currentUser = JSON.parse(sessionStorage.getItem('currentUser'));
        this.token = currentUser && currentUser.token;
    }
 
    public login(username: string, password: string): Observable<boolean> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        let body = JSON.stringify({ Email: username, Password: password });
        return this.http.post('//localhost:5000/api/account/login', body, options)
            .map((response: Response) => {
                // login successful if there's a jwt token in the response
                let loginResult = response.json();
                let authKey: string = null;

                if (loginResult != null && loginResult.result.succeeded) {
                    // get auth cookie
                    this.authKey = loginResult.authKey;
                    let token: string = this.getCookie(this.authKey);
 
                    // store username and jwt token in session storage to keep user logged in between page refreshes
                    sessionStorage.setItem('currentUser', JSON.stringify({ username: username, token: token }));
 
                    // return true to indicate successful login
                    return true;
                } else {
                    // return false to indicate failed login
                    return false;
                }
            });
    }
    
    // returns true if user is authenticated
    public authenticated(): boolean
    {
        let sessionToken = sessionStorage.getItem("currentUser");
        let cookieToken = this.getCookie(this.authKey);

        return sessionToken == cookieToken;
    }

    public logout(): void {
        // clear token remove user from session storage to log user out
        this.token = null;
        sessionStorage.removeItem('currentUser');
    }

    public getCookie(name: string): string {
        let value: string = "; " + document.cookie;
        let parts: string[] = value.split("; " + name + "=");
        let result: string = null;

        if (parts.length == 2) 
            result = parts.pop().split(";").shift();
        
        return result;
    }
}