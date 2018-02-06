import { Component } from '@angular/core';
import { IAnswerItem, AnswerOptionBase } from './baseansweritem.component';
import { Question, ListItem } from '../../../model/core.component';
import { APIService } from '../../../services/api.service';

@Component({
    selector: 'app-multichoice-answer',
    templateUrl: 'multiplechoiceanswer.component.html'
})
export class MultipleChoiceAnswerComponent implements IAnswerItem {
    apiService: APIService;
    private answers: MultipleChoiceAnswerOption;
    private question: Question;
    quizId: number;

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

    load(answerId: number, question: Question) {
        this.question = question;

        this.apiService.getAnswerForQuestion(question.idQuiz, question.id).subscribe(
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

                this.apiService.getListOptions(this.quizId, this.question.id).subscribe(
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
