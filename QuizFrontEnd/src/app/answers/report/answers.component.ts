import { Component, ViewChild, ComponentFactoryResolver } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PublishedQuiz, Quiz, Question, ResultOperation } from '../../model/core.component';
import { AnswerDirective } from '../../answertype/baseanswer.component';
import { APIService } from '../../services/api.service';

@Component({
    selector: 'app-answersreport',
    templateUrl: 'answers.component.html'
})
export class AnswersReportComponent {
    private publishedQuiz: PublishedQuiz;
    private idAnswer: number;
    private errors: string[];

    getErrors() {
        return this.errors;
    }

    getIdAnswer() {
        return this.idAnswer;
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

    constructor(private apiService: APIService, private router: ActivatedRoute) { 
        this.errors = [];
    }

    ngOnInit() {
        const pubId = this.router.snapshot.paramMap.get('pubId');

        if (pubId != null) {
            this.apiService.getFullFinishedQuiz(pubId).subscribe(res => {
                this.publishedQuiz = res as PublishedQuiz;
                this.idAnswer = Number(pubId);
            });
        }
    }

    evaluate() {
        console.log('evaluating..');
        this.apiService.postEvaluateQuiz(this.idAnswer).subscribe(res => {
            console.log(res);
            if (res != null) {
                const resultOpt = res as ResultOperation;

                if (resultOpt.success) {
                    window.location.href = '/user/answers';
                } else {
                    this.errors = resultOpt.errors;
                }
            }
        });
    }
}
