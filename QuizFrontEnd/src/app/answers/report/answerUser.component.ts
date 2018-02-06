import { Component, OnInit, Output } from '@angular/core';
import { DataSource } from '@angular/cdk/collections';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute } from '@angular/router';
import { Quiz, ListItem, UserAnswerForQuiz } from '../../model/core.component';
import { APIService } from '../../services/api.service';

@Component({
    selector: 'app-answersuser',
    templateUrl: 'answerUser.component.html'
})
export class AnswersUserComponent {
    private quizList: Quiz[];

    quiz: Quiz;
    datasource = new AnsweredQuizDataSource(this.apiService, this.quiz);
    displayedColumns = [ 'actions', 'name', 'answeredAt', 'isOpen', 'evaluated' ];

    boolToStr(val: boolean) {
        return val ? 'Yes' : 'No';
    }

    dateToStr(val: Date) {
        if (val !== undefined && val != null) {
            return new Date(val).toLocaleDateString();
        }

        return '';
    }

    getQuizes() {
        if (this.quizList == null) {
            this.quizList = [];
            this.apiService.getQuizes().subscribe(
                item => {
                    this.quizList = item;

                    const quizId = this.route.snapshot.paramMap.get('quizId');
                    if (quizId != null) {
                        this.change(quizId);
                    }
                });
        }

        return this.quizList;
    }

    change(item) {
        this.quiz = this.quizList.find(r => r.id === Number(item));
        this.updateDataSource();
        console.log(this.datasource);
    }

    updateDataSource() {
        this.datasource = new AnsweredQuizDataSource(this.apiService, this.quiz);
    }

    constructor(private apiService: APIService, private route: ActivatedRoute) {
        this.quiz = null;
        this.quizList = null;
    }
}

export class AnsweredQuizDataSource extends DataSource<any> {

    connect(): Observable<UserAnswerForQuiz[]> {
        if (this.quiz != null) {
            console.log(this.quiz);
            return this.apiService.getAnswersForQuiz(this.quiz.id);
        }
        return this.apiService.getAnswersForQuiz(0);
    }

    disconnect(): void { }

    constructor(private apiService: APIService, private quiz: Quiz) {
        super();
    }
}
