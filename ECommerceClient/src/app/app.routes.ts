import { Routes } from '@angular/router';
import {Layout} from './admin/layout/layout';
import {Dashboard} from './admin/components/dashboard/dashboard';
import {Home} from './ui/components/home/home';
import {authGuard} from './guards/common/auth-guard';

export const routes: Routes = [
  {path: "admin", component:Layout, children:[

      {path:"", component:Dashboard,canActivate: [authGuard]}, // Adminin ana sayfası olarak ayarlamak için bu şekilde çağırdık

      {path: "customers",loadChildren : () => import("./admin/components/customers/customers-module")
          .then(module => module.CustomersModule),canActivate: [authGuard]},
      {path: "products",loadChildren : () => import("./admin/components/products/products-module")
          .then(module => module.ProductsModule ),canActivate: [authGuard]},
      {path: "orders",loadChildren : () => import("./admin/components/orders/orders-module")
          .then(module => module.OrdersModule),canActivate: [authGuard]},
    ], canActivate: [authGuard] // admine gelicek isteklerde bu guard devreye girecek
  },

  { path: "", component: Home}, // İlk açılan sayfa

  { path: "basket", loadChildren : () => import("./ui/components/baskets/baskets-module")
      .then(module => module.BasketsModule)},
  { path: "products", loadChildren : () => import("./ui/components/products/products-module")
      .then(module => module.ProductsModule)},
  { path: "register", loadChildren : () => import("./ui/components/register/register-module")
      .then(module => module.RegisterModule)},
  { path: "login", loadChildren : () => import("./ui/components/login/login-module")
      .then(module => module.LoginModule)},
];
