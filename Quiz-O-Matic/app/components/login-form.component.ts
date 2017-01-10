import { Component, OnInit } from '@angular/core';

import { AuthenticationService } from './../services/authentication.service';

@Component({
  moduleId: module.id,
  providers: [
        AuthenticationService
  ],
  selector: 'login-form',
  templateUrl: 'login-form.component.html'
})
export class LoginFormComponent implements OnInit {
  model: any = {};

  public error:string = "";

  constructor(private authenticationService: AuthenticationService) {
  }

  ngOnInit() {
    // reset login status
    this.authenticationService.logout();
  }
  
  loginUser(event: Event): void
  {
    event.preventDefault();
    this.error = "";
    this.authenticationService.login(this.model.username, this.model.password)
       .subscribe(result => {
                if (result === true) {
                    // login successful
                  //  this.router.navigate(['/dashboard']);
                } else {
                    // login failed
                    this.error = 'Username or password is incorrect';
                }
            });
  }
}