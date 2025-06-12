import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-pages',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink
  ],
  templateUrl: './pages.component.html',
  styleUrl: './pages.component.css'
})
export class PagesComponent implements OnInit {
  constructor( private router: Router) {

  }

  username:any;

  ngOnInit(): void {

    if (typeof window !== 'undefined') {
      if (localStorage.getItem('username') != null) {
        this.username = localStorage.getItem('username');
      }
      else {
        localStorage.removeItem('token');
        this.router.navigateByUrl('/login');
      }
    }

    let list = document.querySelectorAll(".navigation li");
    list[1].classList.add("hovered");
  }

  ActiveLink(event: any) {
    console.log("active");
    let list = document.querySelectorAll(".navigation li");
    list.forEach((item: any) => {
      item.classList.remove("hovered");
      if (item == event.target.parentElement) {
        item.classList.add("hovered");
      }
    });
  }

  ToggleClick() {
    let navigation = document.querySelector(".navigation");
    let mainContainer = document.querySelector(".main-container");

    navigation?.classList.toggle("active");
    mainContainer?.classList.toggle("active");
  }

  SignOut() {
    localStorage.removeItem('token');
    this.router.navigateByUrl('/login');
  }
}
