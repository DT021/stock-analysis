import { Component, OnInit } from '@angular/core';
import { StocksService, OptionDefinition, GetErrors } from '../services/stocks.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe, Location } from '@angular/common';

@Component({
  selector: 'app-sold-option-detail',
  templateUrl: './sold-option-detail.component.html',
  styleUrls: ['./sold-option-detail.component.css'],
  providers: [DatePipe]
})
export class SoldOptionDetailComponent implements OnInit {
  public option: OptionDefinition;

  public positionType: string
  public premium: number
  public filled: string
  public numberOfContracts: number

  public errors: string[]

  constructor(
    private service: StocksService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private datePipe: DatePipe
  ) { }

  ngOnInit() {
    this.filled = Date()
    this.filled = this.datePipe.transform(this.filled, 'yyyy-MM-dd');

    var id = this.route.snapshot.paramMap.get('id');

    this.getOption(id)
  }

  getOption(id:string){
    this.service.getOption(id).subscribe( result => {
      this.option = result
    })
  }

  record() {

    this.errors = null;

    var opt = {
      ticker: this.option.ticker,
      strikePrice: this.option.strikePrice,
      optionType: this.option.optionType,
      expirationDate: this.option.expirationDate,
      numberOfContracts: this.numberOfContracts,
      premium: this.premium,
      filled: this.filled
    }

    if (this.positionType == 'buy') this.recordBuy(opt)
    if (this.positionType == 'sell') this.recordSell(opt)
  }

  delete() {

    if (confirm("are you sure you want to delete this option?"))
    {
      this.errors = null;

      this.service.deleteOption(this.option.id).subscribe(r => {
        this.router.navigateByUrl('/dashboard')
      })
    }
  }

  back() {
    this.location.back()
  }

  recordBuy(opt: object) {
    this.service.buyOption(opt).subscribe( r => {
      this.getOption(r.id)
    }, err => {
      this.errors = GetErrors(err)
    })
  }

  recordSell(opt: object) {
    this.service.sellOption(opt).subscribe( r => {
      this.getOption(r.id)
    }, err => {
      this.errors = GetErrors(err)
    })
  }

  expire(assigned:boolean) {
    if (assigned)
    {
      if (!confirm("Are you sure you want to mark this as assigned?"))
      {
        return;
      }
    }
    else
    {
      if (!confirm("Are you sure you want to mark this as expired?"))
      {
        return;
      }
    }

    var opt = {
      id: this.option.id,
      assigned: assigned,
    }

    this.service.expireOption(opt).subscribe( _ => {
      this.router.navigateByUrl('/dashboard')
    }, err => {
      this.errors = GetErrors(err)
    })
  }

}
