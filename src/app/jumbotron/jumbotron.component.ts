import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-jumbotron',
  templateUrl: './jumbotron.component.html',
  styleUrls: ['./jumbotron.component.css']
})
export class JumbotronComponent implements OnInit {

  @Input() Titolo: string
  @Input() SottoTitolo: string
  @Input() Show: true
  

  constructor() { }

  ngOnInit() {
    console.log(this.Titolo)
  }

}
