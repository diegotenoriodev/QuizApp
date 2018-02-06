import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-loading',
    template:
    `<div class="overall {{getLoadingClass()}}">
        <img src="https://www.smartportfolios.com/assets/img/loading.gif" alt="loading" />
    </div>`
})
export class LoadingComponent {

    @Input() isLoading: boolean;

    getLoadingClass() {
        if (this.isLoading) {
            return 'show';
        } else {
            return ' hidden';
        }
    }
}
