import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core'
import { PagerModel } from '../../models/pager.model'

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent {
  @Input() model: PagerModel
  @Output() loadPage = new EventEmitter<number>()

  constructor() { }
}
