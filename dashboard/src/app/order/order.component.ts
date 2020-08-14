import { Component, OnInit } from '@angular/core';
import { OrderService } from './services/order.service';
import { IOrder } from '../shared/models/order';
import { Router, NavigationExtras } from '@angular/router';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {
  orders: IOrder[];

  constructor(
    private orderService: OrderService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initOrders();
  }

  initOrders(): void {
    this.orderService.getOrders().subscribe(response => {
      if (response) {
        this.orders = response;
      }
    },
      error => {
        console.log(error);
      });
  }

}
