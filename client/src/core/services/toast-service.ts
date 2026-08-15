import { Service } from '@angular/core';

@Service()
export class ToastService {


    // sto se slucuva stom napravime instanca od ToastService
    // koga ToastService ke bide kreiran, vednas se pravi i containerot
    constructor() {
        this.createToastContainer();
    }


    private createToastContainer() 
    {
        if(!document.getElementById('toast-container'))     // ako ne postoi containerot 
        {                                                   // go pravime dole 
            const container = document.createElement('div')
            container.id = 'toast-container'
            container.className = 'toast toast-bottom toast-end' // daisyUi/tailwind klasata
            document.body.appendChild(container)
        }
    }

    private createToastElement(message: string, alertClass: string, duration = 5000)
    {
        const toastContainer = document.getElementById('toast-container')
        if(!toastContainer) {
            return;
        }
         
        const toast = document.createElement('div')
        toast.classList.add('alert', alertClass, 'shadow-lg')

        toast.innerHTML = `
            <span>${message}</span>
            <button class="btn btn-sm btn-ghost ml-4">x</button>`   
          
        // za gasenje na notifikacijata
        toast.querySelector('button')?.addEventListener('click', () => {
            toastContainer.removeChild(toast)
        })    

        toastContainer.appendChild(toast)

        setTimeout(() => {
            if(toastContainer.contains(toast)) {
                toastContainer.removeChild(toast)
            }
        }, duration)

    }


    successToast(message: string, duration?: number) {
        this.createToastElement(message, 'alert-success', duration)
    }

    errorToast(message: string, duration?: number) {
        this.createToastElement(message, 'alert-error', duration)
    }

    warningToast(message: string, duration?: number) {
        this.createToastElement(message, 'alert-warning', duration)
    }

    infoToast(message: string, duration?: number) {
        this.createToastElement(message, 'alert-info', duration)
    }


}
