import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
    selector: 'app-guest',
    templateUrl: 'guest.component.html'
})
export class GuestComponent {

    private newUserShowing: boolean;

    newUserClick() {
        this.newUserShowing = !this.newUserShowing;
        console.log('newuser click');
    }

    constructor(private auth: AuthService) {
        if (auth.isAuthenticated()) {
            window.location.href = '/quizes';
        }
        this.newUserShowing = false;
    }

    getClassMainBox() {
        if (this.newUserShowing) {
            return ' col-90';
        } else {
            return ' col-40';
        }
    }

    getClassRegister() {
        if (this.newUserShowing) {
            return 'show';
        } else {
            return 'hidden';
        }
    }

    getClassLogin() {
        if (this.newUserShowing) {
            return 'hidden';
        } else {
            return 'show';
        }
    }
}
