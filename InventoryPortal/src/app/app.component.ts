import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [
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

  readonly metrics = [
    { label: 'Products tracked', value: '1,284', icon: 'inventory_2' },
    { label: 'Low stock items', value: '18', icon: 'warning' },
    { label: 'Inbound orders', value: '42', icon: 'local_shipping' }
  ];

  readonly updates = [
    'Cycle count scheduled for warehouse A',
    'Price sync completed for active catalog',
    'Stock service reports all integrations healthy'
  ];
}
