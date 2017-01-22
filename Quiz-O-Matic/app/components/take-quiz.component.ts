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

   public currentAnswers: Answer[] = null;

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

      this._quizService.getQuizes()
         .subscribe(quizes => {
             that.quizList = quizes;
             if (that.id != null && that.quizList != null) {
                that.quizList.forEach(quiz => {
                    if (quiz.id === that.id) {
                        that.currentQuiz = quiz;
                        that._getQuestions();
                    }
                }); // end foreach
             }
             that.toggleDisplay();
         }); // end subscribe callback
   }

   private _getQuestions(quizId: string = null) : void {
       let that = this;
       if (quizId === null && this.id != null) {
           quizId = this.id;
       }

       if (quizId != null) {
          this._quizService.getQuestions(quizId)
             .subscribe(questions => {
               that.currentQuestions = questions;
               that._getAnswers();               
           });
       } // end if quizId != null
   }

   private _getAnswers(quizId: string = null) : void {
       let that = this;
       if (quizId === null && this.id != null) {
           quizId = this.id;
       }

       if (quizId != null) {
          this._quizService.getAnswers(quizId)
             .subscribe(answers => {
                 that.currentAnswers = answers;
                 if (that.currentQuiz != null && that.currentQuestions != null) {
                     that.currentQuestions.forEach((question, index) => {
                         for (var i = 0; i < that.currentAnswers.length; i++) {
                             if (question.id == that.currentAnswers[i].questionId) {
                                that.currentQuestions[index].answers.push(that.currentAnswers[i]);
                             }
                         } // end for each answer
                    }); // end for each question
                 }
                 
           });
       } // end if quizId != null
   }
}