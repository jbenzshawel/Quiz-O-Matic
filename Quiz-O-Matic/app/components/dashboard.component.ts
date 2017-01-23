import { Component, OnInit } from '@angular/core';

import { AuthenticationService } from './../services/authentication.service';

@Component({
  moduleId: module.id,
   providers: [
    AuthenticationService
  ],
  templateUrl: 'dashboard.component.html'
})
export class DashboardComponent  {
  authenticated: boolean = false;

  public username: string = "";

    constructor(private authenticationService: AuthenticationService) {
      this.authenticated = authenticationService.authenticated();
      if (this.authenticated) {
        this.username = authenticationService.getUsername();
      }
    }
}