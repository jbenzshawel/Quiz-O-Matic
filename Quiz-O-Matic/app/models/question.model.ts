import { Answer } from './answer.model';

export class Question {
    public id: number;

    public title: string;

    public attributes: string;

    public quizId: string;

    public answers: Answer[];

    constructor(id: number = null, title: string = null, attributes: string = null, quizId: string = null, answers: Answer[] = null) {
        if (id != null)
            this.id = id;
        
        if (title != null)
            this.title = title;

        if (quizId != null)
            this.quizId = quizId;

        if (attributes != null)
            this.attributes = attributes;
        
        this.answers = [];      

        if (answers != null)
            this.answers = answers;   
    }
}