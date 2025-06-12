import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css'
})
export class ToastComponent implements OnInit {

  constructor() {

  }

  ngOnInit(): void {

  }

  Close(){
    var toastContainer = document.getElementById("toast-container");
    var progress = document.getElementById("progress");
    toastContainer?.classList.remove("active")
    progress?.classList.remove("active");
  }
}