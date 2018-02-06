import { Component } from '@angular/core';
import { BaseAnswer } from './baseanswer.component';
import { Question, TrueFalseOption, BaseQuestionOption, MultipleChoiceQuestionOption } from '../model/core.component';
import { APIService } from '../services/api.service';

@Component ({
  selector: 'app-multiple-choice',
  templateUrl: 'multiplechoice.component.html'
})
export class MultipleChoiceComponent implements BaseAnswer {
    question: Question;
    options: MultipleChoiceQuestionOption[];

    getQuestionOptions(): BaseQuestionOption[] {
        return this.options;
    }

    constructor(private apiService: APIService) {
        this.options = [
            new MultipleChoiceQuestionOption(),
            new MultipleChoiceQuestionOption()
        ];
    }

    load(question: Question) {
        if (question != null) {
            if (question.id === 0) {
                this.options = [
                    new MultipleChoiceQuestionOption(),
                    new MultipleChoiceQuestionOption()
                ];
            } else {
                this.options = [];
                this.apiService.getQuestionOptions(question.id).forEach(r => {
                    this.options = [];
                    r.forEach(item => {
                        const choice = new MultipleChoiceQuestionOption();
                        choice.content = item.content;
                        choice.id = item.id;
                        choice.isCorrect = item.isCorrect;
                        choice.order = item.order;
                        this.options.push(choice);
                    });
                });
            }
        } else {
            this.cleanObject();
        }
    }

    removeOption(i) {
        this.options.splice(i, 1);
    }

    addOption() {
        this.options.push(new MultipleChoiceQuestionOption());
    }

    protected cleanObject() {
        this.options = [];
        this.question = null;
    }
}
