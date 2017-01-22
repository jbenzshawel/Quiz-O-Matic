import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs';
import { Default } from './../classes/default';
import { Quiz } from './../models/quiz.model';

import 'rxjs/add/operator/map'

@Injectable()
export class QuizService {

    private _default: Default;

    private _headers: Headers = new Headers({ 'Content-Type': 'application/json' });

    constructor(private http: Http) {
        this._default = new Default();
    }

    // gets a list of quizes from the database
    public list():Observable<Quiz[]> {
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
}