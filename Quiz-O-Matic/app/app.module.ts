import { NgModule }              from '@angular/core';
import { BrowserModule }         from '@angular/platform-browser';
import { FormsModule }           from '@angular/forms';
import { HttpModule }            from '@angular/http';
import { MaterialModule } from '@angular/material';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent }          from './app.component';
import { HomeComponent }         from './components/home.component';
import { LoginRegisterFormComponent }    from './components/login-register-form.component';
import { DashboardComponent } from './components/dashboard.component';
import { NavigationComponent } from './components/navigation.component';
import { AuthenticationService } from './services/authentication.service'; 
import { AuthGuard }             from './app.authguard';
import { AppRoutingModule } from './app-routing.module';

import 'hammerjs';


@NgModule({
 imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        MaterialModule.forRoot(),
        AppRoutingModule
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        NavigationComponent,
        DashboardComponent,
        LoginRegisterFormComponent
    ],
    providers: [
        AuthGuard,
        AuthenticationService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
