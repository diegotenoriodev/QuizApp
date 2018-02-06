import { HttpClient } from '@angular/common/http';
import { OperationHandler, ResultOperation } from '../model/core.component';

export abstract class BaseService {
    protected url = 'localhost:4000/quizapp/api/';

    protected post(url, obj, handler: OperationHandler) {
        this.http.post(url, obj).subscribe(
            res => {
                handler.handlerResult(res as ResultOperation);
            },
            err => {
                handler.handleError(err);
            }
        );
    }

    protected put(url, obj, handler: OperationHandler) {
        this.http.put(url, obj).subscribe(
            res => {
                handler.handlerResult(res as ResultOperation);
            },
            err => {
                handler.handleError(err);
            }
        );
    }

    // Dependence injection:
    constructor(protected http: HttpClient) { }
}
