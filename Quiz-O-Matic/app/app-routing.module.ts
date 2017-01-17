import { NgModule }              from '@angular/core';
import { RouterModule, Routes }  from '@angular/router';
import { LoginRegisterFormComponent }    from './components/login-register-form.component';
import { AppComponent }          from './app.component';
import { DashboardComponent } from './components/dashboard.component';
import { QuizesComponent } from './components/quizes.component';
import { HomeComponent } from './components/home.component';
import { AuthenticationService } from './services/authentication.service'; 
import { AuthGuard }             from './app.authguard';

const appRoutes: Routes = [
    { path: 'app',   component: AppComponent },
    { path: 'home', component: HomeComponent },    
    { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
    { path: 'quizes', component: QuizesComponent, canActivate: [AuthGuard]},
    { 
        path: '',
        redirectTo: '/home',
        pathMatch: 'full'
    },
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
    //,{ path: '**', component: PageNotFoundComponent }
];
@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {}
