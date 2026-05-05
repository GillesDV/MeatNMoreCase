import { Provider } from '@angular/core';

import { AuthSession } from './application/auth-session.port';
import { FirebaseAuthSessionService } from './infrastructure/firebase-auth-session.service';

export const provideAuthSession = (): Provider[] => [
  {
    provide: AuthSession,
    useClass: FirebaseAuthSessionService
  }
];
