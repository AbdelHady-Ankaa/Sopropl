import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import {
  HttpRequest,
  HttpClient,
  HttpEventType,
  HttpResponse,
  HttpHeaders
} from '@angular/common/http';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class UploadService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  public upload(
    files: Set<File | Blob | string>,
    url: string
  ): { [key: string]: { progress: Observable<number> } } {
    // this will be the our resulting map
    const status: { [key: string]: { progress: Observable<number> } } = {};
    let i = 0;
    files.forEach(file => {
      i++;
      // create a new multipart-form for every file
      const formData: FormData = new FormData();
      // var fileReader = new FileReader();
      // fileReader.readAsBinaryString(file);

      formData.append(
        'file',
        typeof file === 'string' ? this.dataURItoBlob(file) : file
      );
      // create a http-post request and pass the form
      // tell it to report the upload progress

      const headers = new HttpHeaders();
      headers.append('Accept-Encoding', 'gzip, deflate, br');
      const req = new HttpRequest('POST', this.baseUrl + url, formData, {
        reportProgress: true,
        headers
      });

      // create a new progress-subject for every file
      const progress = new Subject<number>();

      // send the http-request and subscribe for progress-updates
      this.http.request(req).subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          // calculate the progress percentage
          const percentDone = Math.round((100 * event.loaded) / event.total);

          // pass the percentage into the progress-stream
          progress.next(percentDone);
        } else if (event instanceof HttpResponse) {
          // Close the progress-stream if we get an answer form the API
          // The upload is complete
          progress.complete();
        }
      });

      // Save every progress-observable in a map of all observables
      status[i] = {
        progress: progress.asObservable()
      };
    });

    // return the map of progress.observables
    return status;
  }

  private dataURItoBlob(dataURI) {
    // convert base64 to raw binary data held in a string
    // doesn't handle URLEncoded DataURIs - see SO answer #6850276 for code that does this
    const byteString = atob(dataURI.split(',')[1]);

    // separate out the mime component
    const mimeString = dataURI
      .split(',')[0]
      .split(':')[1]
      .split(';')[0];

    // write the bytes of the string to an ArrayBuffer
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }

    // write the ArrayBuffer to a blob, and you're done
    const bb = new Blob([ab]);
    return bb;
  }
}
