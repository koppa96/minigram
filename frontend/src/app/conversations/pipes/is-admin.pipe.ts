import { Pipe, PipeTransform } from '@angular/core'
import { ConversationDetailsDto } from '../../shared/clients'

@Pipe({
  pure: true,
  name: 'isAdmin'
})
export class IsAdminPipe implements PipeTransform {
  transform(value: ConversationDetailsDto, ...args: any[]): boolean {
    return value.memberships.some(x => x.member.id === args[0] && x.isAdmin)
  }
}
