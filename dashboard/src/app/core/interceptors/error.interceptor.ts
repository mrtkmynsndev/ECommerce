import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router, NavigationExtras } from '@angular/router';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(
        private router: Router,
        private toast: ToastrService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {
                if (error.status === 400) {
                    if (error.error.errors) {
                        throw error.error;
                    } else {
                        this.toast.error(error.error.message, error.error.statusCode);
                    }
                }
                if (error.status === 401) {
                    this.toast.error(error.error.message, error.error.statusCode);
                }
                if (error.status === 404) {
                    this.router.navigateByUrl('/not-found');
                }
                if (error.status === 500) {
                    const navExtras: NavigationExtras = { state: { error: error.error } };
                    this.router.navigateByUrl('/server-error', navExtras);
                }

                return throwError(error);
            })
        );
    }
}
