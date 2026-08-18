import { Component, inject, signal } from '@angular/core';
import { ApiError } from '../../../types/ApiError';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  imports: [],
  templateUrl: './server-error.html',
  styleUrl: './server-error.css',
})
export class ServerError {
  
  private router = inject(Router)
  protected error: ApiError;
  protected showDetails = signal<boolean>(false);

  // pristap do router state imame vo construktorot na komponentata preku NavigationExtras
  constructor()
  {
    const navigation = this.router.currentNavigation(); // go ziame celiot navigation objekt so state-ot
    this.error = navigation?.extras?.state?.['error']
  }

  detailsToggle()
  {
    this.showDetails.set(!this.showDetails()) // obratnoto 
  }

}
