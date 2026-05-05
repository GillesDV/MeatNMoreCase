import { Provider } from '@angular/core';

import { environment } from '../../environments/environment';
import { ArticleApi } from './application/ports/article-api.port';
import { ARTICLE_API_BASE_URL } from './infrastructure/http/article-api-base-url.token';
import { ArticleHttpService } from './infrastructure/http/article-http.service';

export const provideArticles = (): Provider[] => [
  {
    provide: ARTICLE_API_BASE_URL,
    useValue: environment.articleApiBaseUrl
  },
  {
    provide: ArticleApi,
    useClass: ArticleHttpService
  }
];
