import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'

import { AuthenticationService } from './../services/authentication.service';
import { Default } from './../classes/default';

declare var $:any;

@Component({
   moduleId: module.id,
   providers: [
     AuthenticationService
   ],
   selector: 'login-register-form',
   templateUrl: 'login-register-form.component.html'
})
export class LoginRegisterFormComponent implements OnInit {
   public model: any = {};
   public default: Default;
  
   public showLogin:boolean = true;
   public showRegister:boolean = false;
 
   public passwordRequirements:boolean = false;
   public passwordReqText:string = "Password must be 8 characters, contain one upper case letter, and one special character";
 
   // selectors for input form fields 
   usernameSel: string = "#username";
   passwordSel:string = "#password";
   regEmailSel: string = "#reg-email";
   regPasswordSel: string = "#reg-password";
   regConfirmPasswordSel: string = "#reg-confirm-password";
 
   constructor(private authenticationService: AuthenticationService, 
                private router: Router, private http: Http) { }
 
   ngOnInit() {
      this.default = new Default();
      this.model = {};
   }
 
   // displays the password requirements message
   togglePassReq(): void {
      if (this.passwordRequirements) {
         this.passwordRequirements = false;
      } else {
        this.passwordRequirements = true;
      }
   }
   
   // shows the registration form
   toggleRegister(): void {
      // reset form
      this.model.regEmail = "";
      this.model.regPassword= "";
      this.model.regConfirmPassword = "";
      this.default.clearErrors();    
      // toggle display
      this.showLogin = false;
      this.showRegister = true;
      this.passwordRequirements = false;
      // validate fields on change
      let that: any = this;    
      $("body").on("blur", this.regEmailSel, function() {
         if (that.model.regEmail != undefined && that.model.regEmail.length > 0)
            that.validateRegEmail();
      });
     
      $("body").on("blur", this.regPasswordSel, function() {
         if (that.model.regPassword != undefined &&that.model.regPassword.length > 0)
            that.validateRegPassword();
      });
 
      $("body").on("blur", this.regConfirmPasswordSel, function() {
         if (that.model.regConfirmPassword != undefined &&that.model.regConfirmPassword.length > 0)
            that.validateRegConfirmPassword();
     });
   }
  
   // shows the login form
   toggleLogin(): void {
      // reset form
      this.model.username = "";
      this.model.password= "";
      this.default.clearErrors();
      // toggle display
      this.showLogin = true;
      this.showRegister = false;
      // validate on field change
      let that = this;
      $("body").on("blur", this.usernameSel, function() {
         if (that.model.username != undefined && that.model.username.trim().length > 0)
            that.default.removeError(this.usernameSel);
      });
  
      $("body").on("blur", this.passwordSel, function() {
         if (that.model.password != undefined && that.model.password.trim().length > 0)
            that.default.removeError(this.passwordSel);
     });
   }
   
   // logs in a user and redirects them to the dashboard
   loginUser(event: Event): void {
      if (event != null)
         event.preventDefault();
      this.default.clearErrors();
      let isValid = true;
      let returnUrl = this.default.getQueryStringParam("returnUrl");
      if (returnUrl != null) {
         returnUrl = decodeURIComponent(returnUrl);
      }
      if (this.model.username.trim().length === 0) {
         isValid = false;
         this.default.addError(this.usernameSel, "The Username field is required");
      }
      if (this.model.password.trim().length === 0) {
         isValid = false;
         this.default.addError(this.passwordSel, "The Password field is required");
      }
      if (isValid) {
         this.authenticationService.login(this.model.username, this.model.password)
            .subscribe(result => {
               if (result === true) {
                   // login successful
                   if (returnUrl != null && returnUrl.length > 0) {
                     this.router.navigate([returnUrl]);
                   } else {
                     this.router.navigate(['/dashboard']);                        
                   }
               } else {
                   // login failed
                   this.default.addError("#username", "Username or Password are incorrect");
                   this.default.addError("#password", "Username or Password are incorrect");
               }
           });
     } // end if isValid 
     return;
   }
   
   // registers a user
   registerUser(event: Event): void {
      if (event != null)
         event.preventDefault();
   
      this.default.clearErrors();    
      // validate fields before creating user
      if (this.validateRegEmail() && this.validateRegPassword() && this.validateRegConfirmPassword()) {
         // set login model params since they will be needed to login the user after creating them
         this.model.username = this.model.regEmail;
         this.model.password = this.model.regPassword;
         let that = this;
         this.authenticationService.createUser(this.model.regEmail, this.model.regPassword, this.model.regConfirmPassword)
            .subscribe(status => {
               if (status === true) {
                  that.loginUser(null);
               }
            });
      } // end if valid
   }
   
   // validates and adds error of the registration password field
   validateRegPassword(): boolean {
      this.default.removeError(this.regPasswordSel);  

      let isValid: boolean = this.default.validatePassword(this.model.regPassword, this.regPasswordSel);
      if (!isValid) {
         this.passwordRequirements = true;
      } else {
         this.passwordRequirements = false;
      }
      return isValid;
   }
 
   // validates and adds error of the registration confirm password field   
   validateRegConfirmPassword(): boolean {
      this.default.removeError(this.regConfirmPasswordSel);

      let isValid: boolean = this.model.regPassword === this.model.regConfirmPassword;
      if (!isValid) {
         this.default.addError(this.regConfirmPasswordSel, "Password and Confirm Password do not match")
      } else {  
         this.default.removeError(this.regConfirmPasswordSel);
      }
     
     return isValid;
   }
   
   // validates registration email field is a valid email address and the 
   // username is not already in use (note username = email)
   validateRegEmail(): any {
      this.default.removeError(this.regEmailSel)
      
      let isValid: boolean = this.default.validateEmail(this.model.regEmail);
      if (isValid) {
         this.default.removeError(this.regEmailSel);
      } else {
         this.default.addError(this.regEmailSel, "Invalid email address");
      }
     
      if (isValid) {
         this.checkUsername(this.model.regEmail)
            .subscribe(nameExists => { 
              if (nameExists)
                isValid = false;
            });
      }
 
     return isValid;
   }
 
   
   // checks username to see if it is unique
   checkUsername(username: string): Observable<boolean> {
      let that = this;        
      let apiEndpoint: string = '//localhost:5000/api/account/UsernameExists/' + username;    
       
      return this.http.get(apiEndpoint)
                .map((response: Response) => {
                   let data = response.json();
                   if (data != null && data.hasOwnProperty("usernameExists")) {
                      if (data.usernameExists) {
                         that.default.addError(that.regEmailSel, "Username already exists");
                      }
                   }
 
                   return data.usernameExists;
               });
     } 
 
 }