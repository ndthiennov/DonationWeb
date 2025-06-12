import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    RouterOutlet
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit{

  constructor(private router:Router){}

  ngOnInit(): void {
    let currentUrl = this.router.url.split("/");
    this.ActiveLink(currentUrl[currentUrl.length - 1].toUpperCase());
  }

  ActiveLink(e:any){
    var textBox = <HTMLInputElement>document.querySelector(".text-box");
    textBox.value = e;

    if(e=="ADMIN"){
      this.router.navigateByUrl('/users/admin');
    }
    else if (e=="ORGANISER"){
      this.router.navigateByUrl('/users/organiser');
    }
    else if (e=="DONOR"){
      this.router.navigateByUrl('/users/donor');
    }
    else if (e=="RECIPIENT"){
      this.router.navigateByUrl('/users/recipient');
    }
    else{
      this.router.navigateByUrl('/users/uncensored');
    }
  }

  UserDropdownOpen(){
    var userDropdown = document.querySelector(".user-dropdown");
    userDropdown?.classList.toggle("active");
  }
}
