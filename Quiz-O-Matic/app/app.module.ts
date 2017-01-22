// @angular
import { NgModule }              from '@angular/core';
import { BrowserModule }         from '@angular/platform-browser';
import { FormsModule }           from '@angular/forms';
import { HttpModule }            from '@angular/http';
import { MaterialModule } from '@angular/material';
import { RouterModule, Routes } from '@angular/router';
// components
import { AppComponent }          from './app.component';
import { HomeComponent }         from './components/home.component';
import { LoginRegisterFormComponent }    from './components/login-register-form.component';
import { DashboardComponent } from './components/dashboard.component';
import { NavigationComponent } from './components/navigation.component';
import { QuizesComponent } from './components/quizes.component';
import { TakeQuizComponent } from './components/take-quiz.component';
// services 
import { AuthenticationService } from './services/authentication.service'; 
import { DataService } from './services/data.service'; 
// guards and modules
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
        QuizesComponent,
        TakeQuizComponent,        
        LoginRegisterFormComponent
    ],
    providers: [
        AuthGuard,
        AuthenticationService,
        DataService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
