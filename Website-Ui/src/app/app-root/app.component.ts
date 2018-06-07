import {Component, OnInit} from '@angular/core';
import {AuthService} from '../auth.service';
import {LoginModel} from '../Models/UserModel';

@Component({selector: 'app-root', templateUrl: './app.component.html', styleUrls: ['./app.component.css']})
export class AppComponent implements OnInit {
    title = 'app';

    constructor(private auth : AuthService) {}

    ngOnInit() : void {
        this
            .auth
            .login(new LoginModel({Email: 'THE EMAIL', Password: 'PASSWORD'}));
    }

    OnClick() {
        this.auth.get < any > ('http://localhost:58461/api/values').then((data) => {
            console.log(data);
        }, (reason) => {
            console.log(reason);
        });
    }
}
