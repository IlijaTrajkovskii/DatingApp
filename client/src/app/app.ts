import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { Nav } from "../layout/nav/nav";

@Component({
  selector: 'app-root',
  imports: [Nav, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

// ova e konstruktor za klasata sto se izvrsuva koga ke se pojavi komponentnata first time 
export class App {
  
  protected router = inject(Router)

}
