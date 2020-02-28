import { Component, OnInit } from '@angular/core';
import { ArticoliDataService } from '../services/data/articoli-data.service';

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
  articoli : Articoli [];

  constructor(private articoliService : ArticoliDataService) { }

  ngOnInit() {
    this.articoliService.getArticoli('Barilla').subscribe(
      response => {
        console.log(response);
        this.articoli = response;
        this.numArt = this.articoli.length;
      }
        
    )
    
  }

}
