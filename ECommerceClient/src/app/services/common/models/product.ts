import { Injectable } from '@angular/core';
import { HttpClientService } from '../http-client';
import { Create_Product } from '../../../contracts/create_product';
import { HttpErrorResponse } from '@angular/common/http';
import { List_Product } from '../../../contracts/list_product';
import { firstValueFrom, Observable } from 'rxjs';
import {List_Product_Image} from '../../../contracts/list_product_image';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  constructor(private httpClientService: HttpClientService) {}

  create (product: Create_Product, succesCallBack?: () => void, errorCallBack?: (errorMessage: string) => void){
    this.httpClientService.post({
      controller: "products"
    }, product).subscribe(result => {
      succesCallBack();
    }, (errorResponse: HttpErrorResponse) => {
      const _error: Array<{key:string, value: Array<string> }> = errorResponse.error;
      let message = "";
      _error.forEach((v, index) => {
        v.value.forEach((_v, _index) => {
          message += `${_v}<br>`;
        });
      });
      errorCallBack(message);
    })
  };

  //İstenen sayıda veriyi listeme için number ve size ekledim
  //Application içinde RequestParameters dosyası oluşturup içinde Pagination.cs oluşturdum
  //ProductController.cs içindeki ilgili metoda parametre olarak verdim
  async read(page: number = 0, size: number = 5, succesCallBack?: () => void, errorCallBack?: (errorMessage: string) => void): Promise<{ totalCount: number; products: List_Product[]}> {
    const promiseData: Promise<{ totalCount: number; products: List_Product[]}> = this.httpClientService.get<{ totalCount: number; products: List_Product[]}>({
      controller: "products",
      queryString: `page=${page}&size=${size}`
    }).toPromise();

    promiseData.then(d => succesCallBack())
      .catch((errorResponse: HttpErrorResponse) => errorCallBack(errorResponse.message))

    return await promiseData
  }

  async delete(id: string){
    const deleteObservable: Observable<any> = this.httpClientService.delete<any>({
      controller: "products"
    }, id);

    await firstValueFrom(deleteObservable);
  }

  async readImages(id: string, successCallback?: () => void) : Promise<List_Product_Image[]> {
    const getObservable: Observable<List_Product_Image[]> = this.httpClientService.get<List_Product_Image[]>({
      action: "getproductimages",
      controller: "products",
    },id);

    const images: List_Product_Image[] = await firstValueFrom(getObservable);
    successCallback()
    return images;
  }

  async deleteImage(id: string, ImageId: string, successCallback?: () => void ) {
    const deleteObservable = this.httpClientService.delete({
      controller: "products",
      action:"deleteproductimage",
      queryString: `imageId=${ImageId}`
    }, id);
    await firstValueFrom(deleteObservable);
    successCallback();
  }
}









