import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Articoli } from 'src/app/articoli/articoli.component';

@Injectable({
  providedIn: 'root'
})
export class ArticoliDataService {
  
  server = "localhost";
  port = "5051";

  constructor(public httpClient: HttpClient) { }

    getArticoliByDesc(descrizione : string){
      return this.httpClient.get<Articoli[]>(`http://${this.server}:${this.port}/api/articoli/cerca/descrizione/${descrizione}`);
    }
    getArticoloByCodice(codArt : string) {
      return this.httpClient.get<Articoli>(`http://${this.server}:${this.port}/api/articoli/cerca/codice/${codArt}`);
    }
    getArticoliByEan(barCode : string) {
      return this.httpClient.get<Articoli[]>(`http://${this.server}:${this.port}/api/articoli/cerca/ean/${barCode}`);
    }
}
