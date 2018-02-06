import { Component, Output, EventEmitter } from '@angular/core';
import { BaseComponentOperation, LoginCredentials } from '../model/core.component';
import { AuthService } from '../services/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: 'login.component.html'
})
export class LoginComponent extends BaseComponentOperation {
    private login: LoginCredentials;

    @Output() newUserClickEvent = new EventEmitter();

    getLogin() {
        return this.login;
    }

    constructor(private authService: AuthService) {
        super();
        this.login = new LoginCredentials();
    }

    newUserClick() {
        this.newUserClickEvent.emit();
    }

    authenticate() {
        this.loading = true;
        this.authService.login(this.login, this);
    }

    protected cleanObject() {
        this.login = null;
    }

    protected onResultReceived(item: any) {
        window.location.href = '/';
     }
}
