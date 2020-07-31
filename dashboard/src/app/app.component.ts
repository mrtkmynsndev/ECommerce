import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/services/basket.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Dashboard Pannel';

  constructor(
    private basketService: BasketService
  ) { }

  ngOnInit(): void {
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
}

