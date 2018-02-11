import { Component } from '@angular/core';
import { Question, ListItem, CheckBoxItem } from '../../model/core.component';
import { APIService } from '../../services/api.service';
import { IAnswerItem, BaseAnswerOption } from './baseanswerOption.component';

@Component({
    selector: 'app-multichoice-answer',
    templateUrl: 'multiplechoiceanswer.component.html'
})
export class MultipleChoiceAnswerComponent implements IAnswerItem {
    private answers: MultipleChoiceAnswerOption;
    private question: Question;

    readOnly: boolean;
    quizId: number;
    apiService: APIService;
    options: CheckBoxItem[];

    save(callBackFunction) {
        console.log(this.answers);
        this.answers.idAnswers = [];

        this.options.forEach(r => {
            if (r.checked) {
                this.answers.idAnswers.push(r.id);
            }
        });

        this.apiService.postAnswerForQuestion('multiplechoice', this.answers).subscribe( ret => {
            callBackFunction(ret);
        });
    }

    load(idAnswer: number, question: Question) {
        this.question = question;

        this.apiService.getAnswerForQuestion(idAnswer, question.id).subscribe(
            res => {
                console.log(res);
                if (res != null) {
                    this.answers = res as MultipleChoiceAnswerOption;
                    this.answers.id = idAnswer;
                } else {
                    this.answers = new MultipleChoiceAnswerOption();
                    this.answers.id = idAnswer;
                    this.answers.idAnswers = [];
                }

                this.answers.question = this.question;

                this.apiService.getListOptions(idAnswer, this.question.id).subscribe(
                    options => {
                        console.log(options);
                        this.options = [];
                        (options as ListItem[]).forEach(r => {
                            const item = new CheckBoxItem();
                            item.id = r.id;
                            item.name = r.name;

                            this.answers.idAnswers.forEach(answer => {
                                if (answer === item.id) {
                                    item.checked = true;
                                }
                            });

                            this.options.push(item);
                        });
                    }
                );
            }
        );
    }
}

export class MultipleChoiceAnswerOption extends BaseAnswerOption {
    idAnswers: number[];
}
