export class Answer {
    public id: number;

    public content: string;

    public identifier: string;

    public attributes: string;

    public questionId: number;
}

export class QuestionAnswer {
    public questionId: number;

    public answers: Answer[];

    constructor(questionId: number = null, answers: Answer[] = null) {
        if (questionId != null) {
            this.questionId = questionId; 
        }

        if (answers != null) {
            this.answers = answers;
        }
    }
}