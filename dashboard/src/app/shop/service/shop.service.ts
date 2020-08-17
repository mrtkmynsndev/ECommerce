import { IPagination, Pagination } from './../../shared/models/pagination';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, delay } from 'rxjs/operators';
import { IBrand } from '../../shared/models/brand';
import { IType } from '../../shared/models/product-type';
import { ShopParam } from '../model/shop-param';
import { IProduct } from 'src/app/shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  private baseUrl = 'https://localhost:5001/api/';
  private productsCached: IProduct[] = [];
  private brandsCached: IBrand[] = [];
  private typesCached: IType[] = [];
  private pagination = new Pagination();
  private shopParams = new ShopParam();

  constructor(private http: HttpClient) { }

  public getProducts(useCache: boolean): Observable<IPagination> {
    if (useCache === false) {
      this.productsCached = [];
    }

    if (this.productsCached.length > 0 && useCache === true) {
      const pageRecieved = Math.ceil(this.productsCached.length / this.shopParams.pageSize);

      if (this.shopParams.pageNumber <= pageRecieved) {
        this.pagination.data =
          this.productsCached.slice((this.shopParams.pageNumber - 1) * this.shopParams.pageSize,
            this.shopParams.pageNumber * this.shopParams.pageSize);

        return of(this.pagination);
      }

    }

    let params = new HttpParams();

    if (this.shopParams.brandId) {
      params = params.append('brandId', this.shopParams.brandId.toString());
    }

    if (this.shopParams.typeId) {
      params = params.append('typeId', this.shopParams.typeId.toString());
    }

    if (this.shopParams.search) {
      params = params.append('search', this.shopParams.search);
    }

    params = params.append('sort', this.shopParams.sort);

    params = params.append('pageIndex', this.shopParams.pageNumber.toString());

    params = params.append('pageSize', this.shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'product', { observe: 'response', params })
      .pipe(
        map(response => {
          this.productsCached = [...this.productsCached, ...response.body.data];
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }

  setShopParams(params: ShopParam): void {
    this.shopParams = params;
  }

  getShopParams(): ShopParam {
    return this.shopParams;
  }

  public getProduct(id: number): Observable<IProduct> {
    const product = this.productsCached.find(x => x.id === id);
    if (product) {
      return of(product);
    }
    return this.http.get<IProduct>(this.baseUrl + 'product/' + id);
  }

  public getBrands(): Observable<IBrand[]> {
    if (this.brandsCached.length > 0) {
      return of(this.brandsCached);
    }
    return this.http.get<IBrand[]>(this.baseUrl + 'product/brands').pipe(
      map(response => {
        if (response) {
          this.brandsCached = response;
          return response;
        }
      })
    );
  }

  public getTypes(): Observable<IType[]> {
    if (this.typesCached.length > 0) {
      return of(this.typesCached);
    }
    return this.http.get<IType[]>(this.baseUrl + 'product/types').pipe(
      map(response => {
        if (response) {
          this.typesCached = response;
          return response;
        }
      })
    );
  }
}
