import { IAddress } from './address';

export interface IOrderCreate {
    basketId: string;
    deliveryMethodId: number;
    shippToAdress: IAddress;
}

export interface IOrder {
    id: number;
    buyerUserName: string;
    shipToAdress: IAddress;
    orderDate: string;
    deliveryMethod: string;
    shippingPrice: number;
    orderItems: IOrderItem[];
    subTotal: number;
    total: number;
    status: string;
}

export interface IOrderItem {
    productId: number;
    productName: string;
    pictureUrl: string;
    price: number;
    quantity: number;
}