import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BussyService {
  bussyRequestCounter = 0;

  constructor(
    private spinnerService: NgxSpinnerService
  ) { }

  busy() {
    this.bussyRequestCounter++;
    this.spinnerService.show(undefined, {
      type: 'pacman',
      bdColor: 'rgba(255, 255, 255, 0.7)',
      color: '#333333'
    });
  }

  idle() {
    this.bussyRequestCounter--;
    if (this.bussyRequestCounter <= 0) {
      this.spinnerService.hide();
    }
  }
}
