import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WelcomeComponent } from '../welcome/welcome.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  userid = ''
  password = ''
  autenticato = true
  //consentito = false
  errorMsg = 'Spiacente, la userid o la password sono errati!'
  //infoMsg = 'Accesso Consentito'

  constructor(private route: Router) { }

  ngOnInit() {
  }

  gestAut() {

    if (this.userid === 'Fabui' && this.password === '123') {
      this.autenticato = true;
      this.route.navigate(['welcome', this.userid])
      //this.consentito = true;
    }
    else {
      this.autenticato = false;
      //this.consentito = false;
    }

  }

}