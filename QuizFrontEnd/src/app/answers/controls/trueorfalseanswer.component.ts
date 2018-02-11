import { Component } from '@angular/core';
import { IAnswerItem, BaseAnswerOption } from './baseanswerOption.component';
import { Question } from '../../model/core.component';
import { APIService } from '../../services/api.service';

@Component({
    selector: 'app-true-false-answer',
    templateUrl: 'trueorfalseanswer.component.html'
})
export class TrueOrFalseAnswerComponent implements IAnswerItem {
    private trueOrFalse: TrueFalseAnswerOption;
    private question: Question;

    apiService: APIService;
    quizId: number;
    readOnly: boolean;

    getQuestionId() {
        if (this.question != null) {
            return this.question.id;
        }

        return 0;
    }

    get option() {
        if (this.trueOrFalse == null) {
            return null;
        }

        if (this.trueOrFalse.option == null) {
            return null;
        }

        if (this.trueOrFalse.option) {
            return 1;
        }

        return 0;
    }

    set option(option) {
        if (Number(option) === 0) {
            this.trueOrFalse.option = false;
        } else {
            this.trueOrFalse.option = true;
        }
    }

    save(callBackFunction) {
        this.apiService.postAnswerForQuestion('truefalse', this.trueOrFalse).subscribe( ret => {
            callBackFunction(ret);
        });
    }

    load(idAnswer: number, question: Question) {
        this.question = question;

        this.apiService.getAnswerForQuestion(idAnswer, question.id).subscribe(
            res => {
                if (res != null) {
                    this.trueOrFalse = res as TrueFalseAnswerOption;
                    this.trueOrFalse.id = idAnswer;
                } else {
                    this.trueOrFalse = new TrueFalseAnswerOption();
                    this.trueOrFalse.option = null;
                    this.trueOrFalse.id = idAnswer;
                }

                this.trueOrFalse.question = this.question;
            }
        );
    }
}

export class TrueFalseAnswerOption extends BaseAnswerOption {
    option: boolean;
}
