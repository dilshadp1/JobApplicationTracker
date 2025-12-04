import { Component, computed, inject } from '@angular/core';
import { AuthService } from '../../services/auth-services/auth.service';
import {
  ActivatedRoute,
  NavigationEnd,
  Router,
  RouterModule,
  RouterOutlet,
} from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { filter, map } from 'rxjs';
import { HomeComponent } from '../../../features/home/home.component';

@Component({
  selector: 'app-main-layout',
  imports: [RouterModule, RouterOutlet],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss',
})
export class MainLayoutComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);

  isLoggedIn = computed(() => !!this.authService.userData());
  isSidebarToggled = false;
  isAdmin = computed(() => this.authService.userData()?.role === 'Admin');
  userEmail = computed(() => this.authService.userData()?.email || 'User');

  pageTitle = toSignal(
    this.router.events.pipe(
      filter((event) => event instanceof NavigationEnd),
      map(() => {
        let route = this.activatedRoute;
        while (route.firstChild) {
          route = route.firstChild;
        }
        return route.snapshot.title || 'Job Tracker';
      })
    ),
    { initialValue: 'Job Tracker' }
  );

  logout() {
    this.authService.logout();
    this.router.navigate(['']);
  }

  toggleSidebar() {
    this.isSidebarToggled = !this.isSidebarToggled;
  }
}
