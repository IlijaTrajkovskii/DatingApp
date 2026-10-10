import { Component, inject } from '@angular/core';
import { MemberService } from '../../../core/services/member-service';
import { Observable } from 'rxjs';
import { Member } from '../../../types/Member';
import { AsyncPipe } from '@angular/common';
import { MemberCard } from '../member-card/member-card';

@Component({
  selector: 'app-member-list',
  imports: [AsyncPipe,MemberCard],
  templateUrl: './member-list.html',
  styleUrl: './member-list.css',
})
  // 1a AsyncPipe koristime koga bi sakale da prikazeme podatoci vo HTML template i automatski unsubscribe
  // 1b subscribe() koga eksplicitno kazuvame sto pravime so responsot next,error,complete.. vo TypeScript!
  // 2a eksplicitno kazuvame deka members$ e observable pa potoa go inicijalizirame vo konstruktor
  // 2b ILI mozes i vaka direktno --> members$ = this.memberService.getMembers() bez konstruktor
export class MemberList {  

  private memberService = inject(MemberService);
  
  members$: Observable<Member[]>;

  constructor()
  {
    this.members$ = this.memberService.getMembers();
  }

 


}
