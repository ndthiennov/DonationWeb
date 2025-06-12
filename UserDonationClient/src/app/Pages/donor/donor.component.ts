import { NgFor, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { getToken, Messaging, onMessage } from '@angular/fire/messaging';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UserTokenService } from '../../Services/user-token.service';
import { HttpEventType } from '@angular/common/http';
import { NotificationService } from '../../Services/notification.service';
import { SharedService } from '../../Shared/shared.service';

@Component({
  selector: 'app-donor',
  standalone: true,
  imports: [RouterOutlet, NgIf, RouterLink, NgFor],
  templateUrl: './donor.component.html',
  styleUrl: './donor.component.css'
})
export class DonorComponent implements OnInit {

  constructor(private userTokenService: UserTokenService,
    private notificationService: NotificationService,
    public sharedService: SharedService,
    private router: Router) {

  }

  // FCM Push Notification
  private readonly _messaging = inject(Messaging);
  private readonly _message = new BehaviorSubject<unknown | undefined>(undefined);
  message$ = this._message.asObservable();
  payload: string | null = null;

  pageIndexNoti: number = 0;
  requestNoti: boolean = true;
  responseNoti: any;
  notiList: Array<any> = [];
  notiNum: number = 0;
  openedNoti: boolean = false;

  username: string | null = null;
  userava: string | null = null;

  ngOnInit(): void {
    // Convert res to base64
    if (localStorage.getItem('token') != null) {
      let base64 = localStorage.getItem('token')!.replace(/-/g, '+').replace(/_/g, '/');
      while (base64.length % 4) {
        base64 += '=';
      }

      var payLoad = JSON.parse(decodeURIComponent(escape(window.atob(base64!.split('.')[1]))));
      var userRole = payLoad['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      var userId = payLoad['Id'];

      if (userRole == "donor") {
        this.CheckReadLatestNotification();
        // this.router.navigateByUrl('/');

        // // FCM Push Notification
        // this.GetTokenFcm();
        // onMessage(this._messaging, (payload) => {
        //   console.log('Message received. ', payload);
        //   // You can show a notification or update the UI based on the payload
        // });
      }
      else if (userRole == "organiser") {
        this.router.navigateByUrl('/organiser/profile/' + userId);
      }
      else if (userRole == "recipient") {
        this.router.navigateByUrl('/recipient/profile/' + userId);
      }
      else {
        this.Logout();
      }
    }

    let first_topic_icon = document.querySelector(".first-topic-icon");
    first_topic_icon?.classList.add("active");

    // FCM Push Notification
    this.GetTokenFcm();
    onMessage(this._messaging, (payload) => {
      console.log('Message received. ', payload);
      // You can show a notification or update the UI based on the payload
    });

    if ('serviceWorker' in navigator) {
      navigator.serviceWorker.register('/firebase-messaging-sw.js')
        .then((registration) => {
          console.log('Service Worker registered with scope:', registration.scope);
        })
        .catch((err) => {
          console.error('Service Worker registration failed:', err);
        });
      navigator.serviceWorker.addEventListener('message', (event) => {
        const payload = event.data;
        console.log("SvNo: ", payload);

        let bell = document.getElementById("bell_noti");
        if (this.notiNum == 0) {
          bell?.classList.add("bell");
        }
        console.log(this.notiNum);
        this.notiNum += 1;
        console.log(this.notiNum);
        bell?.setAttribute('current-count', this.notiNum + "")
      })
    }
  }

  // FCM Push Notification
  GetTokenFcm() {
    if (localStorage.getItem("fcmtoken") == null) {
      getToken(this._messaging, { vapidKey: environment.firebaseConfig.vapidKey })
        .then((token) => {
          console.log(token)
          localStorage.setItem("fcmtoken", token);
          this.userTokenService.Add(JSON.stringify(token)).subscribe(
            (res: any) => {
              if (res.type === HttpEventType.Response) {
                console.log(res);
              }
            },
            (err: any) => {
              console.log(err);
            }
          );
        })
        .catch((err) => {
          console.log(err)
        })
    }
  }

  // Notification
  CheckReadLatestNotification() {
    this.notificationService.CheckReadLatestNotification().subscribe(
      (res: any) => {
        if (res.type === HttpEventType.Response) {
          this.notiNum = res.body;
          if (this.notiNum > 0) {
            let bell = document.getElementById("bell_noti");
            bell?.setAttribute('current-count', this.notiNum + "")
            bell?.classList.add("bell");
          }
        }
      },
      err => {
        console.log(err);
      }
    )
  }

  GetNotification() {
    if (this.requestNoti) {
      this.pageIndexNoti += 1;
      this.notificationService.Get(this.pageIndexNoti).subscribe(
        (res: any) => {
          if (res.type === HttpEventType.Response) {
            this.responseNoti = res.body.$values;
            if (this.responseNoti.length < 3) {
              this.requestNoti = false;
            }
            this.notiList = this.notiList.concat(this.responseNoti);
            console.log(this.requestNoti);
            console.log(res);
          }
        },
        err => {
          console.log(err);
        }
      )
    }
  }

  ProfileOpen() {
    let profile = document.querySelector(".profile");
    let notification = document.querySelector(".notifications");

    profile?.classList.toggle("active");
    notification?.classList.remove("active");
  }

  NotificationOpen() {
    let profile = document.querySelector(".profile");
    let notification = document.querySelector(".notifications");

    notification?.classList.toggle("active");
    profile?.classList.remove("active");

    if (!this.openedNoti) {
      this.notiNum = 0;
      let bell = document.getElementById("bell_noti");
      bell?.setAttribute('current-count', this.notiNum + "")
      bell?.classList.remove("bell");

      this.GetNotification();
      this.openedNoti = !this.openedNoti;
    }
    else {
      this.openedNoti = !this.openedNoti;
    }
  }

  ActiveTopic(event: any) {
    let topicIcons = document.querySelectorAll(".topic-icon");
    topicIcons.forEach(item => {
      item.classList.remove("active");
      if (item == event.target.parentElement) {
        item.classList.add("active");
      }
    })
  }

  Loggedin() {
    if (localStorage.getItem('token')) {
      this.username = localStorage.getItem('username');
      this.userava = localStorage.getItem('userava')
    }

    return localStorage.getItem('token');
  }

  Logout() {
    localStorage.removeItem("username");
    localStorage.removeItem("userava");
    localStorage.removeItem("userid")
    localStorage.removeItem("token");
    localStorage.removeItem("fcmtoken");
  }
}
