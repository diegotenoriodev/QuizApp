import { Component, Input, ViewChild, ComponentFactoryResolver, ViewContainerRef, AfterViewInit } from '@angular/core';
import { BaseComponentOperation, ListItem, Question } from '../model/core.component';
import { APIService } from '../services/api.service';
import { AnswerDirective, BaseAnswer } from '../answertype/baseanswer.component';
import { TrueFalseOptionComponent } from '../answertype/truefalse.component';
import { MultipleChoiceComponent } from '../answertype/multiplechoice.component';

@Component({
    selector: 'app-question',
    templateUrl: './question.component.html'
})
export class QuestionComponent extends BaseComponentOperation implements AfterViewInit {
    private typesOfQuestion: ListItem[];

    constructor(private apiService: APIService,
                private componentFactoryResolver: ComponentFactoryResolver) {
        super();
        this._question = null;
        this.loading = false;
    }

    private _question: Question;

    get question() {
        return this._question;
    }

    @Input()
    set question(question: Question) {
        this._question = question;
        this.loadComponent();
    }

    @ViewChild(AnswerDirective) answerHost: AnswerDirective;

    private options: BaseAnswer;

    ngAfterViewInit() {
    }

    loadComponent() {
        const viewContainerRef = this.answerHost.viewContainerRef;
        viewContainerRef.clear();
        this.options = null;

        if (this.question != null && this.question.questionType != null) {
            let type = null;

            switch (this.question.questionType) {
                case 0: // YesNo_Question
                break;
                case 1: // TrueFalse_Question
                    type = TrueFalseOptionComponent;
                break;
                case 2: // Multiple_Choice
                    type = MultipleChoiceComponent;
                break;
                case 3: // Open_Ended
                break;
                case 4: // Image_Chooser
                break;
            }

            if (type != null) {
                const componentFactory = this.componentFactoryResolver.resolveComponentFactory(type);
                const componentRef = viewContainerRef.createComponent(componentFactory);
                this.options = componentRef.instance as BaseAnswer;

                this.options.load(this._question);
            }
        }
    }

    getQuestion() {
        if (this._question == null) {
            return new Question();
        }

        return this._question;
    }

    getTitle() {
        if (this._question != null) {
            if (this._question.id === 0) {
                return 'New Question';
            } else {
                return 'Alter Question';
            }
        }
    }

    getTypeOfQuestions() {
        if (this.typesOfQuestion == null) {
            this.typesOfQuestion = [];
            this.apiService.getTypeOfQuestion().forEach(item => this.typesOfQuestion = item);
        }

        return this.typesOfQuestion;
    }

    changeTypeQuestion(item) {
        this.loadComponent();
    }

    protected cleanObject() {
        this._question = null;
    }

    private validateFields() {
        this.cleanErrors();

        if (this._question.description == null) {
            this.addError('Please inform the description.');
        }

        return this.hasErrors();
    }

    save() {
        if (this.validateFields()) {
            this.loading = true;

            if (this.options != null) {
                this._question.options = this.options.getQuestionOptions();
            } else {
                this._question.options = [];
            }

            this.apiService.postQuestion(this._question, this);
        }
    }

    cancel() {
        this._question = null;
        this.errors = [];
        this.loading = false;
    }

    protected onResultReceived(item: any) {
    }
}
