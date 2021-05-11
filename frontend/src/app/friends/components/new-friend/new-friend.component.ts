import { Component } from '@angular/core'
import { UserListDto, UsersClient } from '../../../shared/clients'
import { Observable } from 'rxjs'
import { FormControl } from '@angular/forms'
import { debounceTime, distinctUntilChanged, filter, switchMap } from 'rxjs/operators'
import { NbDialogRef } from '@nebular/theme'

@Component({
  selector: 'app-new-friend',
  templateUrl: './new-friend.component.html',
  styleUrls: ['./new-friend.component.scss']
})
export class NewFriendComponent {

  formControl = new FormControl()
  recipients$: Observable<UserListDto[]>

  constructor(
    private client: UsersClient,
    private dialogRef: NbDialogRef<NewFriendComponent>
  ) {
    this.recipients$ = this.formControl.valueChanges.pipe(
      filter(value => value.length >= 3),
      distinctUntilChanged(),
      debounceTime(500),
      switchMap(value => client.listUsers(value))
    )
  }

  displayFn(user: UserListDto | string) {
    if (typeof user === 'string') {
      return user
    } else {
      return user?.userName
    }
  }

  send() {
    this.dialogRef.close(this.formControl.value.id)
  }

  cancel() {
    this.dialogRef.close()
  }
}
