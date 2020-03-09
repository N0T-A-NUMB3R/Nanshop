import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ArticoliDataService } from '../services/data/articoli-data.service';
import { Articoli } from '../articoli/articoli.component';

@Component({
  selector: 'app-newart',
  templateUrl: './newart.component.html',
  styleUrls: ['./newart.component.css']
})
export class NewartComponent implements OnInit {

  CodArt : string = '';
  Articolo : Articoli;

  constructor(private route : ActivatedRoute, private service : ArticoliDataService) { }

  ngOnInit() {

    this.CodArt = this.route.snapshot.params['codeart'];
    this.service.getArticoloByCodice(this.CodArt).subscribe(
      resp => {
        this.Articolo = resp;
        console.log(this.Articolo);
      },
      err => {
        console.log(err);
      }
    )

  }

}
