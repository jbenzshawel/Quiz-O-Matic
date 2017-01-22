export class Answer {
    public id: number;

    public content: string;

    public identifier: string;

    public attributes: Object;

    public quiestionId: number;
}

export class QuestionAnswer {
    public questionId: number;

    public answers: Answer[];
}