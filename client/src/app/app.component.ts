import { HttpClient,HttpParams } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { take } from 'rxjs';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'client';
  registerForm!: FormGroup;

  constructor(private http: HttpClient, private fb : FormBuilder) {
    
  }

  users : any[] = [];
  ngOnInit() {
    this.initForm();
    this.http.get(environment.baseUrl + 'Users',{observe : 'body'}).pipe(take(1)).subscribe((res :any) => {
      console.log('users', res);
      this.users = res;
    })
  }

  initForm() {
    this.registerForm = this.fb.group({
      userName: [""],
      password : [""]
    })
  }

  onRegister() {
    const formValue = this.registerForm.getRawValue();
    const payload = { ...formValue }
    const params = new HttpParams().set('userName', payload.userName).set('password', payload.password);
    this.http.post(environment.baseUrl + 'Account/Register', {}, { params : params,observe: 'body' }).pipe(take(1)).subscribe((res) => {
      this.users.push(res);
    })
  }
}
