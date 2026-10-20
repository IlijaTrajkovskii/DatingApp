import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs';
import { Member } from '../../../types/Member';
import { AgePipe } from '../../../core/pipes/age-pipe';
import { AccountService } from '../../../core/services/account-service';
import { MemberService } from '../../../core/services/member-service';

@Component({
  selector: 'app-member-detailed',
  imports: [RouterLink, RouterLinkActive, RouterOutlet, AgePipe],
  templateUrl: './member-detailed.html',
  styleUrl: './member-detailed.css',
})
export class MemberDetailed implements OnInit {

  private route = inject(ActivatedRoute); // live route object , snapshot e tekovnata situacija na rutata
  private router = inject(Router);
  private accountService = inject(AccountService)
  protected memberService = inject(MemberService)
  protected title = signal<string | undefined>('Profile');
  

  // ako id-to na userot sto e momentalno logiran e ist
  //  so id-to na userot od route parametarot pri detailed page pokazi Edit
  protected isCurrentUser = computed(() => {
    return this.accountService.currentUser()?.id === this.route.snapshot.paramMap.get('id')
  })


  ngOnInit(): void {
    
    this.title.set(this.route.firstChild?.snapshot?.title) 

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)   // ne gi sakame site eventi
    ).subscribe({
      next: () => {
        this.title.set(this.route.firstChild?.snapshot?.title)
      }
    })
    
  }

 
}
