import {Component, Input} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FileSystemFileEntry, NgxFileDropEntry, NgxFileDropModule} from 'ngx-file-drop'; // drag, drop işlevselliği
import {HttpClientService} from '../http-client';
import {HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {AlertifyService, MessageType, Position} from '../../admin/alertify';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../../ui/custom-toastr';
import {DialogService} from '../dialog.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {SpinnerType} from '../../../base/base';
import {FileUploadDialog, FileUploadDialogState} from '../../../dialogs/file-upload-dialog/file-upload-dialog';

@Component({
  selector: 'app-file-upload',
  standalone: true,
  imports: [
    CommonModule,
    NgxFileDropModule
  ],
  templateUrl: './file-upload.html',
  styleUrl: './file-upload.scss'
})
export class FileUpload {

  constructor(
    private httpClientService: HttpClientService,
    private alertifyService: AlertifyService,
    private customToastrService: CustomToastrService,
    private dialogService: DialogService,
    private spinner: NgxSpinnerService
    ) {}

    public files: NgxFileDropEntry[];

    @Input() options:Partial<FileUploadOptions>
    public selectedFiles(files: NgxFileDropEntry[]) {
      console.log('ADIM 1: Dosyalar seçildi, selectedFiles metodu çalıştı.');
      this.files = files;
      const fileData: FormData = new FormData();
      for (const file of files) {
        (file.fileEntry as FileSystemFileEntry).file((_file: File) => {
          fileData.append(_file.name, _file, file.relativePath);
        });
    }
      console.log('ADIM 2: FormData dışarıya fırlatılıyor (emit).');
    this.dialogService.openDialog({
        componentType: FileUploadDialog,
        data: FileUploadDialogState.Yes,
        afterClosed: () => {
          this.spinner.show(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING)
          this.httpClientService.post({
            controller: this.options.controller,
            action: this.options.action,
            queryString: this.options.queryString,
            headers: new HttpHeaders({ "responseType": "blob" })
          }, fileData).subscribe(data => {

          const message: string = "Dosyalar başarıyla yüklenmiştir.";

          this.spinner.hide(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING);
          if (this.options.isAdminPage) {
            this.alertifyService.message(message,
              {
                dismissOthers: true,
                messageType: MessageType.SUCCESS,
                position: Position.TOP_RIGHT
              })
          } else {
            this.customToastrService.message(message, "Başarılı.", {
              messageType: ToastrMessageType.SUCCESS,
              position: ToastrPosition.TOP_RIGHT
            })
          }


        }, (errorResponse: HttpErrorResponse) => {

          const message: string = "Dosyalar yüklenirken beklenmeyen bir hatayla karşılaşılmıştır.";

          this.spinner.hide(SpinnerType.BALL_SPIN_CLOCKWİSE_FADE_ROTATING)
          if (this.options.isAdminPage) {
            this.alertifyService.message(message,
              {
                dismissOthers: true,
                messageType: MessageType.ERROR,
                position: Position.TOP_LEFT
              })
          } else {
            this.customToastrService.message(message, "Başarsız.", {
              messageType: ToastrMessageType.ERROR,
              position: ToastrPosition.TOP_LEFT
            })
          }

        });
      }
    });
  }

  //openDialog(afterClosed: any): void {
  //  const dialogRef = this.dialog.open(FileUploadDialogComponent, {
  //    width: '250px',
  //    data: FileUploadDialogState.Yes,
  //  });

  //  dialogRef.afterClosed().subscribe(result => {
  //    if (result == FileUploadDialogState.Yes)
  //      afterClosed();
  //  });
  //}

}

export class FileUploadOptions {
  controller?: string;
  action?: string;
  queryString?: string;
  explanation?: string;
  accept?: string;
  isAdminPage?: boolean = false;
}
