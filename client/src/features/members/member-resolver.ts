import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { MemberService } from '../../core/services/member-service';
import { Member } from '../../types/Member';
import { EMPTY } from 'rxjs';

export const memberResolver: ResolveFn<Member> = (route, state) => {

  const memberService = inject(MemberService);
  const router = inject(Router)
  const memberId = route.paramMap.get('id');  // route dava informacii za momentalnata ruta

  if(!memberId)
  {
    router.navigateByUrl("/not-found");
    return EMPTY;
  }

  return memberService.getMember(memberId);
};
