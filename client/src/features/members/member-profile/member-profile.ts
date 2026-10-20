import { Component, HostListener, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { EditableMember, Member } from '../../../types/Member';
import { DatePipe } from '@angular/common';
import { MemberService } from '../../../core/services/member-service';
import { ToastService } from '../../../core/services/toast-service';
import { FormsModule, NgForm } from '@angular/forms';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-member-profile',
  imports: [DatePipe, FormsModule],
  templateUrl: './member-profile.html',
  styleUrl: './member-profile.css',
})
export class MemberProfile implements OnInit, OnDestroy{
  
  @ViewChild('editForm') editForm?: NgForm
  
  //prevencija da se izgubat podatoci od formata pri klik na drug website: 
  @HostListener('window:beforeunload', ['$event']) notify($event:BeforeUnloadEvent) {
    if(this.editForm?.dirty)
    {
      $event.preventDefault()
    }
  }

  protected memberService = inject(MemberService);
  private toastService = inject(ToastService);
  private accountService = inject(AccountService)
  
  protected editableMember : EditableMember = {
    displayName: "",
    description: "",
    city: "",
    country: ""
  }
  

// constructor se izvrshuva pred ngOnInit()
  ngOnInit(): void {

    // edit formata da bide popolneta 
    this.editableMember = {
      displayName: this.memberService.member()?.displayName || '',
      description: this.memberService.member()?.description || '',
      city: this.memberService.member()?.city || '',
      country: this.memberService.member()?.country || '',
    }
  }


  ngOnDestroy(): void {
    if(this.memberService.editMode())
    {
      this.memberService.editMode.set(false)
    }
  }

  updateProfile() {
    
    if(!this.memberService.member()) return;
    
    // pravi kombinacija od dvete, i gi overidnuva tie od member() so editableMember(form informaciite)
    const updatedMember = {...this.memberService.member(), ...this.editableMember}
    
    this.memberService.updateMember(this.editableMember).subscribe({
      next: () => {

        // za Navbarot da se update-ne
        const currentUser = this.accountService.currentUser() // ova e User objekt, NE Member !!
        if(currentUser && currentUser.displayName !== updatedMember.displayName)
        {
          this.accountService.setCurrentUser({
            ...currentUser,
            displayName: updatedMember.displayName
          })
        }
        
        this.toastService.successToast("Profile updated succesfully!")
        this.memberService.editMode.set(false);
        this.memberService.member.set(updatedMember as Member) // mnogu vazno !
        this.editForm?.reset(updatedMember) // reset content to updatedMember
        
    
      }
    })
    
    
    
  }
  

  
}
