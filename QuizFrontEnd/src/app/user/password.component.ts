import { Component, Output, EventEmitter } from '@angular/core';
import { fadeInContent } from '@angular/material';
import { BaseComponentOperation, User } from '../model/core.component';
import { AuthService } from '../services/auth.service';

@Component({
    selector: 'app-password',
    templateUrl: 'password.component.html'
})
export class PasswordComponent extends BaseComponentOperation {

    private user: User;

    success: string;

    constructor(private authService: AuthService) {
        super();
        this.user = new User();
        this.loading = false;
        this.success = '';
    }

    getUser() {
        return this.user;
    }

    protected onResultReceived(item) {
        this.success = 'Password was changed!';
        this.user = new User();
        this.loading = false;
    }

    protected cleanObject() {}

    private verifyCurrentPassword() {
        if (this.user.currentPassword === '') {
            this.addError('Current password is mandatory.');
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

        this.verifyCurrentPassword();
        this.verifyPassword();

        return this.hasErrors();
    }

    save() {
        this.success = '';
        if (this.verifyFields()) {
            this.loading = true;
            this.authService.putUser(this.user, this);
        }
    }
}
