import { Component } from '@angular/core';
import { IReportItem, AnswerOptionBase } from './baseansweritem.component';
import { Question } from '../../../model/core.component';
import { APIService } from '../../../services/api.service';

@Component({
    selector: 'app-true-false-answer',
    templateUrl: 'trueorfalseanswer.component.html'
})
export class TrueOrFalseReportComponent implements IReportItem {
    private trueOrFalse: TrueFalseAnswerOption;
    private question: Question;

    apiService: APIService;
    quizId: number;

    getQuestionId() {
        if (this.question == null) {
            return null;
        }

        return this.question.id;
    }

    get option() {
        if (this.trueOrFalse == null) {
            return null;
        }

        if (this.trueOrFalse.option) {
            return 1;
        }

        return 0;
    }

    set option(value) {
        if (value === 1) {
            this.trueOrFalse.option = true;
        } else {
            this.trueOrFalse.option = false;
        }
    }


    load(idAnswer: number, question: Question) {
        this.question = question;

        this.apiService.getAnswerReportForQuestion(idAnswer, question.id).subscribe(
            res => {
                if (res != null) {
                    this.trueOrFalse = res as TrueFalseAnswerOption;
                    this.trueOrFalse.id = idAnswer;
                } else {
                    this.trueOrFalse = new TrueFalseAnswerOption();
                    this.trueOrFalse.id = idAnswer;
                }

                this.trueOrFalse.question = this.question;
            }
        );
    }
}

export class TrueFalseAnswerOption extends AnswerOptionBase {
    option: boolean;
}
