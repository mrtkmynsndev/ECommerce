import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMethod';
import { Observable } from 'rxjs';
import { IOrderCreate } from 'src/app/shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createOrder(orderToCreate: IOrderCreate) {
    return this.http.post(this.baseUrl + 'order', orderToCreate);
  }

  getDeliveryMethods(): Observable<IDeliveryMethod[]> {
    return this.http.get<IDeliveryMethod[]>(this.baseUrl + 'order/deliverymethods').pipe(
      map((response: IDeliveryMethod[]) => {
        return response.sort((a, b) => b.price - a.price);
      })
    );
  }
}
