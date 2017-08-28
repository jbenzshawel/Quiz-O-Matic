// @angular
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router }               from '@angular/router';
// models
import { Quiz }                         from './../models/quiz.model';
import { Question }                     from './../models/question.model';
import { Answer, QuestionAnswer }       from './../models/answer.model';
import { QuizResult }                   from './../models/quiz.result.model';

// services and helpers
import { DataService }                  from './../services/data.service';
import { Common }                      from './../classes/common';
import { QuizEngine }                   from './../classes/quiz.engine';

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

   public currentQuiz: Quiz = null;

   public currentQuestions: Question[] = null;

   public currentAnswers: Answer[] = null;

   public quizResult: QuizResult = null;

   public quizResultHeading: string = null;

   public isValidForm: boolean = false;

   public takeQuiz: boolean = false;

   public showResult: boolean = false;

   public hideForm: boolean = false;

   public activeOption: any = {};

   public totalNumberQuestions: number;

   public currentQuestionNumber: number;

   public resultImageSource: string = "//placehold.it/450X250";

   /////////////////////////////////////////////////////////////
   /// Private Properties

   private _sub: any;

   private _common: Common; 

   ////////////////////////////////////////////////////////////
   /// Constructor and ngOnInit / ngOnDestroy

   constructor(private _dataService: DataService, private _activatedRoute: ActivatedRoute, private _router: Router) {}

   ngOnInit() {
      this.model = {
        quizId: this.id,
        response: {}
      };
      this._common = new Common();
      this._handleRoute();
      this.activeOption = {};
      this.currentQuestionNumber = 1;
      $(".activeOption").removeClass("activeOption");
   }

   ngOnDestroy() {
      this._sub.unsubscribe();
      this.currentQuestions = null;
      this.currentAnswers = null;
      this.currentQuiz = null;
      this.activeOption = {};
      this.model.responses = {};
      $(".activeOption").removeClass("activeOption");
   }

   /////////////////////////////////////////////////////////////
   /// Public Methods 

   // toggles take-quiz view depending on id property / url param
   public toggleDisplay(): void {
       this.showResult = false;
       this._common.clearHash();
       this.model.response = {};
       this.activeOption = {}; 
       $(".activeOption").removeClass("activeOption"); 
   }

   public onResponse(event: Event): void {
       this._validateSelectOptions();
   }

   public submitQuiz(event: Event): void {
      event.preventDefault();
      if (this._validateSelectOptions()) {
         // copy values of responses into a new obj (to pass to quiz engine)
         let responseCopy = JSON.stringify(this.model.response);
         this._dataService.getAnswers(this.id, true)
            .subscribe(answers => {
               let parsedResponse: any = this._common.parseJsonObject(responseCopy);
               let quizEngine = new QuizEngine(this.currentQuiz, answers, this.currentQuestions, parsedResponse);
              
               this.quizResult = new QuizResult();
               this.quizResult.content = quizEngine.scoreTwoOption();
               //this.quizResult.imagePath = quizEngine.status ? this.currentQuiz.images.pass : this.currentQuiz.images.fail;
               this.quizResult.imagePath = this.resultImageSource;
               this.quizResult.title = "Results: ".concat(this.currentQuiz.name);

               window.sessionStorage.setItem(this.quizResult.storageKey.concat(this.id), JSON.stringify(this.quizResult));
               this._router.navigateByUrl("/quiz-result/".concat(this.id));
         });
       } // end if valid select options 
   }

   public toggleResetForm(event: Event): void {
       event.preventDefault();
       this.toggleDisplay();
   }

   public showNextQuestion():void {
       let numberResponses:number = Object.keys(this.model.response).length;
       if (numberResponses === this.currentQuestionNumber) {
          this.currentQuestionNumber += 1;
           window.scrollTo(0, 0);
       } else {
           $("#no-response-modal").modal("show");
       }
   }

   /////////////////////////////////////////////////////////////
   /// Private Methods 
   
   // handle route parameters (either show quiz details or list quizes depending)
   // on what is passed for :id in route take-quiz/:id
   private _handleRoute(): void {  
     this._common.clearHash();
      this._sub = this._activatedRoute.params.subscribe(params => {
         let idParam: string = params["id"];
         this.id = idParam;
         if (this._common.isGuid(idParam)) {
            this.model.id = this.id;
            this._getQuiz(this.id);
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

   private _getQuiz(quizId:string = null): void {
        if (quizId === null && this.id != null) {
           quizId = this.id;
        }

       this._dataService.getQuiz(this.id)
          .subscribe(quiz => {
              this.currentQuiz = quiz;
              if (this.currentQuiz != null)
                this._getQuestions();

             this.toggleDisplay();
          })
   }

   // sets currentQuestions property
   private _getQuestions(quizId: string = null) : void {
       if (quizId === null && this.id != null) {
           quizId = this.id;
       }

       if (quizId != null) {
          this._dataService.getQuestions(quizId)
             .subscribe(questions => {
               this.currentQuestions = questions;
               this.totalNumberQuestions = questions.length;
               if (this.currentQuestions != null) {
                  this._getAnswers();
               }
           });
       } // end if quizId != null
   }

   // sets currentAnswers
   // note: dependent on currentQuiz and currentQuestions already 
   // being set with data from api.
   private _getAnswers(quizId: string = null) : void {
       if (quizId === null && this.id != null) {
           quizId = this.id;
       }

       if (quizId != null) {
          this._dataService.getAnswers(quizId)
             .subscribe(answers => {
                 this.currentAnswers = answers;
                 if (this.currentQuiz != null && this.currentQuestions != null) {
                     // loop over each question and set its answers property
                     this.currentQuestions.forEach((question, index) => {
                         for (var i = 0; i < this.currentAnswers.length; i++) {
                             if (question.id == this.currentAnswers[i].questionId) {
                                this.currentQuestions[index].answers.push(this.currentAnswers[i]);
                             }
                         } // end for each answer
                    }); // end for each question
                 }
           });
       } // end if quizId != null
   }
}
