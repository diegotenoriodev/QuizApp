import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BaseAnswersComponent } from '../controls/baseanswers.component';
import { APIService } from '../../services/api.service';
import { PublishedQuiz, ResultOperation } from '../../model/core.component';

@Component({
    selector: 'app-answersreport',
    templateUrl: 'answers.component.html'
})
export class AnswersReportComponent extends BaseAnswersComponent {
    constructor(private apiService: APIService, private router: ActivatedRoute) {
        super();
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
