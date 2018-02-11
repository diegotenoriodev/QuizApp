import { Component, ViewChild, ComponentFactoryResolver } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BaseAnswersComponent } from '../controls/baseanswers.component';
import { AnswerComponent } from './answer.component';
import { Question, ResultOperation, PublishedQuiz } from '../../model/core.component';
import { APIService } from '../../services/api.service';

@Component({
    selector: 'app-quizanswers',
    templateUrl: 'answers.component.html'
})
export class AnswersComponent extends BaseAnswersComponent {
    private index: number;
    private _finished = false;

    @ViewChild(AnswerComponent) answerComponent: AnswerComponent;

    finished() { return this._finished; }

    getCurrentQuestionNumber() { return this.index + 1; }

    getCurrentQuestion() {
        if (this.publishedQuiz != null) {
            console.log(this.getQuestions()[this.index]);
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
        this.answerComponent.saveCurrent();
    }

    private loadNewAnswer() {
        this.answerComponent.loadNewAnswer(this.getQuiz().id,
                    this.idAnswer,
                    this.getCurrentQuestionNumber(),
                    this.getCurrentQuestion(),
                    super.getComponentType(this.getCurrentQuestion()));
    }

    private finishQuest() {
        this._finished = true;
    }

    constructor(private apiService: APIService,
                private router: ActivatedRoute,
                private componentFactoryResolver: ComponentFactoryResolver) {
        super();
        this.index = 0;
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
