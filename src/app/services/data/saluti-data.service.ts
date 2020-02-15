import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { utf8Encode } from '@angular/compiler/src/util';


@Injectable({
  providedIn: 'root'
})
export class SalutiDataService {

  constructor(public httpClient : HttpClient) { }
  
  getSaluti(utente: string){
    return this.httpClient.get('http://localhost:8050/api/saluti/${utente}');
  
  }
}
