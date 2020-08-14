import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { IOrder } from 'src/app/shared/models/order';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseUrl = environment.apiUrl;

  constructor(
    private http: HttpClient
  ) { }

  getOrders(): Observable<IOrder[]> {
    return this.http.get<IOrder[]>(this.baseUrl + 'order');
  }

  getOrder(id: number): Observable<IOrder> {
    return this.http.get<IOrder>(this.baseUrl + 'order/' + id);
  }
}
