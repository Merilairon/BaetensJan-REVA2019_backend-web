import {AuthenticationService} from './authentication.service';
import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';

@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(private authService: AuthenticationService, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    console.debug(state.url);
    console.debug(`Admin Required: ${this.isAdminRequired(state.url)}`);
    console.debug(`Login Required: ${this.isLoggedInPage(state.url)}`);
    console.debug(`NonLogin Required: ${this.isNonLoggedInPage(state.url)}`);
    console.debug(`Logged in: ${this.authService.isLoggedIn$.getValue()}`);
    console.debug(`Is Admin: ${this.authService.isModerator$.getValue()}`);
    if (this.isAdminRequired(state.url)) {
      console.log("administrator page");
      if (this.authService.isLoggedIn$.getValue()) {
        if (this.authService.isModerator$.getValue()) {
          return true;
        } else {
          //unauthorized => redirect to unauthorized
          this.router.navigate(['/login']);
          return false;
        }
      }
      this.authService.redirectUrl = state.url;
      this.router.navigate(['/login']);
      return false;
    } else if (this.isLoggedInPage(state.url)) {
      console.log("logged in page");
      if (this.authService.isLoggedIn$.getValue()) {
        return true
      }
      this.authService.redirectUrl = state.url;
      this.router.navigate(['/login']);
      return false;
    } else if (this.isNonLoggedInPage(state.url)) {
      console.log("not logged in page");
      if (this.authService.isLoggedIn$.getValue()) {
        this.router.navigate(['/home']);
        return false;
      }
      return true;
    } else return false;
  }

  private isLoggedInPage(url): boolean {
    let pages = ['/groepen', "/opdrachten", "/logout"];
    return pages.includes(url);
  }

  private isNonLoggedInPage(url): boolean {
    //Pages logged-in-users aren't allowed to access anymore
    let pages = ['/login', "/invite-request", "/register"];
    return pages.includes(url);

  }

  private isAdminRequired(url): boolean {
    let adminPages = ["/categorieen", "/exposanten", "/beursplan", "/aanvragen", "/vragen", "/upload-csv"];
    return adminPages.includes(url);

  }
}
