import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError } from 'rxjs';
import { ToastService } from '../services/toast-service';
import { NavigationExtras, Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  const toast = inject(ToastService);
  const router = inject(Router);

  return next(req).pipe(
    catchError(error => {

      if(error)
      {

        switch (error.status) {

          case 400:

            if(error.error.errors)
            {
              const ModelStateErrors = [];
              
              for(const key in error.error.errors) 
              {
                if(error.error.errors[key]) 
                {
                  ModelStateErrors.push(error.error.errors[key])
                }
              }
              throw ModelStateErrors.flat()

            } else
            {
              toast.errorToast(error.error)
            }
            break;
          
          case 401:
            toast.errorToast("Unauthorized");
            break;

          case 404:
            router.navigateByUrl('/not-found') // tuka ne e vazno sto stavame zosto routes ke aktivira ** pateka 
            break;

          case 500:
            const navigationExtras: NavigationExtras = {state: {error: error.error}}  // zimame router state preku NavExtras
            router.navigateByUrl("/server-error", navigationExtras)
            break;

          default:
            toast.errorToast("Something went wrong!")
            break;
        }

      }

      throw error;

    })
  )
};
