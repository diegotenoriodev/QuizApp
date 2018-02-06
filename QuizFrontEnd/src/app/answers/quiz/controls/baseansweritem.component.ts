import { Question } from '../../../model/core.component';
import { APIService } from '../../../services/api.service';

export interface IAnswerItem {
    apiService: APIService;
    quizId: number;
    save(callBackFuntion);
    load(idAnswer: number, question: Question);
}

export abstract class AnswerOptionBase {
    id: number;
    question: Question;
}
