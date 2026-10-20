import { HttpEvent, HttpInterceptorFn } from '@angular/common/http';
import { BusyService } from '../services/busy-service';
import { inject } from '@angular/core';
import { delay, finalize, of, tap } from 'rxjs';

const cache = new Map<string, HttpEvent<unknown>>();  

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {

  const busyService = inject(BusyService)

  if(req.method === "GET")
  {
    const cachedResponse = cache.get(req.url) // proverka : ako metodata e get togas proveri vo cache spored req.url
    if(cachedResponse)
    {
      return of(cachedResponse)
    }
  }

  busyService.busy()

  return next(req).pipe(
    delay(300),
    tap(response => {
      cache.set(req.url, response)
    }),
    finalize(() => {
      busyService.idle()
    })
  );
};
