import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User, LoginCredentials, OperationHandler, ResultOperation } from '../model/core.component';
import { BaseService } from './base.service';

@Injectable()
export class AuthService extends BaseService {
    private APP_TOKEN_NAME = 'app-token';

    private out() {
        localStorage.removeItem(this.APP_TOKEN_NAME);
    }

    private setToken(token: string) {
        localStorage.setItem(this.APP_TOKEN_NAME, token);
    }

    private getUrl() {
        return this.url + 'user/';
    }

    constructor(http: HttpClient) {
        super(http);
    }

    postNewUser(user: User, handler: OperationHandler) {
        this.post(this.getUrl(), user, {
            handlerResult: (result: ResultOperation) => {
                if (result.success) {
                    this.setToken((result.object as AuthReturn).token.toString());
                }
                handler.handlerResult(result);
            },
            handleError: error => handler.handleError(error)
        });
    }

    putUser(user: User, handler: OperationHandler) {
        this.put(this.getUrl(), user, handler);
    }

    login(credentials: LoginCredentials, handler: OperationHandler) {
        this.post(this.getUrl() + 'login', credentials, {
            handlerResult: (result: ResultOperation) => {
                if (result.success) {
                    this.setToken((result.object as AuthReturn).token.toString());
                }
                handler.handlerResult(result);
            },
            handleError: error => handler.handleError(error)
        });
    }

    logout() {
        this.post(this.getUrl() + 'logout', { token: this.getToken() }, {
            handlerResult: (result: ResultOperation) => this.out(),
            handleError: error => this.out()
        });
    }

    isAuthenticated(): boolean {
        return this.getToken() !== null;
    }

    getToken() {
        return localStorage.getItem(this.APP_TOKEN_NAME);
    }
}

class AuthReturn {
    token: string;
}
