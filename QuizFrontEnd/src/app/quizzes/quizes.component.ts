import { Component, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Quiz, ResultOperation } from '../model/core.component';
import { APIService } from '../services/api.service';
import { MatTableDataSource } from '@angular/material';

@Component({
    selector: 'app-quizes',
    templateUrl: 'quizes.component.html'
})
export class QuizesComponent {

    search: string;
    errorMessages: string[];

    datasource: MatTableDataSource<Quiz>;
    displayedColumns = [ 'actions', 'name', 'description', 'qtdQuestions', 'qtdAnswers' ];

    @Output() quiz: Quiz;

    getErrors() {
        return this.errorMessages;
    }

    hasAnswers(quiz: Quiz) {
        return quiz.qtdAnswers > 0;
    }

    hasQuestions(quiz: Quiz) {
        return quiz.qtdQuestions > 0;
    }

    updateDataSource() {
        this.apiService.getQuizes().forEach(r => {
            this.datasource = new MatTableDataSource<Quiz>(r);
        });
    }

    onNewClick() {
        this.quiz = { id: 0, description: null, name: null, qtdQuestions: 0, qtdAnswers: 0 };
        console.log(this.quiz);
    }

    onSearchClick(search: string) {
        this.datasource.filter = search;
    }

    constructor(private apiService: APIService) {
        this.errorMessages = [];
        this.search = '';
        this.quiz = null;
    }

    delete(quiz: Quiz) {
        this.apiService.deleteQuiz(quiz).subscribe(res => {
            const result = res as ResultOperation;

            if (result.success) {
                this.updateDataSource();
            } else {
                this.errorMessages = result.errors;
            }
        });
    }

    editChanged(quiz: Quiz) {
        this.updateDataSource();

        this.quiz = null;
        this.errorMessages = [];
    }

    edit(quiz: Quiz) {
        this.quiz = { id: quiz.id, name: quiz.name, description: quiz.description, qtdQuestions: 0, qtdAnswers: 0  };
    }

    ngOnInit() {
        this.updateDataSource();
    }
}
