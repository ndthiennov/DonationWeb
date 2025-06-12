import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-organiser',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  templateUrl: './organiser.component.html',
  styleUrl: './organiser.component.css'
})
export class OrganiserComponent implements OnInit{

  constructor(private router: Router){

  }

  username:string | null = null;
  userava:string | null = null;

  ngOnInit(): void{
    // let first_topic_icon = document.querySelector(".first-topic-icon");
    // first_topic_icon?.classList.add("active");

    this.Loggedin();
  }

  ProfileOpen(){
    let profile = document.querySelector(".profile");

    profile?.classList.toggle("active");
  }

  Loggedin() {
    if(localStorage.getItem('token')){
      this.username = localStorage.getItem('username');
      this.userava = localStorage.getItem('userava')
    }

    return localStorage.getItem('token');
  }

  Logout(){
    localStorage.removeItem("username");
    localStorage.removeItem("userava");
    localStorage.removeItem("userid")
    localStorage.removeItem("token");

    this.router.navigateByUrl('/');
  }
}
