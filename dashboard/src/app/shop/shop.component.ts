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
  @ViewChild('search', { static: true }) searchTerm: ElementRef;

  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  shopParam: ShopParam = new ShopParam();

  sortOptions = [
    { name: 'Alphabatical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: Hight to Low', value: 'priceDesc' }
  ];

  totalCount: number;

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts(): void {
    this.shopService.getProducts(this.shopParam).subscribe(res => {
      this.products = res.data;
      this.shopParam.pageNumber = res.pageIndex;
      this.shopParam.pageSize = res.pageSize;
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
    this.shopParam.brandId = brandId;
    this.shopParam.pageNumber = 1;
    this.getProducts();
  }

  onTypeSelected(typeId: number): void {
    this.shopParam.typeId = typeId;
    this.shopParam.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sort: string): void {
    this.shopParam.sort = sort;
    this.getProducts();
  }

  onPageChanged(event: any): void {
    if (this.shopParam.pageNumber !== event) {
      this.shopParam.pageNumber = event;
      this.getProducts();
    }
  }

  onSearch(): void {
    this.shopParam.search = this.searchTerm.nativeElement.value;
    this.getProducts();
  }

  onReset(): void {
    this.searchTerm.nativeElement.value = '';
    this.shopParam = new ShopParam();
    this.getProducts();
  }
}
