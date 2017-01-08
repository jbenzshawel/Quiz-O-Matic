import { Component } from '@angular/core';

@Component({
  selector: 'quiz-o-matic',
  templateUrl: 'app/app.component.html'
  //,styleUrls:  []
})
export class AppComponent  { 
  name = 'Quiz-O-Matic'; 
  
  public loaded:boolean = false;

  constructor() {
    // show content and hide loading message on app start
    window.setTimeout(function() {
      this.loaded = true;
    }.bind(this), 1250);
  }
}
