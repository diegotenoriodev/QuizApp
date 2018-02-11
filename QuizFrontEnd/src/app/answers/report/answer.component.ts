import { Component, ViewChild, Input, ComponentFactoryResolver } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PublishedQuiz, Quiz, Question, ResultOperation } from '../../model/core.component';
import { AnswerDirective } from '../../answertype/baseanswer.component';
import { APIService } from '../../services/api.service';
import { BaseAnswerComponent } from '../controls/baseanswer.component';

@Component({
    selector: 'app-answerreport',
    templateUrl: 'answer.component.html'
})
export class AnswerReportComponent extends BaseAnswerComponent {
    @ViewChild(AnswerDirective) private answerHost: AnswerDirective;

    @Input() set idQuiz(value: number) { this._idQuiz = value; }
    @Input() set idAnswer(value: number) { this._idAnswer = value; }
    @Input() set number(value: number) { this._number = value; }
    @Input() set question(value: Question) {
        this._question = value;
    }
    @Input() set componentType(type) {
        super.loadAnswerComponent(type);
    }

    protected getIsReadOnly(): boolean {
        return true;
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
}
