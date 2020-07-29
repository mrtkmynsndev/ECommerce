import { Component, OnInit } from '@angular/core';
import { ShopService } from '../../service/shop.service';
import { IProduct } from 'src/app/shared/models/product';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  product: IProduct;

  constructor(
    private shopService: ShopService,
    private route: ActivatedRoute,
    private bcService: BreadcrumbService) {
      this.bcService.set('@productDetails', '');
     }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(): void {

    this.shopService.getProduct(+this.route.snapshot.paramMap.get('id')).subscribe(response => {
      this.product = response;
      this.bcService.set('@productDetails', this.product.name);
    },
      error => {
        console.log(error);
      });
  }
}
