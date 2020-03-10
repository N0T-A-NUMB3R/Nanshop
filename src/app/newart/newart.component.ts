import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Articoli, ApiMsg, FamAssort, Iva } from '../articoli/articoli.component';
import { ArticoliDataService } from '../services/data/articoli-data.service';

@Component({
  selector: 'app-newart',
  templateUrl: './newart.component.html',
  styleUrls: ['./newart.component.css']
})

export class NewartComponent implements OnInit {

  CodArt: string = '';
  articolo: Articoli;
  Conferma: string = '';
  Errore: string = '';

  apiMsg: ApiMsg;

  Iva: Iva;
  Cat: FamAssort;


  constructor(private route: ActivatedRoute, private articoliService: ArticoliDataService, private router : Router) { }

  ngOnInit() {

    //Inizializziamo l'articolo
    this.articolo = new Articoli("-1", "", "", 0, 0, 0, true, new Date(), 1, 22);

    this.CodArt = this.route.snapshot.params['codart'];

    //Otteniamo i dati dell'Iva
    this.articoliService.getIva().subscribe(
      response => {

        this.Iva = response;
        console.log(response);
      },
      error => {
        console.log(error);
      }
    )

    //Otteniamo i dati della famiglia assortimento
    this.articoliService.getCat().subscribe(
      response => {

        this.Cat = response;
        console.log(response);
      },
      error => {
        console.log(error);
      }
    )

    //Otteniamo i dati dell'articolo
    this.articoliService.getArticoloByCodice(this.CodArt).subscribe(
      response => {

        this.articolo = response;
        console.log(this.articolo);
      },
      error => {
        console.log(error);
      }
    )
  }
  
  salva() {

    console.log(this.articolo);

    this.articoliService.updateArticolo(this.articolo).subscribe(

      response => {
        console.log(response);

        this.apiMsg = response;
        this.Conferma = this.apiMsg.message;

        console.log(this.Conferma);


      },
      error => {

        console.log(error);

        this.apiMsg = error.error;
        this.Errore = this.apiMsg.message;

        console.log(this.Errore); 


      }
    )
  }
  public indietro(filter : string) {
    console.log(`ritorna indietro ${filter}`);
    this.router.navigate(['articoli', filter]);
  }
  
}
