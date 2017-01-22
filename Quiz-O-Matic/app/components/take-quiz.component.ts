// @angular
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
// models
import { Quiz } from './../models/quiz.model';
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
         if (idParam.toLowerCase() === "list") {
             this.listQuizes = true;
             this.takeQuiz = false;
             this._getQuizList();
         } else if (this._default.isGuid(idParam)) {
             this.id = idParam;
             this.takeQuiz = true;
             this.listQuizes = false;
         }
      });
   }

   ngOnDestroy() {
      this._sub.unsubscribe();
   }

   private _getQuizList(): void{
      this._quizService.list()
         .subscribe(quizes => {
             this.quizList = quizes;
         });

   }
}