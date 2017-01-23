import { Component, ViewChild, ElementRef, Renderer } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from './../services/authentication.service';

declare var $:any; 

@Component({
  moduleId: module.id,
   providers: [
    AuthenticationService
  ],
  selector: "quiz-o-matic",
  templateUrl: 'home.component.html'
})
export class HomeComponent  {
  public loaded:boolean = false;
  @ViewChild('createLink') clickElRef:ElementRef;

  constructor(private authenticationService: AuthenticationService, private router: Router, private renderer:Renderer) {
    // show content and hide loading message on app start
    $(".loading").hide();
    this.loaded = true;
    if (authenticationService.authenticated()) {
        router.navigate(["/dashboard"]);
    }
  }
}