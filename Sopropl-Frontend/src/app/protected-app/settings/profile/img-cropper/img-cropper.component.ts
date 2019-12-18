import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { ImageCropperComponent, ImageCroppedEvent } from 'ngx-image-cropper';
import { MatDialogRef, MatDialog } from '@angular/material';
// import { UploadService } from 'src/app/_services/upload.service';
import { AlertifyService } from 'src/app/_services/Alertify.service';
// import { forkJoin } from 'rxjs';
// import { AuthService } from 'src/app/_services/auth.service';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-img-cropper',
  templateUrl: './img-cropper.component.html',
  styleUrls: ['./img-cropper.component.scss']
})
export class ImgCropperComponent implements OnInit {
  constructor(
    private alertify: AlertifyService,
    public dialogRef: MatDialogRef<ImgCropperComponent>,
    public dialog: MatDialog,
    private accountService: AccountService
  ) {
    // this.fileChangeEvent(data.event);
  }
  // progress;
  // canBeClosed = true;
  // primaryButtonText = 'Upload';
  // showCancelButton = true;
  // uploading = false;
  // uploadSuccessful = false;
  @ViewChild('file') file;
  // public files: Set<Blob> = new Set();
  imageChangedEvent: any;
  croppedImage: any;
  showCropper = false;

  @ViewChild(ImageCropperComponent) imageCropper: ImageCropperComponent;
  addFiles() {
    this.file.nativeElement.click();
  }

  editImage() {
    this.dialog.open(ImgCropperComponent);
  }
  uploadPhoto() {
    this.accountService.uploadPhoto(this.croppedImage);
    // // if everything was uploaded already, just close the dialog
    // this.files.add(this.croppedImage);
    // if (this.uploadSuccessful) {
    //   return this.dialogRef.close();
    // }

    // // set the component state to "uploading"
    // this.uploading = true;

    // // start the upload and save the progress map
    // this.progress = this.uploadService.upload(this.files, 'account/setPhoto');

    // // convert the progress map into an array
    // const allProgressObservables = [];
    // for (const key in this.progress) {
    //   if (this.progress.hasOwnProperty(key)) {
    //     allProgressObservables.push(this.progress[key].progress);
    //   }
    // }

    // // Adjust the state variables

    // // The OK-button should have the text "Finish" now
    // this.primaryButtonText = 'Finish';

    // // The dialog should not be closed while uploading
    // this.canBeClosed = false;
    // this.dialogRef.disableClose = true;

    // // Hide the cancel-button
    // this.showCancelButton = false;

    // // When all progress-observables are completed...
    // forkJoin(allProgressObservables).subscribe(end => {
    //   // ... the dialog can be closed again...
    //   this.canBeClosed = true;
    //   this.dialogRef.disableClose = false;

    //   // ... the upload was successful...
    //   this.uploadSuccessful = true;

    //   // ... and the component is no longer uploading
    //   this.uploading = false;
    //   this.auth.reloadToken();
    // });
  }
  ngOnInit() {}

  fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
  }
  imageCropped(event: ImageCroppedEvent) {
    this.croppedImage = event.base64;
  }
  imageLoaded() {
    this.showCropper = true;
  }
  cropperReady() {}
  loadImageFailed() {}
  rotateLeft() {
    this.imageCropper.rotateLeft();
  }
  rotateRight() {
    this.imageCropper.rotateRight();
  }
  flipHorizontal() {
    this.imageCropper.flipHorizontal();
  }
  flipVertical() {
    this.imageCropper.flipVertical();
  }
}
