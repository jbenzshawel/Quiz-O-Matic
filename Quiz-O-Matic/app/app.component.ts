import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
declare var $:any;

// to do : move login stuff to separate login component
@Component({
  selector: 'quiz-o-matic',
  templateUrl: 'app/app.component.html',
  styleUrls:  [ 'app/app.component.css']
})
export class AppComponent  { 
  
  public loaded:boolean = false;
  
  constructor() {
    // show content and hide loading message on app start
    $(".loading").hide();
    this.loaded = true;
  }
}
