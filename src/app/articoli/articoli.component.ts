import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import { ArticoliDataService } from '../services/data/articoli-data.service';
import { from } from 'rxjs';

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
  
  public getArticoli (filter : string){
    this.articoliService.getArticoli(filter).subscribe(
      response => {
        console.log('Ricerchiamo articoli con filtro' + filter);
        this.articoli = response;
        this.numArt = this.articoli.length;
      })
  }
}
