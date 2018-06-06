import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppComponent} from './app-root/app.component';
import {AuthService} from './auth.service';
import {HttpModule} from '@angular/http';
import {HttpClientModule} from '@angular/common/http';
import {JwtModule} from '@auth0/angular-jwt';

export function tokenGetter() {
    return localStorage.getItem('access_token');
}

@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule, HttpModule, HttpClientModule, JwtModule.forRoot({
            config: {
                tokenGetter: tokenGetter
            }
        })
    ],
    providers: [AuthService],
    bootstrap: [AppComponent]
})
export class AppModule {}
