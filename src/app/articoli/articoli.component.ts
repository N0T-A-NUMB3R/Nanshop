import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import { ArticoliDataService } from '../services/data/articoli-data.service';
import { from } from 'rxjs';

export class ApiMsg{
  constructor (
    public code: string,
    public message : string
  ){}
}

export class Articoli{
  
  constructor(
    public codart: string,
    public descrizione: string,
    public um: string,
    public pzcart: number,
    public peso: number,
    public prezzo: number,
    public isActive: boolean,
    public data : Date

  ){}
}

@Component({
  selector: 'app-articoli',
  templateUrl: './articoli.component.html',
  styleUrls: ['./articoli.component.css']
})

export class ArticoliComponent implements OnInit {
  numArt = 0;
  righe = 10;
  pagina = 1;
  filter : string = '';
  articoli : Articoli [];
  articolo : Articoli;

  apiMsg : ApiMsg;
  messaggio : string;

  constructor(private articoliService : ArticoliDataService, private route : ActivatedRoute) { }

  ngOnInit() {
  
    this.filter = this.route.snapshot.params['filter'];
    
    if (this.filter != undefined){
      this.getArticoli(this.filter);
    }
  }

  refresh(){
    this.getArticoli(this.filter);
  }
  
  public getArticoli(filter : string) {

    this.articoliService.getArticoloByCodice(filter).subscribe(
      response => {

        this.articoli = [];

        console.log('Ricerchiamo articoli per codart con filtro ' + filter);

        this.articolo = response;
        console.log(this.articolo);

        this.articoli.push(this.articolo);
        this.numArt = this.articoli.length
        console.log(this.articoli.length);

      },
      error => {
        console.log(error.error);

        console.log('Ricerchiamo per descrizione con filtro ' + filter);
        this.articoliService.getArticoliByDesc(filter).subscribe(
          response => {

            this.articoli = response;
            console.log(this.articoli);

            this.numArt = this.articoli.length
            console.log(this.articoli.length);

          },
          error => {
            console.log(error.error);
            console.log('Ricerchiamo per EAN con filtro ' + filter);

            this.articoliService.getArticoliByEan(filter).subscribe(
              response => {
                this.articoli = [];

                this.articolo = response;
                console.log(this.articolo);

                this.articoli.push(this.articolo);
                this.numArt = this.articoli.length
                console.log(this.articoli.length);
              },
              error => {
                console.log(error.error);
                this.articoli = [];
              }
            )
          }
        )
      }
    )
  }
  public eliminaArticolo (codart : string)
  {
    console.log(`Eliminazione articolo ${codart}`);
    this.articoliService.deleteArticolo(codart).subscribe(
      response => {
        this.apiMsg = response;
        this.messaggio = this.apiMsg.message;
        this.refresh();
      }    
    )
  }
}
