// @angular
import { Component, OnInit, OnDestroy } from '@angular/core';
// models
import { Quiz }                         from './../models/quiz.model';
// services and helpers
import { DataService }                  from './../services/data.service';
import { Default }                      from './../classes/default';

@Component({
  moduleId: module.id,
   providers: [
    DataService
  ],
  templateUrl: 'quiz-list.component.html'
})
export class QuizListComponent implements OnInit, OnDestroy  {
   /////////////////////////////////////////////////////////////
   /// Public Properties

   public id: string = null;

   public quizList: Quiz[] = null;

   /////////////////////////////////////////////////////////////
   /// Private Properties

   private _default: Default; 

   ////////////////////////////////////////////////////////////
   /// Constructor and ngOnInit / ngOnDestroy

   constructor(private _dataService: DataService) {}

   ngOnInit() {
      this._default = new Default();
      this._getQuizData();
   }

   ngOnDestroy() {
     this.quizList = null;
     this._default = null;
   }

   // sets public properties quizList, currentQuiz, and currentAnswers  
   // with quiz data from the quiz-o-matic api
   private _getQuizData(): void {
      this._dataService.getQuizes()
         .subscribe(quizes => {
             this.quizList = quizes;
         }); // end subscribe callback
   }
}
