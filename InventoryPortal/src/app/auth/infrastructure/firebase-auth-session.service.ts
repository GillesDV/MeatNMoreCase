import { Injectable, NgZone, inject } from '@angular/core';
import { FirebaseApp, getApp, getApps, initializeApp } from 'firebase/app';
import {
  Auth,
  GoogleAuthProvider,
  User,
  browserLocalPersistence,
  getAuth,
  getRedirectResult,
  onAuthStateChanged,
  setPersistence,
  signInWithPopup,
  signInWithRedirect,
  signOut
} from 'firebase/auth';
import { BehaviorSubject, Observable, from, map, switchMap, take } from 'rxjs';

import { environment } from '../../../environments/environment';
import { AuthSession } from '../application/auth-session.port';

@Injectable()
export class FirebaseAuthSessionService implements AuthSession {
  private readonly zone = inject(NgZone);
  private readonly app: FirebaseApp = getApps().length > 0
    ? getApp()
    : initializeApp(environment.firebaseAuth);
  private readonly auth: Auth = getAuth(this.app);
  private readonly provider = new GoogleAuthProvider();
  private readonly userSubject = new BehaviorSubject<User | null>(this.auth.currentUser);

  readonly isAuthenticated$ = this.userSubject.asObservable().pipe(
    map((user) => user !== null)
  );

  constructor() {
    void this.initAuth();
  }

  private async initAuth(): Promise<void> {
    try {
      await setPersistence(this.auth, browserLocalPersistence);

      const result = await getRedirectResult(this.auth);
      console.log('Redirect result:', result);
    } catch (error) {
      console.error('Redirect login completion failed.', error);
    }

    onAuthStateChanged(this.auth, (user) => {
      console.log('Auth state user:', user);
      this.setUser(user);
    });
  }

  login(): void {
    void signInWithPopup(this.auth, this.provider)
      .then(async result => {
        console.log('Popup user:', result.user);
        console.log('ID token:', await result.user.getIdToken());
        this.setUser(result.user);
      })
      .catch(error => console.error('Popup login failed', error));
  }

  logout(): void {
    void signOut(this.auth).then(() => {
      this.setUser(null);
    });
  }

  getAuthorizationToken(): Observable<string> {
    return this.userSubject.asObservable().pipe(
      take(1),
      switchMap((user) => {
        if (!user) {
          throw new Error('Cannot create an article without a signed-in Firebase user.');
        }

        return from(user.getIdToken());
      })
    );
  }

  private setUser(user: User | null): void {
    this.zone.run(() => {
      this.userSubject.next(user);
    });
  }
}
