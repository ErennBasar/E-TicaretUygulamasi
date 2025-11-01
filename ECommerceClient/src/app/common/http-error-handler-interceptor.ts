import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpStatusCode} from '@angular/common/http';
import {catchError, Observable, of} from "rxjs";
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../services/ui/custom-toastr';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlerInterceptorService implements HttpInterceptor {

  constructor(private toastrService: CustomToastrService) {
  }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> { // isteklerde araya girdiğimizde bu fonksiyon tetiklenicek
    return next.handle(req).pipe(catchError(error => {
      switch (error.status) {
        case HttpStatusCode.Unauthorized:
          this.toastrService.message("Bu işlemi yapmaya yetkiniz bulunmamaktadır", "Yetkisiz İşlem",{
            messageType: ToastrMessageType.WARNING,
            position: ToastrPosition.BOTTOM_FULL_WIDTH
          });
          break;
        case HttpStatusCode.InternalServerError:
          this.toastrService.message("Sunucuya erişilemiyor", "Sunucu Hatası",{
            messageType: ToastrMessageType.WARNING,
            position: ToastrPosition.BOTTOM_FULL_WIDTH
          });
          break;
        case HttpStatusCode.BadRequest:
          this.toastrService.message("Geçersiz istek yapıldı", "Geçersiz İşlem",{
            messageType: ToastrMessageType.WARNING,
            position: ToastrPosition.BOTTOM_FULL_WIDTH
          });
          break;
        case HttpStatusCode.NotFound:
          this.toastrService.message("Sayfa bulunamadı", "Sayfa Bulunamadı",{
            messageType: ToastrMessageType.WARNING,
            position: ToastrPosition.BOTTOM_FULL_WIDTH
          });
          break;
        default:
          this.toastrService.message("Beklenmeyen bir hata meydana gelmiştir", "Beklenmedik Hata",{
            messageType: ToastrMessageType.WARNING,
            position: ToastrPosition.BOTTOM_FULL_WIDTH
          });
          break;
          }
          return of (error);
    }));
  }
}
