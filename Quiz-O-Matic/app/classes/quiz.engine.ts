import { Quiz }       from './../models/quiz.model';
import { Answer }     from './../models/answer.model';
import { Question }   from './../models/question.model';

export class QuizEngine {
    public quiz: Quiz; 

    public answers: Answer[];

    public questions: Question[];

    public responses: any;

    public status: boolean;

    constructor(quiz: Quiz, answers: Answer[], questions: Question[], responses: any) {
        this.quiz = quiz;
        this.answers = answers;
        this.questions = questions;
        this.responses = responses;
    }

    // ToDo: update scoreTwoOption to call api to get answer option attributes
    // and do not return answer attributes until quiz is submitted (to prevent people
    // from trying to fiddle / "hack" the quiz)
    public scoreTwoOption(): string {
        let result: string;
        let that = this;                 
        if (this.responses == null || this.responses.length == 0 || this.quiz == null 
                || this.answers == null || this.questions == null) {
            return null;
        }
        // used to keep track of each option select count
        let passCount: number = 0;
        let failCount: number = 0;
        if (this.quiz != null && this.quiz.attributes != null) {
           let answerAttribute = this.quiz.attributes.attributeName;
           let responseKeys = Object.keys(this.responses);
           // loop over responses
           for (let i = 0; i < responseKeys.length; i++) {
              let questionNumber = responseKeys[i];
              let questionAns = this.responses[questionNumber];
              let ansKeys = Object.keys(this.answers);
              
              // determine which option response falls into     
              for (var j = 0; j < ansKeys.length; j++) {
                 if (this.answers[j].questionId.toString() === questionNumber &&
                        this.answers[j].identifier === questionAns) {
                    if (this.answers[j].attributes.indexOf("true") > -1) {
                        passCount++;    
                    } else {
                        failCount++;
                    }
                 } // end if answer matches question 
              } // end for each answer   
          } // end for each response

          // get pass or fail message for result
          if (passCount + failCount === responseKeys.length) {
              if (passCount > failCount) {
                  result = this.quiz.attributes.pass;
                  this.status = true;
              } else {
                  result = this.quiz.attributes.fail;
                  this.status = false;
              }
          }
        } // end if this.quiz != null
        
        return result;
    }
}
