import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

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
  

  constructor(private route : Router) { }

  ngOnInit() {
  }

  gestAut() {
    
    if (this.userId === "fabio" && this.password === "123") {
      this.autenticato = true;
      this.route.navigate(["welcome",this.userId]);
     
    }
    else {
      this.autenticato = false;
      this.showMsgLogin = true;
     
    }
  }
}
