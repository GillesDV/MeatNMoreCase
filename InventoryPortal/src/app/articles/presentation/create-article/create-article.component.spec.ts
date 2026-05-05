import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';

import { ArticleApi } from '../../application/ports/article-api.port';
import { AuthSession } from '../../../auth/application/auth-session.port';
import { CreateArticleComponent } from './create-article.component';

describe('CreateArticleComponent', () => {
  let fixture: ComponentFixture<CreateArticleComponent>;
  let component: CreateArticleComponent;
  let articleApi: jasmine.SpyObj<ArticleApi>;
  let authSession: jasmine.SpyObj<AuthSession>;

  beforeEach(async () => {
    articleApi = jasmine.createSpyObj<ArticleApi>('ArticleApi', [
      'getAll',
      'getById',
      'create',
      'update'
    ]);
    authSession = jasmine.createSpyObj<AuthSession>('AuthSession', [
      'login',
      'logout',
      'getAuthorizationToken'
    ], {
      isAuthenticated$: of(true)
    });
    authSession.getAuthorizationToken.and.returnValue(of('firebase-id-token'));

    await TestBed.configureTestingModule({
      imports: [CreateArticleComponent, NoopAnimationsModule],
      providers: [
        { provide: ArticleApi, useValue: articleApi },
        { provide: AuthSession, useValue: authSession }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CreateArticleComponent);
    component = fixture.componentInstance;
  });

  it('creates an article and resets the form after success', () => {
    articleApi.create.and.returnValue(of({
      articleId: 12,
      description: 'Bolts',
      unit: 'piece'
    }));

    component.form.setValue({ description: 'Bolts', unit: 'piece' });
    component.submit();

    expect(articleApi.create).toHaveBeenCalledOnceWith(
      {
        description: 'Bolts',
        unit: 'piece'
      },
      { accessToken: 'firebase-id-token' }
    );
    expect(component.form.getRawValue()).toEqual({
      description: '',
      unit: 'kilogram'
    });
  });

  it('does not submit an invalid form', () => {
    component.form.setValue({ description: '', unit: 'piece' });
    component.submit();

    expect(articleApi.create).not.toHaveBeenCalled();
  });
});
