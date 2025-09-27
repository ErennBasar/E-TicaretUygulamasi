import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {Layout} from './admin/layout/layout';
import {AdminModule} from './admin/admin-module';
import {UiModule} from './ui/ui-module';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Layout, AdminModule, UiModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('ECommerceClient');
}
