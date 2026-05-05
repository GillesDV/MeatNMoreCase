import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { ArticleApi } from '../../application/ports/article-api.port';
import { ARTICLE_API_BASE_URL } from './article-api-base-url.token';
import { ArticleHttpService } from './article-http.service';

describe('ArticleHttpService', () => {
  let service: ArticleApi;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: ARTICLE_API_BASE_URL, useValue: 'http://localhost:5227' },
        { provide: ArticleApi, useClass: ArticleHttpService }
      ]
    });

    service = TestBed.inject(ArticleApi);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('gets all articles from the Article API', () => {
    service.getAll({ accessToken: 'token' }).subscribe((articles) => {
      expect(articles).toEqual([
        { articleId: 1, description: 'Flour', unit: 'kilogram' }
      ]);
    });

    const request = httpMock.expectOne('http://localhost:5227/articles');
    expect(request.request.method).toBe('GET');
    expect(request.request.headers.get('Authorization')).toBe('Bearer token');
    request.flush([{ articleId: 1, description: 'Flour', unit: 'kilogram' }]);
  });

  it('updates an article by id', () => {
    service.update(7, { description: 'Screws', unit: 'piece' }).subscribe((result) => {
      expect(result).toBeNull();
    });

    const request = httpMock.expectOne('http://localhost:5227/articles/7');
    expect(request.request.method).toBe('PUT');
    expect(request.request.body).toEqual({
      articleId: 7,
      description: 'Screws',
      unit: 'piece'
    });
    request.flush(null);
  });
});
