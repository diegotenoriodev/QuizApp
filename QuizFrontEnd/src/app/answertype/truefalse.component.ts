import { Component } from '@angular/core';
import { BaseAnswer } from './baseanswer.component';
import { Question, TrueFalseOption, BaseQuestionOption } from '../model/core.component';
import { APIService } from '../services/api.service';

@Component({
    selector: 'app-truefalse',
    templateUrl: 'truefalse.component.html'
})
export class TrueFalseOptionComponent implements BaseAnswer {
    question: Question;
    option: TrueFalseOption;

    getQuestionOptions(): BaseQuestionOption[] {
        return [ this.option ];
    }


    constructor(private apiService: APIService) {
        this.option = new TrueFalseOption();
    }

    load(question: Question) {
        if (question != null) {
            if (question.id === 0) {
                this.option = new TrueFalseOption();
            } else {
                this.apiService.getQuestionOptions(question.id).forEach(r => {
                    r.forEach(item => {
                        this.option = new TrueFalseOption();
                        this.option.answer = item.answer;
                        this.option.id = item.id;
                    });
                });
            }
        } else {
            this.cleanObject();
        }
    }

    protected cleanObject() {
        this.option = new TrueFalseOption();
        this.question = null;
    }
}

