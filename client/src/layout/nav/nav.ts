import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { LoginCreds } from '../../types/user';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {

  protected creds = {} as LoginCreds //pravime prazen objekt koj sto mu davame atributi vo template-ot 
 
  protected accountService = inject(AccountService);
  protected toastService = inject(ToastService)
  private router = inject(Router);


  login()
  {
    this.accountService.login(this.creds).subscribe({
      next: () => {
        this.router.navigateByUrl("/members")
        this.toastService.successToast("Logged in successfully")
        this.creds = {
          "email":"",
          "password":""
        }
      },
      error: error => {
        this.toastService.errorToast(error.error)

      }
    })
  }

  logout() {
    this.accountService.logout()
    this.toastService.infoToast("Logged out successfully")
    this.router.navigateByUrl("/")
  }

}
