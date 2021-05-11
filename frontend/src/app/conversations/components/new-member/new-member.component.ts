import { Component } from '@angular/core'
import { FormControl } from '@angular/forms'
import { Observable } from 'rxjs'
import { FriendshipsClient, UserListDto } from '../../../shared/clients'
import { NbDialogRef } from '@nebular/theme'
import { debounceTime, distinctUntilChanged, filter, map, switchMap } from 'rxjs/operators'
import { defaultPageSize } from '../../../shared/constants'

@Component({
  selector: 'app-new-member',
  templateUrl: './new-member.component.html',
  styleUrls: ['./new-member.component.scss']
})
export class NewMemberComponent {
  formControl = new FormControl()
  members$: Observable<UserListDto[]>

  constructor(
    private client: FriendshipsClient,
    private dialogRef: NbDialogRef<NewMemberComponent>
  ) {
    this.members$ = this.formControl.valueChanges.pipe(
      filter(value => value.length >= 3),
      distinctUntilChanged(),
      debounceTime(500),
      switchMap(value => client.listFriends(value, 0, defaultPageSize)),
      map(friends => friends.data.map(x => x.friend))
    )
  }

  displayFn(user: UserListDto | string) {
    if (typeof user === 'string') {
      return user
    } else {
      return user?.userName
    }
  }

  add() {
    this.dialogRef.close(this.formControl.value.id)
  }

  cancel() {
    this.dialogRef.close()
  }

}
