export interface IToken {
    tokenHash : string;
    expiresAt : Date;
}

export interface IRefreshTokenModel {
    tokenHash : string;
}

export interface ITokenResponse {
    id : number;
    access_token : IToken;
    refresh_token : IToken;
}

export class Token implements IToken {
    constructor(token : IToken) {
        this.tokenHash = token.tokenHash;
        this.expiresAt = token.expiresAt;
    }

    tokenHash : string;
    expiresAt : Date;
}

export class RefreshTokenModel implements IRefreshTokenModel {
    constructor(refreshToken : IRefreshTokenModel) {
        this.tokenHash = refreshToken.tokenHash;
    }

    tokenHash : string;
}

export class TokenResponse implements ITokenResponse {
    constructor(loginResponse : ITokenResponse) {
        this.id = loginResponse.id;
        this.access_token = loginResponse.access_token;
        this.refresh_token = loginResponse.refresh_token;
    }

    id : number;
    access_token : IToken;
    refresh_token : IToken;
}
