import { Component, OnInit } from '@angular/core';

import { AuthenticationService } from './../services/authentication.service';

@Component({
  moduleId: module.id,
   providers: [
    AuthenticationService
  ],
  selector: 'dashboard',
  templateUrl: 'dashboard.component.html'
})
export class DashboardComponent  {
  authenticated: boolean = false;

    constructor(private authenticationService: AuthenticationService) {
      this.authenticated = authenticationService.authenticated();
    }
}