import { Component, Output, Input, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-top',
    template: `
    <div class="top-controls">
        <div>
            <button mat-button class="btn-new" (click)="newClick()">New</button>
        </div>
        <div>
            <input [(ngModel)]="searchContent" (keyup)="search()" color="primary" class="search-content" type='text'>
            <button mat-button class="btn-search" color="primary" (click)="search()">Search</button>
        </div>
    </div>
    `
})
export class TopComponent {

    searchContent: string;

    @Input() enableNew: boolean;
    @Output() newClickAction = new EventEmitter();
    @Output() searchClickAction = new EventEmitter<string>();

    newClick() {
        this.newClickAction.emit();
    }

    search() {
        console.log(this.searchContent);
        this.searchClickAction.emit(this.searchContent);
    }

    constructor() {
        this.enableNew = true;
    }
}
