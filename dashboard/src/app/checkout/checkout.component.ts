import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account/services/account.service';
import { Observable } from 'rxjs';
import { IBasketTotals } from '../basket/models/basket';
import { BasketService } from '../basket/services/basket.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;
  basketTotal$: Observable<IBasketTotals>;

  constructor(private fb: FormBuilder, private accounService: AccountService, private basketService: BasketService) { }

  ngOnInit(): void {
    this.createCheckoutForm();

    this.initAddressToForm();

    this.initDeliveryMethod();

    this.basketTotal$ = this.basketService.basketTotal$;
  }

  createCheckoutForm(): void {
    this.checkoutForm = this.fb.group({
      addressForm: this.fb.group({
        name: [null, Validators.required],
        lastName: [null, Validators.required],
        street: [null, Validators.required],
        city: [null, Validators.required],
        state: [null, Validators.required],
        zip: [null, Validators.required],
      }),
      deliveryForm: this.fb.group({
        deliveryMethod: [null, Validators.required],
      }),
      paymentForm: this.fb.group({
        nameOnCard: [null, Validators.required]
      })
    });
  }

  initAddressToForm(): void {
    this.accounService.getAddress().subscribe(response => {
      if (response) {
        this.checkoutForm.get('addressForm').patchValue(response);
      }
    });
  }

  initDeliveryMethod() {
    const basket = this.basketService.getBasketValue();
    if (basket.deliveryMethodId !== null) {
      this.checkoutForm.get('deliveryForm').get('deliveryMethod').patchValue(basket.deliveryMethodId.toString());
    }
  }
}
