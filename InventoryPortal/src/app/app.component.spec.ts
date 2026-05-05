import { TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';

import { ArticleApi } from './articles/application/ports/article-api.port';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  beforeEach(async () => {
    const articleApi = jasmine.createSpyObj<ArticleApi>('ArticleApi', [
      'getAll',
      'getById',
      'create',
      'update'
    ]);
    articleApi.create.and.returnValue(of({
      articleId: 1,
      description: 'Test article',
      unit: 'piece'
    }));

    await TestBed.configureTestingModule({
      imports: [AppComponent, NoopAnimationsModule],
      providers: [
        { provide: ArticleApi, useValue: articleApi }
      ]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have the 'Inventory Portal' title`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('Inventory Portal');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('mat-toolbar')?.textContent).toContain('Inventory Portal');
  });
});
