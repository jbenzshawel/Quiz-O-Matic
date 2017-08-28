// @angular 
import { NgModule }                   from '@angular/core';
import { RouterModule, Routes }       from '@angular/router';
// components 
import { LoginRegisterFormComponent } from './components/login-register-form.component';
import { AppComponent }               from './app.component';
import { DashboardComponent }         from './components/dashboard.component';
import { QuizesComponent }            from './components/quizes.component';
import { HomeComponent }              from './components/home.component';
import { QuizListComponent }          from './components/quiz-list.component';
import { TakeQuizComponent }          from './components/take-quiz.component';
import { QuizResultComponent }        from './components/quiz-result.component';
// guards
import { AuthGuard }                  from './app.authguard';

const appRoutes: Routes = [
    { path: 'app',   component: AppComponent },
    { path: 'home', component: HomeComponent },    
    { path: 'quiz/list', component: QuizListComponent },
    { path: 'take-quiz/:id', component: TakeQuizComponent },
    { path: 'quiz-result/:id', component: QuizResultComponent },
    { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
    { path: 'quizes', component: QuizesComponent, canActivate: [AuthGuard]},
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    // otherwise redirect to home
    // ToDo: Add PageNotFoundComponent
    { path: '**', redirectTo: '/home' }
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
