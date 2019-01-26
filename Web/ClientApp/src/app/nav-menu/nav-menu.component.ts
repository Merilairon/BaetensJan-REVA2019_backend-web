import {Component} from '@angular/core';
import {Observable} from "rxjs";
import {AuthenticationService} from "../user/authentication.service";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  /**
   * @ignore
   */
  isExpanded = false;

  constructor(private authService: AuthenticationService) {}

  /**
   * Click event to collapse navbar menu
   */
  collapse() {
    this.isExpanded = false;
  }

  /**
   * Click event to toggle navbar menu
   */
  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  get user(): Observable<string> {
    return this.authService.user$;
  }

  get isAdmin(): Observable<boolean> {
    return this.authService.isModerator$;
  }

}