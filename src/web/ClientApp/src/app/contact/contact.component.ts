import { Component, OnInit } from '@angular/core';
import { StocksService, GetErrors } from '../services/stocks.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {

  public saved: boolean = false
  public email: string
  public message: string
  public errors: string[]

  constructor(
    private stockService:StocksService
  ) { }

  ngOnInit() {
  }

  sendMessage() {
    var obj = {
      email: this.email,
      message: this.message
    }

    this.stockService.sendMessage(obj).subscribe(
      _ => this.saved = true, err => this.errors = GetErrors(err))
  }
}
