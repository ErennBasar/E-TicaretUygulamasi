import {Component, Inject, OnInit, Output} from '@angular/core';
import {FileUpload, FileUploadOptions} from '../../services/common/file-upload/file-upload';
import {BaseDialog} from '../base/base-dialog';
import {MatCard, MatCardModule} from '@angular/material/card';
import {MatDialogModule, MAT_DIALOG_DATA, MatDialogRef,} from '@angular/material/dialog';
import {MatButton} from '@angular/material/button';
import {CommonModule} from '@angular/common';
import {ProductService} from '../../services/common/models/product';
import {List_Product_Image} from '../../contracts/list_product_image';
import {NgxSpinnerService} from 'ngx-spinner';
import {SpinnerType} from '../../base/base';
import {DialogService} from '../../services/common/dialog.service';
import {DeleteDialog, DeleteState} from '../delete-dialog/delete-dialog';
declare var $: any;

@Component({
  selector: 'app-select-product-image-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatCardModule,
    MatButton,
    FileUpload,
  ],
  templateUrl: './select-product-image-dialog.html',
  styleUrl: './select-product-image-dialog.scss'
})
export class SelectProductImageDialog extends BaseDialog<SelectProductImageDialog> implements OnInit{

  options: Partial<FileUploadOptions>;
  constructor(
    dialogRef: MatDialogRef<SelectProductImageDialog>,
    @Inject(MAT_DIALOG_DATA) public data: SelectProductImageState | string,
    private productService: ProductService,
    private spinner: NgxSpinnerService,
    private dialogService: DialogService,
  ) {
    super(dialogRef);
  }

  images: List_Product_Image[];
  async ngOnInit(){

    this.spinner.show(SpinnerType.PACMAN);
    this.images = await this.productService.readImages(this.data as string, ()=> this.spinner.hide(SpinnerType.PACMAN));
    // 5. Değer atamasını burada yap.
    // Bu noktada `this.data`'nın değeri bellidir.
    this.options = {
      accept: ".png, .jpg, .jpeg, .gif",
      action: "upload",
      controller: "products",
      explanation: "Ürün resimini seçin veya buraya sürükleyin...",
      isAdminPage: true,
      queryString: `id=${this.data}`,
    };
  }

  async deleteImage(imageId:string, event: any){

    this.dialogService.openDialog({
      componentType: DeleteDialog,
      data: DeleteState.Yes,
      afterClosed:async () =>{
        this.spinner.show(SpinnerType.PACMAN);
        await this.productService.deleteImage(this.data as string, imageId, () => {
          this.spinner.hide(SpinnerType.PACMAN);
          var card = $(event.srcElement).parent().parent().parent();
          card.fadeOut(350);
        });
      } //afterClosed
    }) //openDialog
  } //deleteImage
}
export enum SelectProductImageState{
  Close
}
