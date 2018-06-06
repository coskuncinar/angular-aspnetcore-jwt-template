import {Injectable} from '@angular/core';
import {ILoginModel, IRegisterModel, IUser} from './Models/UserModel';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { ITokenResponse, RefreshTokenModel, IRefreshTokenModel } from './Models/JwtModel';
import {JwtHelperService} from '@auth0/angular-jwt';
import {environment} from '../environments/environment';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';

@Injectable({providedIn: 'root'})

export class AuthService {

    private url = environment.api_url;

    constructor(private http : HttpClient, private jwtHelper : JwtHelperService) {}

    public register(registerModel : IRegisterModel) : Promise < IUser > {
        const header = this.getBasicHeader();

        return this.post < IUser > (this.url + 'auth/register', registerModel, header);
    }

    public login(loginModel : ILoginModel) : Promise < number > {
        const header = this.getBasicHeader();

        return this.post < ITokenResponse > (this.url + 'auth/login', loginModel, header).then((data : ITokenResponse) => {
            this.setTokens(data.access_token.tokenHash, data.refresh_token.tokenHash);
            return data.id;
        });
    }

    public refresh() : Promise< ITokenResponse > {
        const header = this.getBasicHeader();
        const body = new RefreshTokenModel({ tokenHash : this.getRefreshToken() } as IRefreshTokenModel);

        return this.post< ITokenResponse >(this.url + 'auth/refresh', body, header);
    }

    public logout() : boolean {
        this.clearTokens();

        return true;
    }

    public get < T > (path : string, body? : string, headers? : HttpHeaders) : Promise< T > {
        let header : HttpHeaders;

        if (headers == null) {
            header = this.getBasicHeader();
            header = this.setAuthHeader(header);

            if (this.tokenHasExpired()) {
                this.refresh().then((data : ITokenResponse) => {
                    this.setTokens(data.access_token.tokenHash, data.refresh_token.tokenHash);
                });
            }
        } else {
            header = headers;
        }

        if (body == null) {
            return this
            .http
            .get<T>(path, { headers : header })
            .toPromise();

        } else {
            return this
            .http
            .get<T>(path + body, { headers : header })
            .toPromise();
        }
    }

    public post < T > (path : string, body : any, headers? : HttpHeaders) : Promise < T > {
        let header : HttpHeaders;

        if (headers == null) {
            header = this.getBasicHeader();
            header = this.setAuthHeader(header);

            if (this.tokenHasExpired()) {
                this.refresh().then((data : ITokenResponse) => {
                    this.setTokens(data.access_token.tokenHash, data.refresh_token.tokenHash);
                });
            }
        } else {
            header = headers;
        }

        return this
            .http
            .post<T>(path, body, {headers: headers}).toPromise();
    }

    public put < T > (path : string, body : any, headers? : HttpHeaders) : Promise < T > {
        let header : HttpHeaders;

        if (headers == null) {
            header = this.getBasicHeader();
            header = this.setAuthHeader(header);

            if (this.tokenHasExpired()) {
                this.refresh().then((data : ITokenResponse) => {
                    this.setTokens(data.access_token.tokenHash, data.refresh_token.tokenHash);
                });
            }
        } else {
            header = headers;
        }

        return this
            .http
            .put<T>(path, body, {headers: headers})
            .toPromise();
    }

    public delete < T > (path : string, body? : string, headers? : HttpHeaders) : Promise< T > {
        let header : HttpHeaders;

        if (headers == null) {
            header = this.getBasicHeader();
            header = this.setAuthHeader(header);

            if (this.tokenHasExpired()) {
                this.refresh().then((data : ITokenResponse) => {
                    this.setTokens(data.access_token.tokenHash, data.refresh_token.tokenHash);
                });
            }
        } else {
            header = headers;
        }

        if (body === null) {
            return this
            .http
            .delete<T>(path, { headers : headers })
            .toPromise();

        } else {
            return this
            .http
            .delete<T>(path + body, { headers : headers })
            .toPromise();
        }
    }

    private getBasicHeader() : HttpHeaders {
        let header = new HttpHeaders();
        header = header.append('Content-Type', 'application/json');

        return header;
    }

    private setAuthHeader(header : HttpHeaders) : HttpHeaders {
        const token = this.getAccessToken();
        header = header.append('Authorization', `Bearer ${token}`);

        return header;
    }

    private tokenHasExpired() : boolean {
        const date = new Date();
        const token = this.getAccessToken();
        const expiry = this
            .jwtHelper
            .getTokenExpirationDate(token);

        const accessTokenDate = new Date(expiry);
        accessTokenDate.setSeconds(accessTokenDate.getSeconds() - 2);

        if (date >= accessTokenDate) {
            console.log('Token Expired');
            return true;
        }

        return false;
    }

    private getAccessToken() : string {
        const access_token = window.localStorage.getItem('access_token');

        return access_token;
    }

    private getRefreshToken() : string {
        const refresh_token = window.localStorage.getItem('refresh_token');

        return refresh_token;
    }

    private setTokens(access_token : string, refresh_token : string) {
        window.localStorage.setItem('access_token', access_token);
        window.localStorage.setItem('refresh_token', refresh_token);

        console.log('Tokens set');
    }

    private clearTokens() {
        window.localStorage.removeItem('access_token');
        window.localStorage.removeItem('refresh_token');

        console.log('Tokens cleared');
    }
}
