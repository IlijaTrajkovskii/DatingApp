import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Service, signal } from '@angular/core';
import { EditableMember, Member, Photo } from '../../types/Member';
import { environment } from '../../environments/environment';
import { AccountService } from './account-service';
import { tap } from 'rxjs';

@Service()
export class MemberService {

    private http = inject(HttpClient)
    private accountService = inject(AccountService)

    private baseUrl = environment.apiUrl;

    //go stavame na servis za da bide globalno dostapen 
    public editMode = signal(false);

    public member = signal<Member | null>(null);

    getMembers() 
    {
        return this.http.get<Member[]>(this.baseUrl + '/members')
    }

    getMember(id: string)   //                   /api/members/ + {id}
    {
       return this.http.get<Member>(this.baseUrl + '/members/' + id).pipe(
        tap(member => {
            this.member.set(member) // direktno go polnime signalot i e dostapen nasekade preku memberService
        })
       )   
       

    }

    getMemberPhotos(id: string)
    {
        return this.http.get<Photo[]>(this.baseUrl + '/members/' + id + '/photos')
    }

    updateMember(member: EditableMember)
    {
        return this.http.put(this.baseUrl + '/members', member)
    }

    uploadPhoto(file: File)
    {

        const formData = new FormData()
        formData.append("file", file)

        return this.http.post<Photo>(this.baseUrl + '/members/add-photo', formData)

    }

    setMainPhoto(photo: Photo)
    {
        return this.http.put(this.baseUrl + "/members/set-main-photo/" + photo.id, {})
    }

    deletePhoto(photoId: number)
    {
        return this.http.delete(this.baseUrl + '/members/delete-photo/' + photoId);
    }
    
}
