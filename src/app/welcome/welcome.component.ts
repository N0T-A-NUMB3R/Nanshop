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

  utente = "";

  constructor(private route:ActivatedRoute, public salutiSrv: SalutiDataService) { }

  ngOnInit() {
     this.utente = this.route.snapshot.params['userId'];
  }

  getSaluti(){
    this.salutiSrv.getSaluti();
  }

}
