import { Component } from '@angular/core';
import {Header} from './components/header/header';
import {Sidebar} from './components/sidebar/sidebar';
import {Footer} from './components/footer/footer';
import {RouterOutlet} from '@angular/router';

@Component({
  selector: 'app-layout',
  imports: [
    Header,
    Sidebar,
    Footer,
    RouterOutlet
  ],
  templateUrl: './layout.html',
  styleUrl: './layout.scss'
})
export class Layout {

}
