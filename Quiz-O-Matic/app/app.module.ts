import { NgModule }              from '@angular/core';
import { BrowserModule }         from '@angular/platform-browser';
import { FormsModule }           from '@angular/forms';
import { HttpModule }            from '@angular/http';
import { MaterialModule } from '@angular/material';
 
import { AppComponent }          from './app.component';
import { LoginRegisterFormComponent }    from './components/login-register-form.component';

import { AuthenticationService } from './services/authentication.service'; 
import { AuthGuard }             from './app.authguard';

import 'hammerjs';

@NgModule({
 imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        MaterialModule.forRoot()
   //     routing
    ],
    declarations: [
        AppComponent,
        LoginRegisterFormComponent
    ],
    providers: [
        AuthGuard,
        AuthenticationService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
