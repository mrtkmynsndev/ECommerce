import { IPagination } from './../../shared/models/pagination';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, delay } from 'rxjs/operators';
import { IBrand } from '../../shared/models/brand';
import { IType } from '../../shared/models/product-type';
import { ShopParam } from '../model/shop-param';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  private baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  public getProducts(shopParam: ShopParam): Observable<IPagination> {
    let params = new HttpParams();

    if (shopParam.brandId) {
      params = params.append('brandId', shopParam.brandId.toString());
    }

    if (shopParam.typeId) {
      params = params.append('typeId', shopParam.typeId.toString());
    }

    if (shopParam.search) {
      params = params.append('search', shopParam.search);
    }

    params = params.append('sort', shopParam.sort);

    params = params.append('pageIndex', shopParam.pageNumber.toString());

    params = params.append('pageSize', shopParam.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'product', { observe: 'response', params })
      .pipe(
        map(response => {
          return response.body;
        })
      );
  }

  public getBrands() {
    return this.http.get<IBrand[]>(this.baseUrl + 'product/brands');
  }

  public getTypes() {
    return this.http.get<IType[]>(this.baseUrl + 'product/types');
  }
}
