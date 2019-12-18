import { Injectable, TemplateRef } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { ComponentType } from '@angular/cdk/overlay/index';
import { timeAgo } from '../_extentions/timeAgo';
declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {
  constructor(private matDialog: MatDialog) {}

  confirm(message: string, okCallBack: () => any, cancelCallBack: () => any) {
    alertify.confirm(message, (e: any) => {
      if (e) {
        okCallBack();
      } else {
        cancelCallBack();
      }
    });
  }

  success(body: string, title: string = 'success', date: Date = new Date()) {
    alertify.success(
      '<div class="toast-header bg-transparent"><i class="material-icons">done</i><strong class="mr-auto">' +
        title +
        '</strong><small class="ml-3 text-muted">' +
        timeAgo(date) +
        '</small><button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">' +
        '<span aria-hidden="true">&times;</span></button></div><div class="toast-body">' +
        body +
        '</div>'
    );
  }

  warning(body: string, title: string = 'warning', date: Date = new Date()) {
    alertify.warning(
      '<div class="toast-header bg-transparent"><i class="material-icons">warning</i><strong class="mr-auto">' +
        title +
        '</strong><small class="ml-3 text-muted">' +
        timeAgo(date) +
        '</small><button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">' +
        '<span aria-hidden="true">&times;</span></button></div><div class="toast-body">' +
        body +
        '</div>'
    );
  }

  message(body: string, title: string = 'message', date: Date = new Date()) {
    alertify.message(
      '<div class="bg-white"><div class="toast-header"><i class="material-icons">message</i><strong class="mr-auto">' +
        title +
        '</strong><small class="ml-3 text-muted">' +
        timeAgo(date) +
        '</small><button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">' +
        '<span aria-hidden="true">&times;</span></button></div><div class="toast-body">' +
        body +
        '</div></div>'
    );
  }

  error(body: string, title: string = 'error', date: Date = new Date()) {
    alertify.error(
      '<div class="toast-header bg-transparent"><i class="material-icons">error_outline</i><strong class="mr-auto">' +
        title +
        '</strong><small class="ml-3 text-muted">' +
        timeAgo(date) +
        '</small><button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">' +
        '<span aria-hidden="true">&times;</span></button></div><div class="toast-body">' +
        body +
        '</div>'
    );
  }

  dialog(
    componentOrTemplateRef: ComponentType<unknown> | TemplateRef<unknown>,
    config?: MatDialogConfig<any>
  ) {
    this.matDialog.open(componentOrTemplateRef, config);
  }
}
