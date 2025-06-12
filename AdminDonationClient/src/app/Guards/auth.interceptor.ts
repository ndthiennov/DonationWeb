import { HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";

// @Injectable()
// export class AuthInterceptor implements HttpInterceptor{

//     constructor(private router:Router){}

//     intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>{
//         if(localStorage.getItem('token')!=null){
//             const clonedReq = req.clone({
//                 headers:req.headers.set('Authorization','Bearer ' + localStorage.getItem('token'))
//             });
//             return next.handle(clonedReq).pipe(
//                 tap(
//                     success=>{
//                         console.log(success);
//                     },
//                     error=>{
//                         if(error.status==401){
//                             localStorage.removeItem('token');
//                             this.router.navigateByUrl('/login');
//                         }
//                         else if(error.status==403){
//                             this.router.navigateByUrl('/forbidden');
//                         }
//                     }
//                 )
//             )
//         }
//         else
//         return next.handle(req.clone());
//     }
// }

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
    const token = localStorage.getItem('token');

    const authReq = token
        ? req.clone({ setHeaders: { Authorization: `Bearer ${token}`} })
        : req;
    
        return next(authReq);
}