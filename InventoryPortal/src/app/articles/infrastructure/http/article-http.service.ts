import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import {
  ArticleApi,
  ArticleRequestOptions
} from '../../application/ports/article-api.port';
import { Article, ArticleUpsert } from '../../domain/models/article.model';
import { ARTICLE_API_BASE_URL } from './article-api-base-url.token';

@Injectable()
export class ArticleHttpService implements ArticleApi {
  private readonly articlesUrl: string;

  constructor(
    private readonly http: HttpClient,
    @Inject(ARTICLE_API_BASE_URL) articleApiBaseUrl: string
  ) {
    this.articlesUrl = `${articleApiBaseUrl.replace(/\/$/, '')}/articles`;
  }

  getAll(options?: ArticleRequestOptions): Observable<Article[]> {
    return this.http.get<Article[]>(this.articlesUrl, this.httpOptions(options));
  }

  getById(articleId: number, options?: ArticleRequestOptions): Observable<Article> {
    return this.http.get<Article>(
      `${this.articlesUrl}/${articleId}`,
      this.httpOptions(options)
    );
  }

  create(article: ArticleUpsert, options?: ArticleRequestOptions): Observable<Article> {
    return this.http.post<Article>(this.articlesUrl, article, this.httpOptions(options));
  }

  update(
    articleId: number,
    article: ArticleUpsert,
    options?: ArticleRequestOptions
  ): Observable<void> {
    return this.http.put<void>(
      `${this.articlesUrl}/${articleId}`,
      { ...article, articleId },
      this.httpOptions(options)
    );
  }

  private httpOptions(options?: ArticleRequestOptions): { headers?: HttpHeaders } {
    return options?.accessToken
      ? { headers: new HttpHeaders({ Authorization: `Bearer ${options.accessToken}` }) }
      : {};
  }
}
