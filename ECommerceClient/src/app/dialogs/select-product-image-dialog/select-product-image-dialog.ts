import {Component, Inject, OnInit, Output} from '@angular/core';
import {FileUpload, FileUploadOptions} from '../../services/common/file-upload/file-upload';
import {MatButton} from '@angular/material/button';
import {BaseDialog} from '../base/base-dialog';
import {ProductService} from '../../services/common/models/product';
import {NgxSpinnerService} from 'ngx-spinner';
import {DialogService} from '../../services/common/dialog.service';
import {
  MAT_DIALOG_DATA,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle
} from '@angular/material/dialog';

@Component({
  selector: 'app-select-product-image-dialog',
  standalone: true,
  imports: [
    MatDialogClose,
    MatDialogActions,
    MatDialogContent,
    FileUpload,
    MatButton,
    MatDialogTitle

  ],
  templateUrl: './select-product-image-dialog.html',
  styleUrl: './select-product-image-dialog.scss'
})
export class SelectProductImageDialog extends BaseDialog<SelectProductImageDialog> implements OnInit{

  options: Partial<FileUploadOptions>;
  constructor(dialogRef: MatDialogRef<SelectProductImageDialog>,
    @Inject(MAT_DIALOG_DATA) public data: SelectProductImageState | string,
    private productService: ProductService,
    private spinner: NgxSpinnerService,
    private dialogService: DialogService) {
    super(dialogRef)
}
  ngOnInit(): void {
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

}
export enum SelectProductImageState{
  Close
}
