import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';
import { APIService } from './services/api.service';
import { AfterViewInit } from '@angular/core/src/metadata/lifecycle_hooks';

@Component({
    selector: 'app-main',
    templateUrl: 'main.component.html'
})
export class MainComponent {
    private _class = 'hidden';

    mainMenuResponsiveClass = 'topnav';
    classProfileMenu = 'hide-profile';

    private qtdOpen = 0;
    private qtdClosed = 0;
    private profileClick: boolean;
    private menuClick: boolean;

    bodyClick() {
        if (!this.profileClick) {
            this.classProfileMenu = 'hide-profile';
        }

        if (!this.menuClick) {
            this.mainMenuResponsiveClass = 'topnav';
        }

        this.profileClick = false;
        this.menuClick = false;
    }

    getQtdOpenQuizes() {
        return this.qtdOpen;
    }

    getQtdClosedQuizes() {
        return this.qtdClosed;
    }

    getTotal() {
        return this.getQtdOpenQuizes() + this.getQtdClosedQuizes();
    }

    constructor(private authService: AuthService,
        private apiService: APIService) {
    }

    hideMenu() {
        this.mainMenuResponsiveClass = 'topnav';
        this.classProfileMenu = 'hide-profile';
    }

    changeProfileMenu() {
        this.profileClick = true;
        this.menuClick = false;

        if (this.classProfileMenu === 'hide-profile') {
            this.classProfileMenu = 'show-prifile';
        } else {
            this.classProfileMenu = 'hide-profile';
        }
    }

    changeMenu() {
        this.profileClick = false;
        this.menuClick = true;

        if (this.mainMenuResponsiveClass === 'topnav') {
            this.mainMenuResponsiveClass += ' responsive';
        } else {
            this.mainMenuResponsiveClass = 'topnav';
        }
    }

    getMessageClosedQuizes() {
        if (this.qtdClosed === 0) {
            return 'You don\'t have any finished quiz.';
        } else if (this.qtdClosed === 1) {
            return 'You have one finished quiz waiting for your evaluation.';
        } else {
            return 'You have ' + this.qtdClosed + ' finished quizes waiting for your evaluation.';
        }
    }

    getMessageOpenQuizes() {
        if (this.qtdOpen === 0) {
            return 'You don\'t have any open quiz to answer.';
        } else if (this.qtdOpen === 1) {
            return 'You have one quiz waiting for your answers.';
        } else {
            return 'There are ' + this.qtdOpen + ' quizes for you to answer.';
        }
    }

    ngOnInit() {
        this.loadQtdCosedQuizes();
        this.loadQtdOpenQuizes();
    }

    loadQtdOpenQuizes() {
        this.apiService.getQtdOpenQuizes().forEach(r => {
            this.qtdOpen = r as number;
        });
    }

    loadQtdCosedQuizes() {
        this.apiService.getQtdClosedQuizes().forEach(r => {
            this.qtdClosed = r as number;
        });
    }

    exit() {
        this.authService.logout();
        this.hideMenu();
    }

    isAuthenticated() {
        return this.authService.isAuthenticated();
    }
}
