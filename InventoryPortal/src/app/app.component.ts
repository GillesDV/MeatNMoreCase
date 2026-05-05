import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';

import { CreateArticleComponent } from './articles/presentation/create-article/create-article.component';

@Component({
  selector: 'app-root',
  imports: [
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
  title = 'Inventory Portal';

  readonly updates = [
    'Cycle count scheduled for warehouse A',
    'Price sync completed for active catalog',
    'Stock service reports all integrations healthy'
  ];
}
