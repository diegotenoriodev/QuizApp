import { Component, Output, EventEmitter } from '@angular/core';
import { fadeInContent } from '@angular/material';
import { BaseComponentOperation, User } from '../model/core.component';
import { AuthService } from '../services/auth.service';

@Component({
    selector: 'app-register',
    templateUrl: 'register.component.html'
})
export class RegisterComponent extends BaseComponentOperation {

    private user: User;
    @Output() loginClick = new EventEmitter();

    constructor(private authService: AuthService) {
        super();
        this.user = new User();
        this.loading = false;
    }

    getUser() {
        return this.user;
    }

    protected onResultReceived(item) {
        window.location.href = '/quizes';
    }

    protected cleanObject() {}

    private verifyUserame() {
        if (this.user.username === '') {
            this.addError('Username is mandatory.');
        }
    }

    private verifyEmail() {
        if (this.user.email === '') {
            this.addError('Email is mandatory.');
        }
    }

    private verifyPassword() {
        if (this.user.password === '') {
            this.addError('Password is mandatory.');
        } else if (this.user.confirmPassword === '') {
            this.addError('Password confirmation is mandatory.');
        } else {
            if (this.user.password !== this.user.confirmPassword) {
                this.addError('Password and password confirmation do not match.');
            }
        }
    }

    private verifyFields() {
        this.cleanErrors();

        this.verifyUserame();
        this.verifyEmail();
        this.verifyPassword();

        return this.hasErrors();
    }

    save() {
        if (this.verifyFields()) {
            this.loading = true;
            this.authService.postNewUser(this.user, this);
        }
    }

    onLoginClick() {
        this.loginClick.emit();
    }
}
