import { APIService } from '../../services/api.service';
import { Question } from '../../model/core.component';


export interface IAnswerItem {
    apiService: APIService;
    quizId: number;
    readOnly: boolean;
    save(callBackFuntion);
    load(idAnswer: number, question: Question);
}

export abstract class BaseAnswerOption {
    id: number;
    question: Question;
}
