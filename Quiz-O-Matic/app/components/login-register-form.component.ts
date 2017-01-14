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
      if (scopedThis.default.validateEmail(scopedThis.model.regEmail)) {
        scopedThis.default.removeError(scopedThis.regEmailSel);
      } else {
        scopedThis.default.addError(scopedThis.regEmailSel, "Invalid email address");
      }
    });
    
    $("body").on("blur", this.regPasswordSel, function() {
      if (!scopedThis.default.validatePassword(scopedThis.model.regPassword, scopedThis.regPasswordSel)) {
        scopedThis.passwordRequirements = true;
      } else {
        scopedThis.passwordRequirements = false;
      }
    })

    $("body").on("blur", this.regConfirmPasswordSel, function() {
      if (scopedThis.model.regPassword != scopedThis.model.regConfirmPassword) {
        scopedThis.default.addError(scopedThis.regConfirmPasswordSel, "Password and Confirm Password do not match")
      } else {
        scopedThis.default.removeError(scopedThis.regConfirmPasswordSel);
      }
    })
  }

  toggleLogin(): void {
    // reset form
    this.model.username = "";
    this.model.password= "";
    this.default.clearErrors();
    // toggle displat
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
}