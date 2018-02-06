import { Component, ViewChild, Input, ComponentFactoryResolver } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PublishedQuiz, Quiz, Question, ResultOperation } from '../../model/core.component';
import { IReportItem } from './controls/baseansweritem.component';
import { AnswerDirective } from '../../answertype/baseanswer.component';
import { APIService } from '../../services/api.service';
import { OpenEndedReportComponent } from './controls/openended.component';
import { MultipleChoiceReportComponent } from './controls/multiplechoiceanswer.component';
import { TrueOrFalseReportComponent } from './controls/trueorfalseanswer.component';

@Component({
    selector: 'app-answerreport',
    templateUrl: 'answer.component.html'
})
export class AnswerReportComponent {
    private currentAnswerItem: IReportItem;
    private _question: Question;

    @Input() idAnswer: number;
    @Input() index: number;
    @Input() idQuiz: number;

    @Input() set question(quest: Question) {
        console.log(quest);
        this._question = quest;
        this.loadNewAnswer();
    }

    @ViewChild(AnswerDirective) answerHost: AnswerDirective;

    getQuestionNumber() {
        return this.index + 1;
    }

    getQuestion() {
        return this._question;
    }

    private loadNewAnswer() {
        const viewContainerRef = this.answerHost.viewContainerRef;
        viewContainerRef.clear();
        this.currentAnswerItem = null;
        let type = null;

        switch (this.getQuestion().questionType) {
            case 1: // TrueFalse_Question
                type = TrueOrFalseReportComponent;
            break;
            case 2: // Multiple_Choice
                type = MultipleChoiceReportComponent;
            break;
            case 3: // Open_Ended
                type = OpenEndedReportComponent;
            break;
        }

        if (type != null) {
            const componentFactory = this.componentFactoryResolver.resolveComponentFactory(type);
            const componentRef = viewContainerRef.createComponent(componentFactory);
            this.currentAnswerItem = componentRef.instance as IReportItem;

            this.currentAnswerItem.quizId = this.idQuiz;
            this.currentAnswerItem.apiService = this.apiService;
            this.currentAnswerItem.load(this.idAnswer as number, this.getQuestion());
        }
    }

    constructor(private apiService: APIService, private componentFactoryResolver: ComponentFactoryResolver) { }
}
