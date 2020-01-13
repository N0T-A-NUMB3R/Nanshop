import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthappService } from '../services/authapp.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  userId = "";
  password = "";
  autenticato = false;
  showMsgLogin  = false;

  errorMsg = "Spiacente, la userId o la password sono errati";
  

  constructor(private route : Router, private BasicAuth : AuthappService) { }

  ngOnInit() {
  }

  gestAut() {
   if ( this.BasicAuth.autentica(this.userId, this.password))
   {
    this.autenticato = true;
    this.route.navigate(["welcome",this.userId]);
   }
    else {
      this.autenticato = false;
      this.showMsgLogin = true;
     
    }

  }

}