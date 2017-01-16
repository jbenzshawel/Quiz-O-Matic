import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from './../services/authentication.service';

declare var $:any;

@Component({
  moduleId: module.id,
  providers: [
    AuthenticationService
  ],
  selector: 'app-navigation',
  templateUrl: 'navigation.component.html'
})
export class NavigationComponent implements OnInit {

  constructor(private authenticationService: AuthenticationService, private router: Router) { }

  ngOnInit() {
  }

  logout(event: Event):any {
      event.preventDefault()
      this.authenticationService.logout();
      return this.router.navigate(["/home"]);
  }
}