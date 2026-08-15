import { HttpClient } from '@angular/common/http';
import { inject, Service, signal } from '@angular/core';
import { LoginCreds, RegisterCreds, User } from '../../types/user';
import { tap } from 'rxjs';

@Service()  // our service stays alive through out the whole lifecycle of the APP!
export class AccountService {

    private http = inject(HttpClient);
    baseUrl = "https://localhost:5001/api/";
    
    currentUser = signal<User | null>(null);


    login(creds:LoginCreds)
    {
        return this.http.post<User>(this.baseUrl + 'account/login', creds).pipe(
            tap(user => {
                if(user) {
                    this.setCurrentUser(user); // pomosna funkcija
                }
            })
        )
    }

    register(creds:RegisterCreds) 
    {
        return this.http.post<User>(this.baseUrl + 'account/register', creds).pipe(
            tap(user => {
                if(user) {
                    this.setCurrentUser(user)
                }
            })
        )
            
        
    }

    setCurrentUser(user: User) 
    {                                                       // cross-site scripting
        localStorage.setItem("user", JSON.stringify(user)) // Rizik od XSS attack! NOT SAFE
        this.currentUser.set(user)
    }


    logout()
    {
        localStorage.removeItem("user")
        this.currentUser.set(null)
       
    }


}
