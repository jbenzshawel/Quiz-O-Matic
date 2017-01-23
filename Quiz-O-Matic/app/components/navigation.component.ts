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
  dashboardActive: boolean = false;

  myQuizesActive: boolean = false;
  
  myResultsActive: boolean = false;

  constructor(private authenticationService: AuthenticationService, private router: Router) {
    let currentRoute: string = this.router.url.toLowerCase();
    switch (currentRoute) {
      case "/dashboard":
        this.dashboardActive = true;
        break;
      case "/quizes":
        this.myQuizesActive = true;
        break;
    }
  }

  ngOnInit() {
  }

  logout(event: Event):any {
      event.preventDefault()
      this.authenticationService.logout();
      return this.router.navigate(["/home"]);
  }
}