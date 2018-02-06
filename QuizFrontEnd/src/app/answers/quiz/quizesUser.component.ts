import { Component } from '@angular/core';
import { QuizAnswer } from '../../model/core.component';
import { APIService } from '../../services/api.service';

@Component({
    selector: 'app-quizesuser',
    templateUrl: 'quizesUser.component.html'
})
export class QuizUserComponent {
    private quizAnswer: QuizAnswer[];

    getQuizAnswer() {
        return this.quizAnswer;
    }

    // 1 - New, 2 - In Progress, 3 - Complete
    getNewQuizzes() {
        if (this.quizAnswer != null) {
            return this.quizAnswer.filter(item => item.status === 1);
        }
        return [];
    }

    getInProgressQuizzes() {
        if (this.quizAnswer != null) {
            return this.quizAnswer.filter(item => item.status === 2);
        }
        return [];
    }

    getCompleteQuizzes() {
        if (this.quizAnswer != null) {
            return this.quizAnswer.filter(item => item.status === 3);
        }
        return [];
    }

    constructor(private apiService: APIService) {
    }

    ngOnInit() {
        this.apiService.getQuizesForUser().subscribe(res => {
            this.quizAnswer = res as QuizAnswer[];
        });
    }
}
