import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';

import { CreateArticleComponent } from './articles/presentation/create-article/create-article.component';
import { AuthSession } from './auth/application/auth-session.port';

@Component({
  selector: 'app-root',
  imports: [
    AsyncPipe,
    CreateArticleComponent,
    RouterOutlet,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatIconModule,
    MatListModule,
    MatToolbarModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  private readonly authSession = inject(AuthSession);

  title = 'Inventory Portal';
  readonly isAuthenticated$ = this.authSession.isAuthenticated$;

  readonly updates = [
    'Cycle count scheduled for warehouse A',
    'Price sync completed for active catalog',
    'Stock service reports all integrations healthy'
  ];

  login(): void {
    this.authSession.login();
  }

  logout(): void {
    this.authSession.logout();
  }
}
