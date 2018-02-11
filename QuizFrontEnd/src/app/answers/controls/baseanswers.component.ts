import { PublishedQuiz, Question, Quiz } from '../../model/core.component';
import { TrueOrFalseAnswerComponent } from './trueorfalseanswer.component';
import { MultipleChoiceAnswerComponent } from './multiplechoiceanswer.component';
import { OpenEndedAnswerComponent } from './openended.component';

export abstract class BaseAnswersComponent {
    protected publishedQuiz: PublishedQuiz;
    protected idAnswer: number;
    protected errors: string[];

    getErrors() {
        return this.errors;
    }

    getIdAnswer() {
        return this.idAnswer;
    }

    getQuiz() {
        if (this.publishedQuiz != null) {
            return this.publishedQuiz.quiz;
        }
        return new Quiz();
    }

    getQuestions() {
        if (this.publishedQuiz != null) {
            return this.publishedQuiz.questions;
        }
        return [];
    }

    getComponentType(question: Question) {
        let type = null;

        switch (question.questionType) {
            case 1: // TrueFalse_Question
                type = TrueOrFalseAnswerComponent;
            break;
            case 2: // Multiple_Choice
                type = MultipleChoiceAnswerComponent;
            break;
            case 3: // Open_Ended
                type = OpenEndedAnswerComponent;
            break;
        }

        return type;
    }

    constructor() {
        this.errors = [];
    }
}
