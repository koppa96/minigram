import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common';
import { PagerComponent } from './components/pager/pager.component'
import { NbButtonModule, NbIconModule } from '@nebular/theme'
import { DefaultPipe } from './pipes/default.pipe'
import { PagerPipe } from './pipes/pager.pipe'

@NgModule({
  imports: [
    CommonModule,
    NbButtonModule,
    NbIconModule
  ],
  declarations: [
    PagerComponent,
    DefaultPipe,
    PagerPipe
  ],
  exports: [
    PagerComponent,
    DefaultPipe,
    PagerPipe
  ]
})
export class SharedModule { }
