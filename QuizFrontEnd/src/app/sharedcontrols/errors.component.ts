import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-errors',
    template: `
    <ul class="errors">
        <li *ngFor="let item of getErrors();">
            {{item}}
        </li>
    </ul>`
})
export class ErrorsComponent {

    @Input() errors: string[];

    getErrors() {
        return this.errors;
    }
}
