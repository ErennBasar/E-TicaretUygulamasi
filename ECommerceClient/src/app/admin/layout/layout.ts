import {Component, OnInit} from '@angular/core';
import {Header} from './components/header/header';
import {Sidebar} from './components/sidebar/sidebar';
import {Footer} from './components/footer/footer';
import {RouterOutlet} from '@angular/router';
import {MatSidenavModule} from '@angular/material/sidenav';

@Component({
  selector: 'app-layout',
  imports: [
    Header,
    Sidebar,
    Footer,
    RouterOutlet,
    MatSidenavModule
  ],
  templateUrl: './layout.html',
  styleUrl: './layout.scss'
})
export class Layout implements OnInit{

  constructor() {}
  ngOnInit(): void {

  }
}
