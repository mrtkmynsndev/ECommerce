import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { IBasket, IBasketItem, Basket, IBasketTotals } from '../models/basket';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { IProduct } from 'src/app/shared/models/product';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  basketKey = environment.basketKey;

  private basketSource: BehaviorSubject<IBasket> = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalSource: BehaviorSubject<IBasketTotals> = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSource.asObservable();
  private shipping = 0;

  constructor(private http: HttpClient) { }

  getBasket(id: string): Observable<IBasket> {
    return this.http.get<IBasket>(this.baseUrl + 'basket?id=' + id)
      .pipe(
        map(response => {
          this.basketSource.next(response);
          this.calculateTotals();
          return response;
        })
      );
  }

  setBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket', basket).subscribe((response: IBasket) => {
      this.basketSource.next(response);
      this.calculateTotals();
    },
      error => {
        console.log(error);
      });
  }

  setShippingPrice(deliveryMethod: IDeliveryMethod) {
    this.shipping = deliveryMethod.price;
    this.calculateTotals();
  }

  incrementItemQuantity(item: IBasketItem) {
    const basket = this.getBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem) {
    const basket = this.getBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if (basket.items[foundItemIndex].quantity > 1) {
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    } else {
      this.removeItemFromBasket(item);
    }
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getBasketValue();
    if (basket.items.some(x => x.id === item.id)) {
      basket.items = basket.items.filter(x => x.id !== item.id);
      if (basket.items.length > 0) {
        this.setBasket(basket);
      } else {
        this.deleteBasket(basket);
      }
    }
  }

  deleteBasket(basket: IBasket) {
    this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(response => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem(this.basketKey);
    },
      error => {
        console.log(error);
      });
  }

  deleteLocalBasket(id: string): void {
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem(this.basketKey);
  }

  private calculateTotals() {
    const basket = this.getBasketValue();
    const shipping = this.shipping;
    const subtotal = basket.items.reduce((p, c) => (c.price * c.quantity) + p, 0);
    const total = shipping + subtotal;

    this.basketTotalSource.next({ shipping, total, subtotal });
  }

  addItemToBasket(product: IProduct, quantity = 1): void {
    const itemToAdd: IBasketItem = this.mapProductToBasket(product, quantity);
    const basket = this.getBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItems(basket.items, itemToAdd, quantity);

    this.setBasket(basket);
  }

  addOrUpdateItems(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const itemIndex = items.findIndex(x => x.id === itemToAdd.id);

    if (itemIndex === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      items[itemIndex].quantity += quantity;
    }

    return items;
  }

  createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem(this.basketKey, basket.id);
    return basket;
  }

  mapProductToBasket(product: IProduct, quantity: number): IBasketItem {
    return {
      id: product.id,
      productName: product.name,
      price: product.price,
      quantity,
      brand: product.productBrand,
      type: product.productType,
      pictureUrl: product.pictureUrl
    };
  }

  getBasketValue(): IBasket {
    return this.basketSource.value;
  }
}
