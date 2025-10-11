import {Injectable} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {Observable} from 'rxjs';
import {DeleteDialog} from '../../dialogs/delete-dialog/delete-dialog';
import {FileUploadDialog} from '../../dialogs/file-upload-dialog/file-upload-dialog';

//Yeni dialog eklemek için DialogType enum'a ekle
//openDialog() switch'ine case ekle
//İsteğe bağlı özel metod ekle ( örn: openMyDialog() )
export enum DialogType {
  Delete = 'delete',
  FileUpload = 'fileUpload'
}

export interface DialogOptions {
  width?: string;
  height?: string;
  enterAnimationDuration?: string;
  exitAnimationDuration?: string;
  data?: any;
}

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) {
  }

  openDialog(dialogType: DialogType, options?: DialogOptions): Observable<any> {
    const defaultOptions: DialogOptions = {
      width: '350px',
      enterAnimationDuration: '300ms',
      exitAnimationDuration: '300ms',
      ...options
    };

    let dialogComponent: any;

    switch (dialogType) {
      case DialogType.Delete:
        dialogComponent = DeleteDialog;
        defaultOptions.width = '250px';
        break;
      case DialogType.FileUpload:
        dialogComponent = FileUploadDialog;
        break;
      default:
        throw new Error(`Unknown dialog type: ${dialogType}`);
    }

    const dialogRef = this.dialog.open(dialogComponent, defaultOptions);
    return dialogRef.afterClosed();
  }

  openDeleteDialog(options?: DialogOptions): Observable<boolean> {
    return this.openDialog(DialogType.Delete, options);
  }

  openFileUploadDialog(options?: DialogOptions): Observable<boolean> {
    return this.openDialog(DialogType.FileUpload, options);
  }
}
