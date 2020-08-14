import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/services/basket.service';
import { CheckoutService } from '../../services/checkout.service';
import { ToastrService } from 'ngx-toastr';
import { IBasket } from 'src/app/basket/models/basket';
import { IOrderCreate } from '../../../shared/models/order';
import { Router, NavigationExtras } from '@angular/router';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm: FormGroup;

  constructor(
    private basketService: BasketService,
    private checkoutService: CheckoutService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  onSubmitOrder(): void {
    const basket = this.basketService.getBasketValue();
    const orderToCreate = this.getOrderToCreate(basket);
    this.checkoutService.createOrder(orderToCreate).subscribe(response => {
      if (response) {
        this.toastr.success('Order created successfully');
        
        this.basketService.deleteLocalBasket(basket.id);

        const navExtras: NavigationExtras = { state: response };

        this.router.navigate(['checkout/success'], navExtras);
      }
    },
      error => {
        console.log(error);
        this.toastr.error('Opss! an error occured while creatin order!');
      });
  }

  getOrderToCreate(basket: IBasket): IOrderCreate {
    return {
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      shippToAdress: this.checkoutForm.get('addressForm').value
    };
  }
}
