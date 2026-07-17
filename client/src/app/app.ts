import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports:[],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

// ova e konstruktor za klasata sto se izvrsuva koga ke se pojavi komponentnata first time 
export class App implements OnInit {
  

  private http = inject(HttpClient)

  protected readonly title = signal('DatingApp');

  protected name = "Ilija"

  protected age:number = 24;

  protected members = signal<any>([]);

  protected selectedMember = signal<any>(null); //sekogas signali vo template gi pristapuvame so ()


  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/members').subscribe({
      next: response => this.members.set(response),
      error: error => console.log(error),
      complete: () => console.log("Completed the http getMembers request") 
    })
  }

  getMember(id:string) {
    this.http.get(`https://localhost:5001/api/members/${id}`).subscribe({
      next: member => this.selectedMember.set(member),
      error: error => console.log(error),
      complete: () => console.log("Succesfully fetched member")
    })
  }

}
