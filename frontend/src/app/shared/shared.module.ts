import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common';
import { PagerComponent } from './components/pager/pager.component'
import { NbButtonModule, NbIconModule } from '@nebular/theme'
import { DefaultPipe } from './pipes/default.pipe'

@NgModule({
  imports: [
    CommonModule,
    NbButtonModule,
    NbIconModule
  ],
  declarations: [
    PagerComponent,
    DefaultPipe
  ],
  exports: [
    PagerComponent,
    DefaultPipe
  ]
})
export class SharedModule { }
