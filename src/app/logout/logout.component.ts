import { Component, OnInit } from '@angular/core';
import { AuthappService } from '../services/authapp.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  constructor(public BasicAuth: AuthappService) 
  { }

  ngOnInit() {
    this.BasicAuth.clearAll();
  }

}
