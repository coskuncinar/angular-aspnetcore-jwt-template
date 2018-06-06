export interface IUser {
    Id : number;
    UserName : string;
    FirstName : string;
    LastName : string;
}

export interface ILoginModel {
    Email : string;
    Password : string;
}

export interface IRegisterModel {
    Email : string;
    Password : string;
    FirstName : string;
    LastName : string;
}

export class User implements IUser {
    constructor(user : IUser) {
        this.Id = user.Id;
        this.UserName = user.UserName;
        this.FirstName = user.FirstName;
        this.LastName = user.LastName;
    }

    Id : number;
    UserName : string;
    FirstName : string;
    LastName : string;
}

export class LoginModel implements ILoginModel {
    constructor(loginModel : ILoginModel) {
        this.Email = loginModel.Email;
        this.Password = loginModel.Password;
    }

    Email : string;
    Password : string;
}

export class RegisterModel implements IRegisterModel {
    constructor(registerModel : IRegisterModel) {
        this.Email = registerModel.Email;
        this.Password = registerModel.Password;
        this.FirstName = registerModel.FirstName;
        this.LastName = registerModel.LastName;
    }

    Email : string;
    Password : string;
    FirstName : string;
    LastName : string;
}
