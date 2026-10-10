import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { LoginCreds } from '../../types/user';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';
import { themes } from '../theme';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav implements OnInit {

  protected creds = {} as LoginCreds //pravime prazen objekt koj sto mu davame atributi vo template-ot 
 
  protected accountService = inject(AccountService);
  protected toastService = inject(ToastService)
  private router = inject(Router);

  protected selectedTheme = signal<string>(localStorage.getItem('theme') || 'light')
  protected themes = themes;

  ngOnInit(): void {
    document.documentElement.setAttribute('data-theme', this.selectedTheme())
  }


  handleSelectTheme(theme: string)
  {
    this.selectedTheme.set(theme)
    localStorage.setItem('theme', theme) // za da pamti pri refresh
    document.documentElement.setAttribute('data-theme', theme)

    // pri izbor na theme da se zatvori prozorecot
    const closeWindow = document.activeElement as HTMLDivElement   // elementot sto ima FOKUS 
    if(closeWindow) closeWindow.blur();
  }


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
