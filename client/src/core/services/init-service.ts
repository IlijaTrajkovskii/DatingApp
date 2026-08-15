import { inject, Service } from '@angular/core';
import { AccountService } from './account-service';
import { of } from 'rxjs';

@Service()
export class InitService {

    // obezbeduvame pri inicijalizija na aplikacijata da imame currentUser
    private accountService = inject(AccountService)

    init() {

        const userString = localStorage.getItem("user");
        if (!userString) return of(null);
        const user = JSON.parse(userString);

        this.accountService.currentUser.set(user);

        return of(null);
    }
}
