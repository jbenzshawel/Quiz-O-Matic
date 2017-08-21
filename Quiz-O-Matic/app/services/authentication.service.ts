import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs';
import { Default } from './../classes/default';

import 'rxjs/add/operator/map';

// ToDo: Refactor to use local storage instead of session storage or just be cookie based?
// auth service modified from http://jasonwatmore.com/post/2016/08/16/angular-2-jwt-authentication-example-tutorial 
// to rely on ASP.NET Identity Authenticaiton Cookie instead of local storage 
@Injectable()
export class AuthenticationService {
    // property for JWt 
    public token: string;

    public isLockedOut: boolean;

    public isNotAllowed: boolean;

    public requiresTwoFactor: boolean;

    // key used for auth cookie
    private _authKey: string = ".AspNetCore.Identity.Application";

    // number of days until cookie expires 
    private _cookieLength: number = 1;

    private _default: Default;

    private _headers: Headers = new Headers({ 'Content-Type': 'application/json' });

    constructor(private http: Http) {
        // set token if saved in session storage
        let currentUser = JSON.parse(sessionStorage.getItem('currentUser'));
        this.token = currentUser && currentUser.token;
        this._default = new Default();
    }

    // returns user object (username and token) from session storage
    private getUserSession(): any {
        let currentUser = sessionStorage.getItem("currentUser");
        let userObject = currentUser != null ? JSON.parse(currentUser) : null;

        return userObject;
    }

    // submits login request and completes front end post authenticatin
    public login(username: string, password: string): Observable<boolean> {
        let options = new RequestOptions({ headers: this._headers });
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
                    this._authKey = loginResult.authKey;
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
                    this.createCookie(this._authKey, this.token, this._cookieLength);
                    // store username and jwt token in session storage to keep user logged in between page refreshes
                    sessionStorage.setItem('currentUser', JSON.stringify({ username: username, token: this.token }));
                    
                    return signInResult.succeeded;
                    
                } 

                // if we get this far sign in result unsuccessful. no response from server
                return false;
            });
    }

    // submits login request and completes front end post authenticatin
    public createUser(username: string, password: string, confirmPassword:string): Observable<boolean> {
        let options = new RequestOptions({ headers: this._headers });
        let body = JSON.stringify({ Email: username, Password: password, ConfirmPassword: confirmPassword });        

        return this.http.post('//localhost:5000/api/account/register', body, options)
            .map((response: Response) => {
                let status:boolean = false;
                let result = response.json();
                if (result != null && result.hasOwnProperty("succeeded")) {
                    status = result.succeeded;
                }
                return status;
            });
    }
    
    // returns true if user is authenticated
    public authenticated(): boolean {
        let userObject = this.getUserSession();
        if (userObject == null) {
            return false;
        }
        let sessionToken = userObject.token;
        let cookieToken = this.getCookie(".AspNetCore.Identity.Application");

        return sessionToken == cookieToken;
    }

    // returns current user's username if a user is authenticated
    // else returns empty string
    public getUsername(): string {
        let userObject: any = this.getUserSession();
        let username: string = "";
        if (userObject != null) {
            username = userObject.username
        }
        
        return username;
    }

    // logs out current user 
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
        // clear token and cookies
        this.token = null;
        sessionStorage.removeItem('currentUser');
        this.deleteCookie(this._authKey);
        // log out of identity session
        this.http.get("//localhost:5000/api/account/logoff")
           .map((res: Response) => {
               if (res.status < 400) {
                  if (refresh) {
                     window.location.reload();
                  }
                  
                  return true;                
               }
               
               return false;
           })
           .subscribe();
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

    // creates a cookie with the name, value, and number of days passed in
    private createCookie(name:string,value:string,days: number): void {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days*24*60*60*1000));
            var expires = "; expires=" + date.toUTCString();
        }
        else var expires = "";
        document.cookie = name + "=" + value + expires + "; path=/";
    }
    
    // deletes a cookie with the passed in name
    private deleteCookie(name:string): void {
        document.cookie = name +'=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    }

}