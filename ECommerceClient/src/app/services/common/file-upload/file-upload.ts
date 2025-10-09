import {Component, Input} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FileSystemFileEntry, NgxFileDropEntry, NgxFileDropModule} from 'ngx-file-drop';
import {HttpClientService} from '../http-client';
import {HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {AlertifyService, MessageType, Position} from '../../admin/alertify';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../../ui/custom-toastr';

@Component({
  selector: 'app-file-upload',
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
    ) {
  }

  public files: NgxFileDropEntry[];

  @Input() options:Partial<FileUploadOptions>
  public selectedFiles(files: NgxFileDropEntry[]) {
    this.files = files;
    const fileData: FormData = new FormData();
    for (const file of files) {
      (file.fileEntry as FileSystemFileEntry).file((_file: File) => {
        fileData.append(_file.name, _file, file.relativePath)
      })
    }

    this.httpClientService.post({
      controller: this.options.controller,
      action: this.options.action,
      queryString: this.options.queryString,
      headers: new HttpHeaders({"responseType": "blob"})
    }, fileData).subscribe(data => {

      const message: string = "Files succesfuly uploaded";

      if(this.options.isAdminPage){
        this.alertifyService.message(message, {
          dismissOthers: true,
          messageType: MessageType.MESSAGE,
          position: Position.BOTTOM_CENTER
        })
      } else {
        this.customToastrService.message(message,"Succes",{
          messageType: ToastrMessageType.SUCCESS,
          position: ToastrPosition.BOTTOM_CENTER
        })
      }

    }, (err: HttpErrorResponse) => {

      const message: string = "Unexpected error occured while file uploading";

      if(this.options.isAdminPage){
        this.alertifyService.message(message, {
          dismissOthers: true,
          messageType: MessageType.MESSAGE,
          position: Position.BOTTOM_CENTER
        })
      } else {
        this.customToastrService.message(message,"Succes",{
          messageType: ToastrMessageType.SUCCESS,
          position: ToastrPosition.BOTTOM_CENTER
        })
      }

    });
  }
}

export class FileUploadOptions {
  controller?: string;
  action?: string;
  queryString?: string;
  explanation?: string;
  accept?: string;
  isAdminPage?: boolean = false;
}
