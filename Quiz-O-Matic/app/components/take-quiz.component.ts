// @angular
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
// models
import { Quiz } from './../models/quiz.model';
import { Question } from './../models/question.model';
import { Answer, QuestionAnswer } from './../models/answer.model'
// services and helpers
import { DataService } from './../services/data.service';
import { Default } from './../classes/default';
import { QuizEngine } from './../classes/quiz.engine';

// declare globals for ts compiler 
declare var $: any;
declare var window: any;

@Component({
  moduleId: module.id,
   providers: [
    DataService
  ],
  templateUrl: 'take-quiz.component.html'
})
export class TakeQuizComponent implements OnInit, OnDestroy  {
   /////////////////////////////////////////////////////////////
   /// Public Properties
   
   public model: any;

   public validForm: boolean = false;

   public id: string = null;
   // default to list view
   public listQuizes: boolean = true;

   public quizList: Quiz[] = null;

   public currentQuiz: Quiz = null;

   public currentQuestions: Question[] = null;

   public currentAnswers: Answer[] = null;

   public quizResult: string = null;

   public isValidForm: boolean = false;

   public takeQuiz: boolean = false;

   public showResult: boolean = false;

   public hideForm: boolean = false;

   public activeOption: any = {};

   /////////////////////////////////////////////////////////////
   /// Private Properties
   
   private _sub: any;

   private _default: Default; 

   constructor(private _dataService: DataService, private _activatedRoute: ActivatedRoute) {}

   ngOnInit() {
      this.model = {
        quizId: this.id,
        response: {}
      };
      this._default = new Default();
      this._handleRoute();
      this.activeOption = {};
      $(".activeOption").removeClass("activeOption");      
   }

   ngOnDestroy() {
      this._sub.unsubscribe();
      this.currentQuestions = null;
      this.currentAnswers = null;
      this.currentQuiz = null;
      this.activeOption = {};      
      $(".activeOption").removeClass("activeOption");         
   }

   /////////////////////////////////////////////////////////////
   /// Public Methods 

   // toggles take-quiz view depending on id property / url param
   public toggleDisplay(): void {
       if (this.id === "list") {
           this.listQuizes = true;
           this.takeQuiz = false;
           this.showResult = false;
       } else {
           this.listQuizes = false;
           this.takeQuiz = true;
           this.showResult = false;         
           this.hideForm = false;
       }
       this._default.clearHash();
       this.model.response = {};
       this.activeOption = {};       
       $(".activeOption").removeClass("activeOption");                     
   }

   public onResponse(event: Event): void {
       this._validateSelectOptions();       
   }

   public submitQuiz(event: Event): void {
      event.preventDefault();
      window.location.hash = "score";
      if (this._validateSelectOptions()) {
         // copy values of responses into a new obj (to pass to quiz engine)
         let responseCopy = JSON.stringify(this.model.response);
         this._dataService.getAnswers(this.id, true)
            .subscribe(answers => {
               let parsedResponse: any = JSON.parse(responseCopy);
               let quizEngine = new QuizEngine(this.currentQuiz, answers, this.currentQuestions, parsedResponse);
               this.quizResult = quizEngine.scoreTwoOption();
               this.showResult = true;
               this.hideForm= true;
         });
       } // end if valid select options 
   }

   public toggleResetForm(event: Event): void {
       event.preventDefault();
       this.toggleDisplay();
   }

   /////////////////////////////////////////////////////////////
   /// Private Methods 
   
   // handle route parameters (either show quiz details or list quizes depending)
   // on what is passed for :id in route take-quiz/:id
   private _handleRoute(): void {  
     this._default.clearHash();
      this._sub = this._activatedRoute.params.subscribe(params => {
         let idParam: string = params["id"];
         this.id = idParam;         
         if (this._default.isGuid(idParam)) {
            this.model.id = this.id;
            this._getQuiz(this.id);
         } else if (idParam.toLowerCase() === "list") {
            this._getQuizData();    
            this._clearCurrentQuiz();                          
         }
      }); // end subscribe to activatedRoute params
   }

   private _clearCurrentQuiz(): void {
       this.currentQuiz = null;
       this.currentAnswers = null;
       this.currentQuestions = null;
   }

   // enables submit button and returns true if form is valid 
   // else returns false and disables submit button
   private _validateSelectOptions(): boolean {
       let propertyLength: number = Object.keys(this.model.response).length;
       if (!isNaN(propertyLength) && propertyLength === this.currentQuestions.length) {
           this.isValidForm = true;
       } else {
           this.isValidForm = false;
       }
       return this.isValidForm;
   }

   // sets public properties quizList, currentQuiz, and currentAnswers  
   // with quiz data from the quiz-o-matic api
   private _getQuizData(): void {
      let that = this;

      this._dataService.getQuizes()
         .subscribe(quizes => {
             that.quizList = quizes;
             that.toggleDisplay();
         }); // end subscribe callback
   }

   private _getQuiz(quizId:string = null): void {
       let that = this;

        if (quizId === null && this.id != null) {
           quizId = this.id;
       }

       this._dataService.getQuiz(this.id)
          .subscribe(quiz => {
              that.currentQuiz = quiz;
              if (that.currentQuiz != null)
                that._getQuestions();
            
             that.toggleDisplay();            
          })
   }

   // sets currentQuestions property
   private _getQuestions(quizId: string = null) : void {
       let that = this;

       if (quizId === null && this.id != null) {
           quizId = this.id;
       }

       if (quizId != null) {
          this._dataService.getQuestions(quizId)
             .subscribe(questions => {
               that.currentQuestions = questions;
               if (that.currentQuestions != null)
                  that._getAnswers();               
           });
       } // end if quizId != null
   }

   // sets currentAnswers
   // note: dependent on currentQuiz and currentQuestions already 
   // being set with data from api.
   private _getAnswers(quizId: string = null) : void {
       let that = this;
       if (quizId === null && this.id != null) {
           quizId = this.id;
       }

       if (quizId != null) {
          this._dataService.getAnswers(quizId)
             .subscribe(answers => {
                 that.currentAnswers = answers;
                 if (that.currentQuiz != null && that.currentQuestions != null) {
                     // loop over each question and set its answers property
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