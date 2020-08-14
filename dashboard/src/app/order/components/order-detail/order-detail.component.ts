import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/shared/models/order';
import { OrderService } from '../../services/order.service';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss']
})
export class OrderDetailComponent implements OnInit {
  order: IOrder;

  constructor(
    private activatedRoute: ActivatedRoute,
    private orderService: OrderService,
    private bc: BreadcrumbService
  ) {
    this.bc.set('@OrderDetailed', '');
  }

  ngOnInit(): void {
    this.initOrder();
  }

  initOrder(): void {
    this.orderService.getOrder(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(response => {
      if (response) {
        this.bc.set('@OrderDetailed', `# ${response.id} ${response.status}`);
        this.order = response;
        console.log(response);
      }
    },
    error => {
      console.log(error);
    });
  }

}
