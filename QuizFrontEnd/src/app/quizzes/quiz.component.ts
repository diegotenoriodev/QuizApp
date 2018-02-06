import { Component, Input, Output, EventEmitter } from '@angular/core';
import { BaseComponentOperation, Quiz } from '../model/core.component';
import { APIService } from '../services/api.service';

@Component({
    selector: 'app-quiz',
    templateUrl: 'quiz.component.html'
})
export class QuizComponent extends BaseComponentOperation {
    constructor(private apiService: APIService) {
        super();
        this.quiz = null;
        this.loading = false;
    }

    @Input() quiz: Quiz;

    getQuiz() {
        if (this.quiz == null) {
            return new Quiz();
        }

        return this.quiz;
    }

    getTitle() {
        if (this.quiz != null) {
            if (this.quiz.id === 0) {
                return 'New Quiz';
            } else {
                return 'Alter Quiz';
            }
        }
    }

    protected cleanObject() {
        this.quiz = null;
    }

    private validateFields() {
        this.cleanErrors();

        if (this.quiz.name == null) {
            this.addError('Please inform the name.');
        }

        if (this.quiz.description == null) {
            this.addError('Please inform the description.');
        }

        return this.hasErrors();
    }

    save() {
        if (this.validateFields()) {
            this.loading = true;
            console.log(this.quiz);
            this.apiService.postQuiz(this.quiz, this);
        }
    }

    cancel() {
        this.quiz = null;
        this.errors = [];
    }

    protected onResultReceived(item) {
    }
}
