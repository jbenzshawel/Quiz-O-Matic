import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Http, Headers, Response } from '@angular/http';
import { AuthenticationService } from './services/authentication.service';

@Component({
  selector: 'quiz-o-matic',
  templateUrl: 'app/app.component.html',
  styleUrls:  [ 'app/app.component.css']
})
export class AppComponent implements OnInit  { 
  model: any = {};

  public loaded:boolean = false;

  public error:string = "";
  
  constructor(private authenticationService: AuthenticationService) {
    // show content and hide loading message on app start
    $(".loading").hide();
      this.loaded = true;
    // window.setTimeout(function() {
      
    // }.bind(this), 900);
  }

  ngOnInit() {
    // reset login status
    this.authenticationService.logout();
  }
  
  loginUser(): any
  {
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
