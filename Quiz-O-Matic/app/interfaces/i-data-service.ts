// @angular
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
// models
import { Quiz } from './../models/quiz.model';
import { Question } from './../models/question.model';
import { Answer, QuestionAnswer } from './../models/answer.model';


export interface IDataService {

    getQuizes():Observable<Quiz[]>;

    getQuiz(quizId: string): Observable<Quiz>;

    getQuestions(quizId: string): Observable<Question[]>;

    getAnswers(quizId: string,  includeActive: boolean): Observable<Answer[]>;

}