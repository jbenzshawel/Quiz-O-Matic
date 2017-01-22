// @angular
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
// models
import { Quiz } from './../models/quiz.model';
import { Question } from './../models/question.model';
import { Answer, QuestionAnswer } from './../models/answer.model'
// services and helpers
import { QuizService } from './../services/quiz.service';
import { Default } from './../classes/default';

@Component({
  moduleId: module.id,
   providers: [
    QuizService
  ],
  templateUrl: 'take-quiz.component.html'
})
export class TakeQuizComponent implements OnInit, OnDestroy  {
   
   public id: string = null;

   // default to list view
   public listQuizes: boolean = true;

   public quizList: Quiz[] = null;

   public currentQuiz: Quiz = null;

   public currentQuestions: Question[] = null

   public currentQuestionAnswers: QuestionAnswer = null;

   public takeQuiz: boolean = false;

   private _sub: any;

   private _default: Default; 

   constructor(private _quizService: QuizService, private _activatedRoute: ActivatedRoute) {}

   ngOnInit() {
      this._default = new Default();
      // handle route parameters (either show quiz details or list quizes depending)
      // on what is passed for :id in route take-quiz/:id
      this._sub = this._activatedRoute.params.subscribe(params => {
         let idParam: string = params["id"];

         if (idParam.toLowerCase() === "list" || this._default.isGuid(idParam)) {
             this.id = idParam;
         } 
         
         this._getQuizList();                 
      });
   }

   ngOnDestroy() {
      this._sub.unsubscribe();
   }

   public toggleDisplay(): void {
       if (this.id === "list") {
           this.listQuizes = true;
           this.takeQuiz = false;
       } else {
           this.listQuizes = false;
           this.takeQuiz = true;
       }
   }

   // sets public property quizList with data from api
   private _getQuizList(): void {
      let that = this;
      this._quizService.list()
         .subscribe(quizes => {
             that.quizList = quizes;
             if (that.id != null && that.quizList != null) {
                that.quizList.forEach(quiz => {
                    if (quiz.id === that.id) {
                        this.currentQuiz = quiz;
                    }
                }); // end foreach
             }
             this.toggleDisplay();
         }); // end subscribe callback
   }

   private _getQuestions() : void {


   }

   private _getQuizOptions(): void {

   }
}