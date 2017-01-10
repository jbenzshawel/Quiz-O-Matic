import { NgModule }              from '@angular/core';
import { BrowserModule }         from '@angular/platform-browser';
import { FormsModule }           from '@angular/forms';
import { HttpModule }            from '@angular/http';
 
import { AppComponent }          from './app.component';
import { LoginFormComponent }    from './components/login-form.component';

import { AuthenticationService } from './services/authentication.service'; 
import { AuthGuard }             from './app.authguard';

@NgModule({
 imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
   //     routing
    ],
    declarations: [
        AppComponent,
        LoginFormComponent
    ],
    providers: [
        AuthGuard,
        AuthenticationService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
