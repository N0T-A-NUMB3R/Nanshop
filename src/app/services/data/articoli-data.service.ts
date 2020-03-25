import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Articoli, ApiMsg, FamAssort, Iva } from 'src/app/articoli/articoli.component';

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
      return this.httpClient.get<Articoli>(`http://${this.server}:${this.port}/api/articoli/cerca/ean/${barCode}`);
    }
    deleteArticolo(codArt : string){
      return this.httpClient.delete<ApiMsg>(`http://${this.server}:${this.port}/api/articoli/elimina/${codArt}`);
    }
    getIva(){
      return this.httpClient.get<Iva>(`http://${this.server}:${this.port}/api/iva`);
    }
    getCat(){
     return this.httpClient.get<FamAssort>(`http://${this.server}:${this.port}/api/cat`);
    }
    updateArticolo(articolo: Articoli) {
      return this.httpClient.put<ApiMsg>(`http://${this.server}:${this.port}/api/articoli/modifica/`, articolo);
  }
}
