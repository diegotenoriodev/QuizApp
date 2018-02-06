import { Question } from '../../../model/core.component';
import { APIService } from '../../../services/api.service';

export interface IReportItem {
    apiService: APIService;
    quizId: number;
    load(idAnswer: number, question: Question);
}

export abstract class AnswerOptionBase {
    id: number;
    question: Question;
}
