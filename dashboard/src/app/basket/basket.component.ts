import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket, IBaskettem } from './models/basket';
import { BasketService } from './services/basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
  basket$: Observable<IBasket>;

  constructor(
    private basketService: BasketService
  ) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  removeBasketItem(item: IBaskettem){
    this.basketService.removeItemFromBasket(item);
    
  }

  incrementItemQuantity(item: IBaskettem){
    this.basketService.incrementItemQuantity(item);
  }

  decrementItemQuantity(item: IBaskettem){
    this.basketService.decrementItemQuantity(item);
  }
}
