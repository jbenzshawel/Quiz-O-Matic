// @angular
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute }               from '@angular/router';
// models
import { QuizResult }                         from './../models/quiz.result.model';
// services and helpers
import { Common }                      from './../classes/common';

// declare globals for ts compiler 
declare var window: any;

@Component({
  moduleId: module.id,
  templateUrl: 'quiz-result.component.html'
})
export class QuizResultComponent implements OnInit, OnDestroy  {
  /////////////////////////////////////////////////////////////
  /// Public Properties
  
  public quizResult: QuizResult = null;

  /////////////////////////////////////////////////////////////
  /// Private Properties

  private _sub: any;
   
  private _common: Common; 

  ////////////////////////////////////////////////////////////
  /// Constructor and ngOnInit / ngOnDestroy

   constructor(private _activatedRoute: ActivatedRoute) {}

  ngOnInit() {
    this.quizResult = new QuizResult();
    this._common = new Common();    
    this._sub = this._activatedRoute.params.subscribe(params => {
      this._setResult(params["id"]);
    });
  }

  ngOnDestroy() {
    this._common = null;
    this.quizResult = null;
  }

  private _setResult(id:string): void {
    let storageResultString:string = window.sessionStorage.getItem(this.quizResult.storageKey.concat(id));
    let parsedStorageResult:any = this._common.parseJsonObject(storageResultString);

    if (this._common.hasProperties(parsedStorageResult, ["title", "content", "imagePath"])) {
      this.quizResult.title = parsedStorageResult.title;
      this.quizResult.content = parsedStorageResult.content;
      this.quizResult.imagePath = parsedStorageResult.imagePath;
    }
  }
}