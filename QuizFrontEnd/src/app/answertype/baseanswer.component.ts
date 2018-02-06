import { Directive, ViewContainerRef } from '@angular/core';
import { Question, BaseQuestionOption } from '../model/core.component';

@Directive({
  selector: 'answer-host',
})
export class AnswerDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }

}

export interface BaseAnswer {
    question: Question;

    getQuestionOptions(): BaseQuestionOption[];

    // save(question: Question, successCallback, errorCallback);
    load(question: Question);
}
