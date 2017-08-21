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
export class DataService {

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
        let quizList: Quiz[] = [];
        let apiEndpoint: string = this._baseUrl.concat("/quizes/list");

        return this.http.get(apiEndpoint)
        .map((response: Response) => {
            let data = response.json();
            if (data != null && typeof (data) === "object" && data.length > 0) {
                data.forEach((quiz: any) => {
                    quizList.push(new Quiz (quiz.id, quiz.name, quiz.description, this._getAttribute(quiz.attributes),
                        quiz.type, quiz.typeId, quiz.Date, quiz.Updated));
                });
            }

            return quizList;
        });
    }

    public getQuiz(quizId: string): Observable<Quiz> {
        let quiz: Quiz = null;
        let apiEndpoint: string = null
        let options: RequestOptions = new RequestOptions({ headers : this._headers });

        if (this._default.isGuid(quizId)) {
            apiEndpoint = this._baseUrl.concat("/quizes/").concat(quizId);

            return this.http.get(apiEndpoint, options)
                .map((response: Response) => {
                    let data = response.json();
                    if (data != null) {
                        quiz = new Quiz (data.id, data.name, data.description, this._getAttribute(data.attributes),
                            data.type, data.typeId, data.Date, data.Updated);
                    }

                    return quiz;
                });
        }
    }

    public getQuestions(quizId: string): Observable<Question[]> {
        let questionList: Question[] = [];
        let apiEndpoint: string = null;
        let options: RequestOptions = new RequestOptions({ headers : this._headers });

        if (this._default.isGuid(quizId)) {
            apiEndpoint = this._baseUrl.concat("/questions/list/").concat(quizId);

            return this.http.get(apiEndpoint, options)
                .map((response: Response) => {
                    let data = response.json();
                    if (data != null) {
                        data.forEach((question: Question) => {
                            questionList.push(new Question(question.id, question.title, question.attributes, question.quizId))
                        });
                    }

                    return questionList;
                });
        }

        return null;
    }

    public getAnswers(quizId: string,  includeActive: boolean = false): Observable<Answer[]> {
        let questionAnswer: Answer[] =  [];
        let apiEndpoint: string = null;

        if (this._default.isGuid(quizId)) {
            apiEndpoint = this._baseUrl.concat("/answers/list/").concat(quizId);

            // to obfruscate only add attribute flag if we need it 
            if (includeActive) {
                apiEndpoint = apiEndpoint.concat("/true");    
            }

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

    private _getAttribute(attString:string): Object {
        let attObj: Object = null
        if (attString == null || attString == undefined) {
            return attObj;
        }
        try {
            attObj = attString.length > 0 ? JSON.parse(attString) : null;
        } catch (e) {
            console.log(e);
        }
        return attObj;
    }
}