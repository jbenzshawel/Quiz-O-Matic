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


@Injectable()
export class QuizService {

    private _default: Default;

    private _headers: Headers = new Headers({ 'Content-Type': 'application/json' });

    constructor(private http: Http) {
        this._default = new Default();
    }

    ///////////////////////////////////////////////////////////////
    /// GET Methods

    // ToDo: add pagination 
    // gets a list of quizes from the database
    public getQuizes():Observable<Quiz[]> {
        let quizList: Quiz[] = null;
        let apiEndpoint: string = "//localhost:5000/api/quizes/list";

        return this.http.get(apiEndpoint)
        .map((response: Response) => {
            let data = response.json();
            if (data != null && typeof (data) === "object" && data.length > 0) {
               quizList = data; 
            }

            return quizList;
        });
    }


    public getQuestions(quizId: string): Observable<Question[]> {
        let questionList: Question[] = [];
        let apiEndpoint: string = null;

        if (this._default.isGuid(quizId)) {
            apiEndpoint = "//localhost:5000/api/questions/list/" + quizId;

            return this.http.get(apiEndpoint)
                .map((response: Response) => {
                    let data = response.json();
                    if (data != null)
                        data.forEach((question: Question) => {
                            questionList.push(new Question(question.id, question.title, question.attributes, question.quizId))
                        })

                    return questionList;
                });
        }
        
        return null;
    }

    public getAnswers(quizId: string): Observable<Answer[]> {
        let questionAnswer: Answer[] =  [];
        let apiEndpoint: string = null;

        if (this._default.isGuid(quizId)) {
            apiEndpoint = "//localhost:5000/api/answers/list/" + quizId;

            return this.http.get(apiEndpoint)
                .map((response: Response) => {
                    let data: Answer[] = response.json();
                    
                    if (data != null) {
                        data.forEach(answer => {
                            questionAnswer.push(answer)
                        });
                    }

                    return questionAnswer;
                });
        }

        return null;
    }
}