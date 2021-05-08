import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  pure: true,
  name: 'default'
})
export class DefaultPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {
    return (value === undefined || value === null) ? args[0] : value
  }
}
