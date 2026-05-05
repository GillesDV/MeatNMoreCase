export type ArticleUnit = 'Unknown' | 'kilogram' | 'piece';

export interface Article {
  articleId: number;
  description: string;
  unit: ArticleUnit;
}

export type ArticleUpsert = Omit<Article, 'articleId'> & {
  articleId?: number;
};
