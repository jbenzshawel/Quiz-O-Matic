export class QuizResult {
    public title:string;
    public content: string;
    public imagePath:string;
    public storageKey: string;

    constructor() {
        this.storageKey = "quiz-result-";
    }
}