import { Component, Input, ViewChild, ComponentFactoryResolver } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PublishedQuiz, Quiz, Question, ResultOperation } from '../../model/core.component';
import { AnswerDirective } from '../../answertype/baseanswer.component';
import { APIService } from '../../services/api.service';
import { IAnswerItem } from '../controls/baseanswerOption.component';
import { BaseAnswerComponent } from '../controls/baseanswer.component';

@Component({
    selector: 'app-quizanswer',
    templateUrl: 'answer.component.html'
})
export class AnswerComponent extends BaseAnswerComponent {

    @ViewChild(AnswerDirective) private answerHost: AnswerDirective;

    set currentAnswerItem(answerItem: IAnswerItem) {
        this._currentAnswerItem = answerItem;
    }

    protected getIsReadOnly(): boolean {
        return false;
    }

    protected getApiService(): APIService {
        return this.apiService;
    }

    protected getComponentFactoryResolver(): ComponentFactoryResolver {
        return this.componentFactoryResolver;
    }

    protected getAnswerDirective(): AnswerDirective {
        return this.answerHost;
    }

    constructor(private apiService: APIService,
        private componentFactoryResolver: ComponentFactoryResolver) { super(); }

    public saveCurrent() {
        this._currentAnswerItem.save(res => {
            console.log(res);
        });
    }

    public loadNewAnswer(idQuiz, idAnswer, number, question, type) {
        this._idAnswer = idAnswer;
        this._idQuiz = idQuiz;
        this._question = question;
        this._number = number;

        super.loadAnswerComponent(type);
    }
}
