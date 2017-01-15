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

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit() {
    // reset login status
    this.authenticationService.logout();
    this.default = new Default();
    this.model = {};
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
    })

    $("body").on("blur", this.regConfirmPasswordSel, function() {
      if (that.model.regConfirmPassword != undefined &&that.model.regConfirmPassword.length > 0)
        that.validateRegConfirmPassword();
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
    // validate on field change
    let that = this;
    $("body").on("blur", this.usernameSel, function() {
      if (that.model.username != undefined && that.model.username.trim().length > 0)
        this.default.removeError(this.usernameSel);
    });

    $("body").on("blur", this.passwordSel, function() {
      if (that.model.password != undefined && that.model.password.trim().length > 0)
        this.default.removeError(this.passwordSel);
    })

  }
  
  loginUser(event: Event): void {
    event.preventDefault();
    this.default.clearErrors();
    let isValid = true;
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
                    //  this.router.navigate(['/dashboard']);
                  } else {
                      // login failed
                      this.default.addError("#username", "Username or Password are incorrect");
                      this.default.addError("#password", "Username or Password are incorrect");
                  }
              });
    } 
    return;
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