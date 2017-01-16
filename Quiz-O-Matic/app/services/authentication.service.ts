import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs';
import { Default } from './../classes/default';

import 'rxjs/add/operator/map'

// auth service modified from http://jasonwatmore.com/post/2016/08/16/angular-2-jwt-authentication-example-tutorial 
// to rely on ASP.NET Identity Authenticaiton Cookie instead of local storage 
@Injectable()
export class AuthenticationService {
    public token: string;

    public isLockedOut: boolean;

    public isNotAllowed: boolean;

    public requiresTwoFactor: boolean;
    
    private authKey: string = ".AspNetCore.Identity.Application"; 

    private cookieLength: number = 5;

    private default: Default;

    constructor(private http: Http) {
        // set token if saved in session storage
        var currentUser = JSON.parse(sessionStorage.getItem('currentUser'));
        this.token = currentUser && currentUser.token;
        this.default = new Default();
    }
    
    // submits login request and completes front end post authenticatin
    public login(username: string, password: string): Observable<boolean> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        let body = JSON.stringify({ Email: username, Password: password });

        return this.http.post('//localhost:5000/api/account/login', body, options)
            .map((response: Response) => {
                let loginResult = response.json();
                let authKey: string = null;
                let signInResult: any = null;
                let token: string = null;

                // set auth service properties based on Identity sign in result
                if (loginResult != null && 
                    loginResult.hasOwnProperty("signInResult")) {
                    signInResult = loginResult.signInResult;
                    this.isLockedOut = signInResult.isLockedOut;
                    this.isNotAllowed = signInResult.isNotAllowed;
                    this.requiresTwoFactor = signInResult.requiresTwoFactor;
                    this.token = loginResult.token;
                    this.authKey = loginResult.authKey;
                }
                else {
                    // if we did not get the signInResult in the response see if the lockOut property was set
                    this.isLockedOut = loginResult.hasOwnProperty("lockOut") ? 
                                        loginResult.lockOut : 
                                        false;
                    // since there was not a signInResult set succeeded to false
                    signInResult = { succeeded : false };
                }

                // complete post authentication if this user was authenticated 
                if (signInResult != null && signInResult.succeeded && this.token != null) {
                    this.createCookie(this.authKey, this.token, this.cookieLength);
                    // store username and jwt token in session storage to keep user logged in between page refreshes
                    sessionStorage.setItem('currentUser', JSON.stringify({ username: username, token: this.token }));
                } 

                return signInResult.succeeded;
            });
    }

    // submits login request and completes front end post authenticatin
    public createUser(username: string, password: string, confirmPassword:string): boolean {
        let body = JSON.stringify({ Email: username, Password: password, ConfirmPassword: confirmPassword });
        let callback = (response: any) => {
            let status:boolean = false;
            if (response != null && response.hasOwnProperty("succeeded")) {
                status = response.succeeded;
                if (status) {
                    this.login(username, password);
                }
            }
            return status;
        };
        let settings = {
            url: '//localhost:5000/api/account/register',
            data: body,
            success: callback
        }

        return this.default.post(settings);
        
    }
    
    // returns true if user is authenticated
    public authenticated(): boolean {
        let currentUser = sessionStorage.getItem("currentUser");
        let sessionToken = currentUser != null ? JSON.parse(currentUser).token : "";
        let cookieToken = this.getCookie(".AspNetCore.Identity.Application");

        return sessionToken == cookieToken;
    }

    public logout(refresh:boolean = false): void {
        // clear token and remove user from session storage 
        let settings: any = {
            url: "//localhost:5000/api/account/logoff"
        };
        if (refresh) {
            settings.success = () => {
                window.location.reload();
            }
        }
        try {
            this.default.get(settings);            
        }
        catch (ex) {}
        this.token = null;
        sessionStorage.removeItem('currentUser');
        this.deleteCookie(this.authKey);
    }

    // returns a cookie with key name or null if the cookie does not exist
    public getCookie(name: string): string {
        let value: string = "; " + document.cookie;
        let parts: string[] = value.split("; " + name + "=");
        let result: string = null;

        if (parts.length == 2) 
            result = parts.pop().split(";").shift();
        
        return result;
    }

    private createCookie(name:string,value:string,days: number): void {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days*24*60*60*1000));
            var expires = "; expires=" + date.toUTCString();
        }
        else var expires = "";
        document.cookie = name + "=" + value + expires + "; path=/";
    }

    private deleteCookie(name:string): void {
        document.cookie = name +'=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    }

}