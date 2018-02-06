import { Injectable } from '@angular/core';
import { HttpInterceptor } from '@angular/common/http';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    private APP_TOKEN_NAME = 'app-token';

    getToken() {
        return localStorage.getItem(this.APP_TOKEN_NAME);
    }

    intercept(req, next) {
        const authReq = req.clone({
            headers: req.headers.set('Authorization', `Bearer ${this.getToken()}`)
        });

        return next.handle(authReq);
    }
}
