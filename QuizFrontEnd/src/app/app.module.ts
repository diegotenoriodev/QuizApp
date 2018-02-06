import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';

import { MatTableModule, MatNativeDateModule } from '@angular/material';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule, MatSelectModule } from '@angular/material';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatRadioModule } from '@angular/material/radio';

import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';

import { MultipleChoiceComponent } from './answertype/multiplechoice.component';
import { TrueFalseOptionComponent } from './answertype/truefalse.component';
import { AnswerDirective } from './answertype/baseanswer.component';
import { QuizesComponent } from './quizzes/quizes.component';
import { QuestionsComponent } from './questions/questions.component';
import { QuizComponent } from './quizzes/quiz.component';
import { PublishComponent } from './answers/publish.component';
import { AnswerComponent } from './answers/quiz/answer.component';
import { GuestComponent } from './guest.component';
import { LoginComponent } from './user/login.component';
import { MainComponent } from './main.component';
import { RegisterComponent } from './user/register.component';
import { QuestionComponent } from './questions/question.component';
import { ErrorsComponent } from './sharedcontrols/errors.component';
import { TopComponent } from './sharedcontrols/top.component';
import { UserGridComponent } from './sharedcontrols/usergrid.component';
import { LoadingComponent } from './sharedcontrols/loading.component';
import { TrueOrFalseAnswerComponent } from './answers/quiz/controls/trueorfalseanswer.component';
import { MultipleChoiceAnswerComponent } from './answers/quiz/controls/multiplechoiceanswer.component';
import { OpenEndedAnswerComponent } from './answers/quiz/controls/openended.component';
import { APIService } from './services/api.service';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './services/auth.interceptor';
import { AnswersReportComponent } from './answers/report/answers.component';
import { AnswerReportComponent } from './answers/report/answer.component';
import { QuizUserComponent } from './answers/quiz/quizesUser.component';
import { AnswersUserComponent } from './answers/report/answerUser.component';
import { OpenEndedReportComponent } from './answers/report/controls/openended.component';
import { TrueOrFalseReportComponent } from './answers/report/controls/trueorfalseanswer.component';
import { MultipleChoiceReportComponent } from './answers/report/controls/multiplechoiceanswer.component';
import { PasswordComponent } from './user/password.component';

const routes = [
  { path: 'quizes', component: QuizesComponent},
  { path: 'questions', component: QuestionsComponent},
  { path: 'questions/:quizId', component: QuestionsComponent},
  { path: 'account', component: PasswordComponent},
  { path: 'publish/:quizId', component: PublishComponent},
  { path: 'user/quizes', component: QuizUserComponent},
  { path: 'user/quizes/:pubId', component: AnswerComponent},
  { path: 'user/answers', component: AnswersUserComponent},
  { path: 'user/answers/:pubId', component: AnswersReportComponent}
];

@NgModule({
  declarations: [
    AppComponent,
    GuestComponent,
    LoginComponent,
    MainComponent,
    RegisterComponent,
    QuizesComponent,
    QuizComponent,
    QuestionsComponent,
    QuestionComponent,
    QuizUserComponent,
    AnswerComponent,
    ErrorsComponent,
    TopComponent,
    MultipleChoiceComponent,
    TrueFalseOptionComponent,
    UserGridComponent,
    LoadingComponent,
    AnswerDirective,
    PublishComponent,
    TrueOrFalseAnswerComponent,
    MultipleChoiceAnswerComponent,
    OpenEndedAnswerComponent,
    AnswersUserComponent,
    AnswerReportComponent,
    AnswersReportComponent,
    MultipleChoiceReportComponent,
    TrueOrFalseReportComponent,
    PasswordComponent,
    OpenEndedReportComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    MatTableModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatSelectModule,
    MatCardModule,
    MatInputModule,
    MatCheckboxModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatIconModule,
    MatRadioModule,
    FormsModule
  ],
  providers: [
    APIService,
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  entryComponents: [
    MultipleChoiceComponent,
    TrueFalseOptionComponent,
    UserGridComponent,
    TrueOrFalseAnswerComponent,
    MultipleChoiceAnswerComponent,
    OpenEndedAnswerComponent,
    MultipleChoiceReportComponent,
    TrueOrFalseReportComponent,
    OpenEndedReportComponent, ],
    bootstrap: [AppComponent]
})
export class AppModule { }
