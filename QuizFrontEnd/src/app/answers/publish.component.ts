import { Component, ComponentFactoryResolver, ViewChild, AfterViewInit, ViewContainerRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuizPublicationInfo, Quiz, BaseComponentOperation } from '../model/core.component';
import { UserGridComponent } from '../sharedcontrols/usergrid.component';
import { AnswerDirective } from '../answertype/baseanswer.component';
import { APIService } from '../services/api.service';

@Component({
    selector: 'app-publish',
    templateUrl: 'publish.component.html'
})
export class PublishComponent extends BaseComponentOperation implements AfterViewInit {
    private publicationInfo: QuizPublicationInfo;
    private usersGridComponent: UserGridComponent;
    private _showUrl: boolean;

    @ViewChild(AnswerDirective) answerHost: AnswerDirective;

    getQuiz(): Quiz {
        return this.publicationInfo.quiz;
    }

    getPublicationInfo() {
        return this.publicationInfo;
    }

    showURL() {
        return this._showUrl;
    }

    private cleanComponents() {
        this.answerHost.viewContainerRef.clear();
        this.usersGridComponent = null;
        this._showUrl = false;
    }

    private addPrivateAccess() {
        const componentFactory = this.componentFactoryResolver.resolveComponentFactory(UserGridComponent);
        const componentRef = this.answerHost.viewContainerRef.createComponent(componentFactory);
        this.usersGridComponent = componentRef.instance as UserGridComponent;
        this.usersGridComponent.load(this.getQuiz().id);
    }

    private addPublicAccess() {
        this._showUrl = true;
    }

    protected cleanObject() {
    }
    protected onResultReceived(item: any) {
        window.location.href = '/quizes';
    }

    constructor(private apiService: APIService,
                private router: ActivatedRoute,
                private componentFactoryResolver: ComponentFactoryResolver) {
        super();

        this.publicationInfo = new QuizPublicationInfo();
        this.publicationInfo.quiz = new Quiz();
    }

    ngAfterViewInit() {
        const quizId = this.router.snapshot.paramMap.get('quizId');
        if (quizId != null) {
            this.apiService.getQuizPublicationInfo(quizId).subscribe(
                r => {
                    if (r != null) {
                        this.publicationInfo = r;
                        this.changeAccess(this.publicationInfo.access);
                    }
                }
            );
        }

        console.log(this.answerHost);
    }

    changeAccess(accessId) {
        this.cleanComponents();

        switch (accessId) {
            case 1: // public access selected
                this.addPublicAccess();
            break;
            case 2: // Private access selected
                this.addPrivateAccess();
            break;
        }
    }

    save() {
        this.publicationInfo.userIds = [];

        if (this.usersGridComponent != null) {
            this.loading = true;

            this.usersGridComponent.getUsers().forEach(
                r => {
                    if (r.isMarked) {
                        this.publicationInfo.userIds.push(r.idUser);
                    }
                });
        }

        this.apiService.postQuizPublicationInfo(this.publicationInfo, this);
    }
}
