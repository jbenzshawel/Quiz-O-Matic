// @angular
import { Component, OnInit, OnDestroy } from '@angular/core';
// models
import { Quiz }                         from './../models/quiz.model';
// services and helpers
import { DataService }                  from './../services/data.service';

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

   ////////////////////////////////////////////////////////////
   /// Constructor and ngOnInit / ngOnDestroy

   constructor(private _dataService: DataService) {}

   ngOnInit() {
      this._getQuizData();
   }

   ngOnDestroy() {
     this.quizList = null;
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
