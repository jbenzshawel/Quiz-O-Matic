import { Component, OnInit } from '@angular/core';

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
export class LoginFormComponent implements OnInit {
  public model: any = {};

  public showLogin:boolean = true;
  public showRegister:boolean = false;
  public passwordRequirements:boolean = false;
  public passwordReqText:string = "Password must be 8 characters, contain one upper case letter, and one special character";

  public error:string = "";
  public default: Default;

  regEmailSel: string = "#reg-email";
  regPasswordSel: string = "#reg-password";
  regConfirmPasswordSel: string = "#reg-confirm-password";
  
  loginForm: any; 
  registerForm:any;

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit() {
    // reset login status
    this.authenticationService.logout();
    this.default = new Default();
  }

  togglePassReq(): void {
    if (this.passwordRequirements) {
      this.passwordRequirements = false;
    } else {
      this.passwordRequirements = true;
    }
  }

  toggleRegister(): void {
    // reset form
    this.model.regEmail = "";
    this.model.regPassword= "";
    this.model.regConfirmPassword = "";
    this.default.clearErrors();    
    // toggle display
    this.showLogin = false;
    this.showRegister = true;

    // validate fields on change
    let scopedThis: any = this;    
    $("body").on("blur", this.regEmailSel, function() {
      scopedThis.validateRegEmail();
    });
    
    $("body").on("blur", this.regPasswordSel, function() {
      scopedThis.validateRegPassword();
    })

    $("body").on("blur", this.regConfirmPasswordSel, function() {
      scopedThis.validateRegConfirmPassword();
    })
  }

  toggleLogin(): void {
    // reset form
    this.model.username = "";
    this.model.password= "";
    this.default.clearErrors();
    // toggle display
    this.showLogin = true;
    this.showRegister = false;
  }
  
  loginUser(event: Event): void {
    event.preventDefault();
    this.default.clearErrors();
    this.authenticationService.login(this.model.username, this.model.password)
       .subscribe(result => {
                if (result === true) {
                    // login successful
                  //  this.router.navigate(['/dashboard']);
                } else {
                    // login failed
                    this.default.addError("#username", "Username or Password are incorrect");
                    this.default.addError("#password", "Username or Password are incorrect");
                }
            });
  }

  validateRegPassword(): boolean {
    let isValid: boolean = this.default.validatePassword(this.model.regPassword, this.regPasswordSel);
    if (!isValid) {
        this.passwordRequirements = true;
    } else {
      this.passwordRequirements = false;
    }
    return isValid;
  }

  validateRegEmail(): boolean {
    let isValid: boolean = this.default.validateEmail(this.model.regEmail);
    if (isValid) {
        this.default.removeError(this.regEmailSel);
    } else {
      this.default.addError(this.regEmailSel, "Invalid email address");
    }
    return isValid;
  }

  validateRegConfirmPassword(): boolean {
    let isValid: boolean = this.model.regPassword === this.model.regConfirmPassword;
    if (!isValid) {
        this.default.addError(this.regConfirmPasswordSel, "Password and Confirm Password do not match")
    } else {  
      this.default.removeError(this.regConfirmPasswordSel);
    }
    return isValid;
  }
}