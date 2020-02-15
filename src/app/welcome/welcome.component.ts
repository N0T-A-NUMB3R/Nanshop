import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SalutiDataService } from '../services/data/saluti-data.service';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css']
})
export class WelcomeComponent implements OnInit {

 // messaggio = "Saluti sono il componente welcome";
  saluti = 'Benvenuti nel sito Nanshop'
  titolo2 = 'Seleziona gli articoli da acquistare'
  messaggio = "";
  utente = "";

  constructor(private route:ActivatedRoute, public salutiSrv: SalutiDataService) { }

  ngOnInit() {
     this.utente = this.route.snapshot.params['userId'];
  }

  getSaluti(){
    
    this.salutiSrv.getSaluti(this.utente).subscribe(
      res => this.handleResponse(res),
      err => this.handleError(err)
      
      );

  }
  handleResponse(response: Object){
    this.messaggio = response.toString();
  }

  handleError(error){
    this.messaggio = error.error.messaggio;
    
  }
}
