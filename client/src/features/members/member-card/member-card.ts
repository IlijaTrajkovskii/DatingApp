import { Component, input } from '@angular/core';
import { Member } from '../../../types/Member';
import { RouterLink } from '@angular/router';
import { AgePipe } from '../../../core/pipes/age-pipe';

@Component({
  selector: 'app-member-card',
  imports: [RouterLink, AgePipe],
  templateUrl: './member-card.html',
  styleUrl: './member-card.css',
})
export class MemberCard {

  // we use(Input) this to use data from a parent component
  // parent to child communication

  // child to parent communication we use OUTPUT

  member = input.required<Member>(); 


                                 

}
