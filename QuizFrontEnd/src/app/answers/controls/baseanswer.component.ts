import { ViewChild, ComponentFactoryResolver } from '@angular/core';
import { IAnswerItem } from './baseanswerOption.component';
import { Question } from '../../model/core.component';
import { AnswerDirective } from '../../answertype/baseanswer.component';
import { APIService } from '../../services/api.service';

export abstract class BaseAnswerComponent {
    protected _currentAnswerItem: IAnswerItem;

    protected _idQuiz: number;
    protected _idAnswer: number;
    protected _number: number;
    protected _question: Question;

    protected abstract getIsReadOnly(): boolean;
    protected abstract getApiService(): APIService;
    protected abstract getComponentFactoryResolver(): ComponentFactoryResolver;
    protected abstract getAnswerDirective(): AnswerDirective;

    getNumber() {
        return this._number + 1;
    }

    getQuestion(): Question {
        if (this._question != null) {
            return this._question;
        } else {
            return new Question(); // Giving a placeholder to the view
        }
    }

    protected loadAnswerComponent(type) {
        const viewContainerRef = this.getAnswerDirective().viewContainerRef;
        viewContainerRef.clear();
        this._currentAnswerItem = null;

        if (type != null) {
            const componentFactory = this.getComponentFactoryResolver().resolveComponentFactory(type);
            const componentRef = viewContainerRef.createComponent(componentFactory);
            this._currentAnswerItem = componentRef.instance as IAnswerItem;

            this._currentAnswerItem.quizId = this._idQuiz;
            this._currentAnswerItem.apiService = this.getApiService();
            this._currentAnswerItem.readOnly = this.getIsReadOnly();
            this._currentAnswerItem.load(this._idAnswer as number, this._question);
        }
    }
}
