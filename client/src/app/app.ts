import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Nav } from "../layout/nav/nav";
import { AccountService } from '../core/services/account-service';
import { lastValueFrom } from 'rxjs';
import { Home } from "../features/home/home";
import { User } from '../types/user';

@Component({
  selector: 'app-root',
  imports: [Nav, Home],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

// ova e konstruktor za klasata sto se izvrsuva koga ke se pojavi komponentnata first time 
export class App implements OnInit {
  

  private http = inject(HttpClient)
  private accountService = inject(AccountService)
  protected readonly title = signal('DatingApp');
  protected members = signal<User[]>([]);


  async ngOnInit() {

    this.setCurrentUser(); // prvin treba da postavime user za da izbegneme Flicker vo UI
    this.members.set(await this.getMembers());
  
  }

  setCurrentUser() {
    const userString = localStorage.getItem("user");
    if (!userString) return;
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

  async getMembers() {
    // lastValueFrom ja dava poslednata vrednsot od observable-ot i convertnuva vo Promise
    try 
    { 
      return lastValueFrom(this.http.get<User[]>('https://localhost:5001/api/members'));
    } catch(error) 
    {
      console.log(error)
      throw error;
    }
  }

  
}
