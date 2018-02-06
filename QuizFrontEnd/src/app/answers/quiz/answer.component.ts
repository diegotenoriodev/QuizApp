import { Component, ViewChild, ComponentFactoryResolver } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PublishedQuiz, Quiz, Question, ResultOperation } from '../../model/core.component';
import { IAnswerItem } from './controls/baseansweritem.component';
import { AnswerDirective } from '../../answertype/baseanswer.component';
import { TrueOrFalseAnswerComponent } from './controls/trueorfalseanswer.component';
import { MultipleChoiceAnswerComponent } from './controls/multiplechoiceanswer.component';
import { OpenEndedAnswerComponent } from './controls/openended.component';
import { APIService } from '../../services/api.service';

@Component({
    selector: 'app-quizanswers',
    templateUrl: 'answer.component.html'
})
export class AnswerComponent {
    private publishedQuiz: PublishedQuiz;
    private idAnswer: number;
    private index: number;
    private currentAnswerItem: IAnswerItem;

    private _finished = false;

    private errors: string[];

    @ViewChild(AnswerDirective) answerHost: AnswerDirective;

    finished() {
        return this._finished;
    }

    getQuiz() {
        if (this.publishedQuiz != null) {
            return this.publishedQuiz.quiz;
        }
        return new Quiz();
    }

    getQuestions() {
        if (this.publishedQuiz != null) {
            return this.publishedQuiz.questions;
        }
        return [];
    }

    getCurrentQuestionNumber() {
        return this.index + 1;
    }

    getCurrentQuestion() {
        if (this.publishedQuiz != null) {
            return this.getQuestions()[this.index];
        }

        return new Question();
    }

    classSelected(index) {
        if (this.index === index) {
            return 'selected';
        }
        return '';
    }

    previousQuestion() {
        this.saveCurrent();

        if (this.index > 0) {
            this.index -= 1;
            this.loadNewAnswer();
        }
    }

    nextQuestion() {
        this.saveCurrent();

        if (this.index < this.getQuestions().length - 1) {
            this.index += 1;
            this.loadNewAnswer();
        } else {
            this.finishQuest();
        }
    }

    goToQuestion(index: number) {
        this.saveCurrent();

        if (index >= 0 && index <= this.getQuestions().length - 1) {
            this.index = index;
        }

        this.loadNewAnswer();
    }

    cancel() {
        this._finished = false;
    }

    finishQuiz() {
        this.apiService.postFinishQuiz(this.idAnswer).subscribe(res => {
            const result = res as ResultOperation;

            if (result.success) {
                window.location.href = 'user/quizes';
            } else {
                this.errors = result.errors;
            }
        });
    }

    private saveCurrent() {
        this.currentAnswerItem.save(res => {
            console.log(res);
        });
    }

    private loadNewAnswer() {
        const viewContainerRef = this.answerHost.viewContainerRef;
        viewContainerRef.clear();
        this.currentAnswerItem = null;
        let type = null;

        switch (this.getCurrentQuestion().questionType) {
            case 1: // TrueFalse_Question
                type = TrueOrFalseAnswerComponent;
            break;
            case 2: // Multiple_Choice
                type = MultipleChoiceAnswerComponent;
            break;
            case 3: // Open_Ended
                type = OpenEndedAnswerComponent;
            break;
        }

        if (type != null) {
            const componentFactory = this.componentFactoryResolver.resolveComponentFactory(type);
            const componentRef = viewContainerRef.createComponent(componentFactory);
            this.currentAnswerItem = componentRef.instance as IAnswerItem;

            this.currentAnswerItem.quizId = this.getQuiz().id;
            this.currentAnswerItem.apiService = this.apiService;
            this.currentAnswerItem.load(this.idAnswer as number, this.getCurrentQuestion());
        }
    }

    private finishQuest() {
        this._finished = true;
    }

    constructor(private apiService: APIService,
                private router: ActivatedRoute,
                private componentFactoryResolver: ComponentFactoryResolver) {
        this.index = 0;
        this.errors = [];
    }

    ngOnInit() {
        const pubId = this.router.snapshot.paramMap.get('pubId');

        if (pubId != null) {
            this.apiService.getFullQuiz(pubId).subscribe(res => {
                this.publishedQuiz = res as PublishedQuiz;
                this.idAnswer = Number(pubId);
                this.index = 0;
                this.loadNewAnswer();
            });
        }
    }
}
