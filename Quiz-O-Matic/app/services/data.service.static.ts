// @angular
import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
// models
import { Quiz } from './../models/quiz.model';
import { Question } from './../models/question.model';
import { Answer, QuestionAnswer } from './../models/answer.model';
import { Default } from './../classes/default';
// data service interface 
import { IDataService } from './../interfaces/i-data-service';

// import static files 
var quiz1: Quiz                = require('/static/static-quiz1.json');
var quiz1Questions: Question[] = require('/static/static-quiz1-questions.json');
var quiz1Answers: Answer[]     = require('/static/static-quiz1-answers.json');
var quiz1Score: Answer[]       = require('/static/static-quiz1-score.json');

@Injectable()
export class DataServiceStatic implements IDataService {

    private _default: Default;

    private _headers: Headers = new Headers({ 'Content-Type': 'application/json' });

    private _baseUrl: string = "//localhost:5000/api";

    constructor(private http: Http) {
        this._default = new Default();
    }

    ///////////////////////////////////////////////////////////////
    /// GET Methods

    // ToDo: add pagination 
    // gets a list of quizes from the database
    public getQuizes():Observable<Quiz[]> {
        let quizList: Quiz[] = [ quiz1 ];
        
        return Observable.of(quizList).map(q => q);
    }

    public getQuiz(quizId: string): Observable<Quiz> {
        let quiz: Quiz = quiz1;
        
        return Observable.of(quiz).map(quiz => quiz);
    }

    public getQuestions(quizId: string): Observable<Question[]> {
        
        return Observable.of(quiz1Questions).map(questions => questions);
    }

    public getAnswers(quizId: string,  includeActive: boolean = false): Observable<Answer[]> {
        if (includeActive) {
            return Observable.of(quiz1Score).map(score => score);
        }
        return Observable.of(quiz1Answers).map(ans => ans);
    }
}