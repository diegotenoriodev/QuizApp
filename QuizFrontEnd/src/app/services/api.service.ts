import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { resource } from 'selenium-webdriver/http';
import { OperationHandler, Quiz, ResultOperation, Question,
    ListItem, UserQuizPublish, QuizPublicationInfo, UserAnswerForQuiz } from '../model/core.component';
import { Observable } from 'rxjs/Observable';
import { BaseService } from './base.service';

@Injectable()
export class APIService extends BaseService {

    // Helpers
    private getUrlQuiz() {
        return this.url + 'quiz/';
    }

    private getUrlAnswer() {
        return this.url + 'answer/';
    }

    private getUrlQuestions() {
        return this.url + 'questions/';
    }

    private getUrlUser() {
        return this.url + 'user/';
    }

    constructor(http: HttpClient) {
        super(http);
    }

    // Quizes
    getQuizes(): Observable<Quiz[]> {
        return this.http.get<Quiz[]>(this.getUrlQuiz());
    }

    getQuiz(id): Observable<Quiz> {
        return this.http.get<Quiz>(this.getUrlQuiz() + id);
    }

    postQuiz(quiz: Quiz, handler: OperationHandler) {
        this.post(this.getUrlQuiz(), quiz, handler);
    }

    deleteQuiz(quiz: Quiz) {
        return this.http.delete(this.getUrlQuiz() + quiz.id);
    }

    getQtdOpenQuizes() {
        return this.http.get(this.getUrlQuiz() + 'qtdopen');
    }

    getQtdClosedQuizes() {
        return this.http.get(this.getUrlQuiz() + 'qtdclosed');
    }

    getQuizesForUser() {
        return this.http.get(this.getUrlQuiz() + 'foruser');
    }

    getAnswersForQuiz(idQuiz: number):  Observable<UserAnswerForQuiz[]> {
        return this.http.get<UserAnswerForQuiz[]>(this.getUrlQuiz() + 'forowner/' + idQuiz);
    }

    getFullQuiz(id): any {
        return this.http.get(this.getUrlQuiz() + 'published/' + id);
    }

    getFullFinishedQuiz(idQuiz): any {
        return this.http.get(this.getUrlQuiz() + 'finished/' + idQuiz);
    }

    postFinishQuiz(idAnswer) {
        return this.http.post(this.getUrlAnswer() + 'finish/', idAnswer);
    }

    postEvaluateQuiz(idAnswer) {
        return this.http.post(this.getUrlAnswer() + 'evaluate/', idAnswer);
    }

    // Questions
    getTypeOfQuestion(): Observable<ListItem[]> {
        return this.http.get<ListItem[]>(this.getUrlQuestions() + 'typelist/');
    }

    postQuestion(question: Question, handler: OperationHandler) {
        this.post(this.getUrlQuestions(), question, handler);
    }

    getQuestions(id):  Observable<Question[]> {
        return this.http.get<Question[]>(this.getUrlQuestions() + id);
    }

    getQuestion(id) {
        this.http.get(this.url + id).subscribe(res => {
            console.log(res);
        });
    }

    deleteQuestion(id) {
        return this.http.delete(this.getUrlQuestions() + id);
    }

    // question Options
    postQuestionOption(idQuestion: number, options) {
        return this.http.post(this.getUrlQuestions() + 'options/' + idQuestion, options);
    }

    getQuestionOptions(idQuestion: number): Observable<any[]> {
        return this.http.get<any[]>(this.getUrlQuestions() + 'options/' + idQuestion);
    }

    getListOptions(quizId: number, questionId: number) {
        return this.http.get(this.getUrlQuestions() + 'listoptions/' + quizId + '/' + questionId);
    }

    getListOptionsAnswer(quizId: number, questionId: number) {
        return this.http.get(this.getUrlQuestions() + 'listoptionsanswer/' + quizId + '/' + questionId);
    }

    // Publish
    getQuizPublicationInfo(id): Observable<QuizPublicationInfo> {
        return this.http.get<QuizPublicationInfo>(this.getUrlQuiz() + 'publication/' + id);
    }

    postQuizPublicationInfo(quizPublication: QuizPublicationInfo, handler: OperationHandler) {
        this.post(this.getUrlQuiz() + 'publication/', quizPublication, handler);
    }

    getUsersQuiz(idQuiz) {
        return this.http.get(this.getUrlUser() + 'quiz/' + idQuiz);
    }

    // Answers
    getAnswerForQuestion(quizId: number, questionId: number) {
        return this.http.get(this.getUrlAnswer() + quizId + '/' + questionId);
    }

    getAnswerReportForQuestion(idAnswer: number, questionId: number) {
        return this.http.get(this.getUrlAnswer() + 'report/' + idAnswer + '/' + questionId);
    }

    postAnswerForQuestion(questionType, answer) {
        return this.http.post(this.getUrlAnswer() + questionType + '/', answer);
    }

}
