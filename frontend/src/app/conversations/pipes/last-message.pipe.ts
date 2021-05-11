import { Pipe, PipeTransform } from '@angular/core'
import { MessageDto } from '../../shared/clients'

@Pipe({
  pure: true,
  name: 'lastMessage'
})
export class LastMessagePipe implements PipeTransform {
  transform(value: MessageDto | undefined | null): string | undefined {
    return value ? `${value.sender.userName}: ${value.text.substr(0, 10)}${value.text.length > 10 ? '...' : ''}` : undefined;
  }
}
