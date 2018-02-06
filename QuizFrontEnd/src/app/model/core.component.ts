import { Output, EventEmitter } from '@angular/core';

export class ListItem {
    id: number;
    name: string;
}

export class Quiz {
    id: number;
    name: string;
    description: string;
    qtdQuestions: number;
    qtdAnswers: number;
}

export class BaseQuestionOption {
    id: number;
}

export class MultipleChoiceQuestionOption extends BaseQuestionOption {
    $type = 'QuizAPI.Domain.MultipleChoice, QuizAPI';
    isCorrect: boolean;
    order: number;
    content: string;

    constructor() { super(); }
}

export class TrueFalseOption extends BaseQuestionOption {
    $type = 'QuizAPI.Domain.TrueFalseAnswer, QuizAPI';
    answer: boolean;
}

export class Question {
    id: number;
    description: string;
    imageUrl: string;
    questionType: number; // YesNo_Question, TrueFalse_Question, Multiple_Choice, Open_Ended, Image_Chooser
    idQuiz: number;
    options: BaseQuestionOption[];
}

export class User {
    id: number;
    username: string;
    email: string;
    currentPassword: string;
    password: string;
    confirmPassword: string;
}

export class LoginCredentials {
    username: string;
    password: string;
}

export class QuizPublicationInfo {
    quiz: Quiz;
    userIds: string[];
    access: number; // 1 for public, 2 for specific users
    expirationDate: Date;
    url: string;
}

export class UserQuizPublish {
    idUser: string;
    userName: string;
    idQuiz: number;
    isMarked: boolean;
    hasAnswered: boolean;
}

export class QuizAnswer {
    id: number;
    quiz: Quiz;
    status: number; // 1 - New, 2 - In Progress, 3 - Complete
}

export class UserAnswerForQuiz extends ListItem {
    answeredAt: Date;
    isOpen: boolean;
    evaluated: boolean;
}

export class PublishedQuiz {
    questions: Question[];
    quiz: Quiz;
}

/////////////// Operations

export class ResultOperation {
    success: boolean;
    errors: string[];
    object: Object;
}

export interface OperationHandler {

    handlerResult(result: ResultOperation);
    handleError(error);

}

export abstract class BaseComponentOperation implements OperationHandler {

    protected successMessage: string;

    protected errors: String[];
    protected continueAfterOperation: boolean;
    protected loading: boolean;

    @Output() actionExecuted = new EventEmitter<Object>();

    getSuccessMessage() {
        return this.successMessage;
    }

    getErrors() {
        return this.errors;
    }

    getLoading() {
        return this.loading;
    }

    constructor() {
        this.errors = [];
        this.loading = false;
        this.continueAfterOperation = true;
    }

    protected addError(error: string) {
        this.errors.push(error);
    }

    protected cleanErrors() {
        this.errors = [];
    }

    protected hasErrors() {
        return this.errors.length === 0;
    }

    protected abstract cleanObject();

    protected afterOperation(item) {
        this.successMessage = 'Operation was successful.';
        this.actionExecuted.emit(item);
        this.cleanObject();
        this.loading = false;
    }

    handlerResult(result: ResultOperation) {
        this.successMessage = '';
        this.cleanErrors();

        if (result.success) {
            this.onResultReceived(result.object);

            if (this.continueAfterOperation) {
                this.afterOperation(result.object);
            }
        } else {
            this.errors = result.errors;
            this.loading = false;
        }
    }

    protected abstract onResultReceived(item);

    handleError(error: any) {
        this.loading = false;
    }
}
