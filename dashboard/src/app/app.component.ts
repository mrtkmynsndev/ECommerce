import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/services/basket.service';
import { environment } from 'src/environments/environment';
import { AccountService } from './account/services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Dashboard Pannel';

  constructor(
    private basketService: BasketService,
    private accountService: AccountService
  ) { }

  ngOnInit(): void {
    this.initBasket();
    this.initCurrentUser();
  }

  initBasket(): void {
    const basketId = localStorage.getItem(environment.basketKey);
    if (basketId) {
      this.basketService.getBasket(basketId).subscribe(() => {
        console.log('basket initialized');
      },
        error => {
          console.log(error);
        });
    }
  }

  initCurrentUser(): void {
    const tokenValue = localStorage.getItem(environment.tokenKey);
    this.accountService.loadCurrentUser(tokenValue).subscribe(result => {
      console.log('current user initialized');
    },
      error => {
        console.log(error);
      });
  }
}

