import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthappService } from './authapp.service';

@Injectable({
  providedIn: 'root'
})
export class RouteGuardService implements CanActivate {
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) :boolean {
    if (!this.BasicAuth.isLogged())
    {
      this.route.navigate(["login"]);
      return false;
    }
    else
    {
      return true;
    }
    
  }

  constructor(public BasicAuth : AuthappService, private route:Router) { }
}
