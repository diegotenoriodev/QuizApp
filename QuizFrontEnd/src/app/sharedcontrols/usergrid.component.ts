import { Component, Input } from '@angular/core';
import { DataSource } from '@angular/cdk/collections';
import { Observable } from 'rxjs/Observable';
import { UserQuizPublish } from '../model/core.component';
import { APIService } from '../services/api.service';

@Component({
    selector: 'app-usergrid',
    templateUrl: 'usergrid.component.html'
})
export class UserGridComponent {

    private idQuiz: number;
    private userQuiz: UserQuizPublish[];

    private _checkAllMarked: boolean;

    set checkAllMarked(_checkAllMarked: boolean) {
        this._checkAllMarked = _checkAllMarked;

        this.userQuiz.forEach(item => {
            console.log(item);
            item.isMarked = this._checkAllMarked;
        });
    }

    getUsers() {
        return this.userQuiz;
    }

    load(idQuiz) {
        this.idQuiz = idQuiz;
        this.apiService.getUsersQuiz(idQuiz).subscribe(res => this.userQuiz = res as UserQuizPublish[]);
    }

    getStatus(user: UserQuizPublish) {
        if (user.isMarked) {
            if (user.hasAnswered) {
                return 'User completed quiz';
            } else {
                return 'Selected, waiting for answer';
            }
        } else {
            return 'Not Selected';
        }
    }

    constructor(private apiService: APIService) {
    }
}
