import { Observable } from 'rxjs';

export abstract class AuthSession {
  abstract readonly isAuthenticated$: Observable<boolean>;

  abstract login(): void;

  abstract logout(): void;

  abstract getAuthorizationToken(): Observable<string>;
}
