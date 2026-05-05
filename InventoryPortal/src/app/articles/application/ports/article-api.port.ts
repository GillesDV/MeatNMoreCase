import { Observable } from 'rxjs';

import { Article, ArticleUpsert } from '../../domain/models/article.model';

export interface ArticleRequestOptions {
  accessToken?: string;
}

export abstract class ArticleApi {
  abstract getAll(options?: ArticleRequestOptions): Observable<Article[]>;

  abstract getById(articleId: number, options?: ArticleRequestOptions): Observable<Article>;

  abstract create(article: ArticleUpsert, options?: ArticleRequestOptions): Observable<Article>;

  abstract update(
    articleId: number,
    article: ArticleUpsert,
    options?: ArticleRequestOptions
  ): Observable<void>;
}
