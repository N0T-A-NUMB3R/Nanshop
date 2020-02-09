import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SalutiDataService {

  constructor() { }

  getSaluti(){
    console.log("Saluti");
  }
}
