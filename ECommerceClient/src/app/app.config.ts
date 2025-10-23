import {
  ApplicationConfig,
  importProvidersFrom,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import {provideAnimations} from '@angular/platform-browser/animations';
import {provideToastr} from 'ngx-toastr';
import {provideHttpClient, withInterceptorsFromDi} from '@angular/common/http';
import {JwtModule} from '@auth0/angular-jwt';
//import {HttpClientModule} from '@angular/common/http';

// 2. Token getter fonksiyonunu (AOT derlemesi için) dışarı aldık
export function tokenGetter() {
  return localStorage.getItem("accessToken");
}
export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(withEventReplay()),
    provideAnimations(),
    provideToastr(),

    // HttpClient'ı bu şekilde, köprü fonksiyonuyla birlikte sağladım
    //Çünkü withInterceptorsFromDi() olmadan jwt unauthorized hatası veriyordu. (HttpClientModule'de kullanılabilir ama eski bir yöntem)
    provideHttpClient(withInterceptorsFromDi()),

    { provide: 'baseUrl', useValue: 'http://localhost:5013/api', multi: true },

    importProvidersFrom(
      //HttpClientModule, <-- Eski yöntem
      JwtModule.forRoot({
        config: {
          tokenGetter: tokenGetter, // Dışarıdaki fonksiyonu kullan
          allowedDomains: ["localhost:5013"]
        }
      })
    )
  ]
};
