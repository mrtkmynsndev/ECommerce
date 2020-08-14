import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BussyService } from '../services/bussy.service';
import { delay, finalize } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
    constructor(
        private bussyService: BussyService
    ) {

    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (req.url.indexOf('emailexist') === 1) {
            return next.handle(req);
        }
        this.bussyService.busy();
        return next.handle(req).pipe(
            delay(1000),
            finalize(() => {
                this.bussyService.idle();
            })
        );
    }
}
