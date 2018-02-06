import { Component, OnInit, Output } from '@angular/core';
import { DataSource } from '@angular/cdk/collections';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute } from '@angular/router';
import { Quiz, Question, ResultOperation } from '../model/core.component';
import { APIService } from '../services/api.service';
import { MatTableDataSource } from '@angular/material';

@Component({
    selector: 'app-questions',
    templateUrl: 'questions.component.html'
})
export class QuestionsComponent {
    search: string;
    errorMessages: string[];

    quiz: Quiz;

    datasource: MatTableDataSource<Question>;
    displayedColumns = [ 'actions', 'description' ];

    @Output() question: Question;

    private quizList: Quiz[];

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
    }

    getErrors() {
        return this.errorMessages;
    }

    updateDataSource() {
        this.apiService.getQuestions(this.quiz.id).forEach(
            r => this.datasource = new MatTableDataSource<Question>(r)
        );
    }

    onNewClick() {
        if (this.quiz == null) {
            this.errorMessages = [];
            this.errorMessages.push('Quiz must be selected first!');
        } else {
            this.question = {
                id: 0,
                description: null,
                idQuiz: this.quiz.id,
                imageUrl: '',
                questionType: 0,
                options: [] };
        }
    }

    onSearchClick(search: string) {
        this.datasource.filter = search;
    }

    constructor(private apiService: APIService, private route: ActivatedRoute) {
        this.errorMessages = [];
        this.search = '';
        this.question = null;
        this.quiz = null;
        this.quizList = null;
    }

    delete(question: Question) {
        this.apiService.deleteQuestion(question.id).subscribe(res => {
            const result = res as ResultOperation;

            if (result.success) {
                this.updateDataSource();
            } else {
                this.errorMessages = result.errors;
            }
        });
    }

    editChanged(question: Question) {
        this.updateDataSource();
        this.question = null;
        this.errorMessages = [];
    }

    edit(question: Question) {
        this.question = {
            id: question.id,
            description: question.description,
            idQuiz: question.idQuiz,
            imageUrl: question.imageUrl,
            questionType: question.questionType,
            options: question.options};
    }
}

export class QuestionDataSource extends DataSource<any> {

    connect(): Observable<Question[]> {
        if (this.quiz != null) {
            console.log(this.quiz);
        }

        return this.apiService.getQuestions(0);
    }

    disconnect(): void { }

    constructor(private apiService: APIService, private quiz: Quiz) {
        super();
    }
}
