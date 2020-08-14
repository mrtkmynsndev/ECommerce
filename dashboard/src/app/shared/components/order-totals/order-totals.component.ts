import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasketTotals } from 'src/app/basket/models/basket';
import { BasketService } from 'src/app/basket/services/basket.service';

@Component({
  selector: 'app-order-totals',
  templateUrl: './order-totals.component.html',
  styleUrls: ['./order-totals.component.scss']
})
export class OrderTotalsComponent implements OnInit {
  // basketTotal$: Observable<IBasketTotals>;
  // @Input() basketTotals: IBasketTotals;
  @Input() subtotal: number;
  @Input() shipping: number;
  @Input() total: number;

  constructor(
    private basketService: BasketService
  ) { }

  ngOnInit(): void {
    //this.basketTotal$ = this.basketService.basketTotal$;
  }

}
