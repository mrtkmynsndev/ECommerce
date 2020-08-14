import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AccountService } from 'src/app/account/services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-checkout-adress',
  templateUrl: './checkout-adress.component.html',
  styleUrls: ['./checkout-adress.component.scss']
})
export class CheckoutAdressComponent implements OnInit {
  @Input() checkoutForm: FormGroup;

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
  }

  saveUserAddress(): void {
    const address = this.checkoutForm.get('addressForm').value;

    if (address) {
      this.accountService.updateAddress(address).subscribe(response => {
        this.toastr.success('Address saved');
      },
        error => {
          console.log(error);
          this.toastr.error(error.message);
        });
    } else {
      this.toastr.warning('Current Address is invalid');
    }
  }
}
