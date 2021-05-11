import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { NbDialogRef } from '@nebular/theme'

@Component({
  selector: 'app-new-conversation',
  templateUrl: './new-conversation.component.html',
  styleUrls: ['./new-conversation.component.scss']
})
export class NewConversationComponent {

  formGroup: FormGroup

  constructor(
    private dialogRef: NbDialogRef<NewConversationComponent>,
    formBuilder: FormBuilder
  ) {
    this.formGroup = formBuilder.group({
      conversationName: ['', Validators.required]
    })
  }

  cancel() {
    this.dialogRef.close()
  }

  create(data: { conversationName: string }) {
    this.formGroup.markAllAsTouched()
    if (this.formGroup.valid) {
      this.dialogRef.close(data)
    }
  }

}
