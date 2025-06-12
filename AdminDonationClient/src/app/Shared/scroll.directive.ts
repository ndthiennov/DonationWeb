import { Directive, HostListener, Output, EventEmitter } from "@angular/core";

@Directive({
    selector: 'appScroll',
    standalone: true
})

export class ScrollDirective{
    @Output() scrolledToBottom = new EventEmitter<void>();

    @HostListener('window:scroll', ['$event'])
    onWindowScroll(event: Event): void {
        console.log("scroll");
        const threshold = 1500; // Distance from the bottom in pixels
        const position = window.innerHeight + window.scrollY;
        const maxScroll = document.documentElement.scrollHeight;

        if(maxScroll - position <= threshold) {
            console.log("scroll");
            this.scrolledToBottom.emit();
        }
    }
}