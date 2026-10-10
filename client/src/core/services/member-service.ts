import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Member, Photo } from '../../types/Member';
import { environment } from '../../environments/environment';
import { AccountService } from './account-service';

@Service()
export class MemberService {

    private http = inject(HttpClient)
    private accountService = inject(AccountService)

    private baseUrl = environment.apiUrl;

    // potrebno e da go ispratime jwt-to kako header 

    getMembers() 
    {
        return this.http.get<Member[]>(this.baseUrl + '/members')
    }

    getMember(id: string)   //                   /api/members/ + {id}
    {
       return this.http.get<Member>(this.baseUrl + '/members/' + id)
    }

    getMemberPhotos(id: string)
    {
        return this.http.get<Photo[]>(this.baseUrl + '/members/' + id + '/photos')
    }
    
}
