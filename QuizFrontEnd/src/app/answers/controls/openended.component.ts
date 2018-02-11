import { Component } from '@angular/core';
import { IAnswerItem, BaseAnswerOption } from './baseanswerOption.component';
import { Question } from '../../model/core.component';
import { APIService } from '../../services/api.service';

@Component({
    selector: 'app-open-ended-answer',
    templateUrl: 'openendedanswer.component.html'
})
export class OpenEndedAnswerComponent implements IAnswerItem {
    private openEnded: OpenEndedAnswerOption;
    private question: Question;

    apiService: APIService;
    quizId: number;
    readOnly: boolean;

    getAnswer() {
        if (this.openEnded == null) {
            return new OpenEndedAnswerOption();
        }

        return this.openEnded;
    }

    save(callBackFunction) {
        console.log(this.openEnded);
        this.apiService.postAnswerForQuestion('openended', this.openEnded).subscribe( ret => {
            callBackFunction(ret);
        });
    }

    load(idAnswer: number, question: Question) {
        this.question = question;

        this.apiService.getAnswerForQuestion(idAnswer, question.id).subscribe(
            res => {
                if (res != null) {
                    this.openEnded = res as OpenEndedAnswerOption;
                    this.openEnded.id = idAnswer;
                } else {
                    this.openEnded = new OpenEndedAnswerOption();
                    this.openEnded.id = idAnswer;
                }

                this.openEnded.question = this.question;
            }
        );
    }
}

export class OpenEndedAnswerOption extends BaseAnswerOption {
    content: string;
}
