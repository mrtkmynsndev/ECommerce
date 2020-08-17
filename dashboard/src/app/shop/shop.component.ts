import { ShopService } from './service/shop.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/product-type';
import { ShopParam } from './model/shop-param';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;

  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  shopParam: ShopParam;

  sortOptions = [
    { name: 'Alphabatical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: Hight to Low', value: 'priceDesc' }
  ];

  totalCount: number;

  constructor(private shopService: ShopService) {
    this.shopParam = shopService.getShopParams();
   }

  ngOnInit(): void {
    this.getProducts(true);
    this.getBrands();
    this.getTypes();
  }

  getProducts(useCache = false): void {
    this.shopService.getProducts(useCache).subscribe(res => {
      this.products = res.data;
      this.totalCount = res.count;
    },
      error => {
        console.log(error);
      });
  }

  getBrands(): void {
    this.shopService.getBrands().subscribe(res => {
      this.brands = [{ id: 0, name: 'ALL' }, ...res];
    },
      error => {
        console.log(error);
      });
  }

  getTypes(): void {
    this.shopService.getTypes().subscribe(res => {
      this.types = [{ id: 0, name: 'ALL' }, ...res];
    },
      error => {
        console.log(error);
      });
  }

  onBrandSelected(brandId: number): void {
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onTypeSelected(typeId: number): void {
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onSortSelected(sort: string): void {
    const params = this.shopService.getShopParams();
    params.sort = sort;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onPageChanged(event: any): void {
    const params = this.shopService.getShopParams();
    if (this.shopParam.pageNumber !== event) {
      params.pageNumber = event;
      this.shopService.setShopParams(params);
      this.getProducts(true);
    }
  }

  onSearch(): void {
    const params = this.shopService.getShopParams();
    params.search = this.searchTerm.nativeElement.value;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onReset(): void {
    this.searchTerm.nativeElement.value = '';
    this.shopParam = new ShopParam();
    this.shopService.setShopParams(this.shopParam);
    this.getProducts();
  }
}
