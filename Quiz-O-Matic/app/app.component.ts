import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {  } from '@angular/router';


declare var $:any;

// to do : move login stuff to separate login component
@Component({
  selector: 'quiz-o-matic',
  template: '<router-outlet></router-outlet>'
})
export class AppComponent  { 
  
  public loaded:boolean = false;
  
  constructor() {
    // show content and hide loading message on app start
    $(".loading").hide();
    this.loaded = true;
  }
}
