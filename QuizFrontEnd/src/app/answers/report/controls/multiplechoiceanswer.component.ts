import { Component } from '@angular/core';
import { IReportItem, AnswerOptionBase } from './baseansweritem.component';
import { Question, ListItem } from '../../../model/core.component';
import { APIService } from '../../../services/api.service';

@Component({
    selector: 'app-multichoice-answer',
    templateUrl: 'multiplechoiceanswer.component.html'
})
export class MultipleChoiceReportComponent implements IReportItem {
    private answers: MultipleChoiceAnswerOption;
    private question: Question;

    idAnswer: number;
    apiService: APIService;
    quizId: number;
    options: CheckBoxItem[];

    load(answerId: number, question: Question) {
        this.question = question;

        this.apiService.getAnswerReportForQuestion(answerId, question.id).subscribe(
            res => {
                console.log(res);
                if (res != null) {
                    this.answers = res as MultipleChoiceAnswerOption;
                    this.answers.id = answerId;
                } else {
                    this.answers = new MultipleChoiceAnswerOption();
                    this.answers.id = answerId;
                    this.answers.idAnswers = [];
                }

                this.answers.question = this.question;

                this.apiService.getListOptionsAnswer(this.quizId, this.question.id).subscribe(
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

export class MultipleChoiceAnswerOption extends AnswerOptionBase {
    idAnswers: number[];
}

class CheckBoxItem extends ListItem {
    checked: boolean;
}
