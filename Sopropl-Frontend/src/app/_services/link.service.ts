import { Injectable } from '@angular/core';
import { Link } from '../_models/Link';

@Injectable({
  providedIn: 'root'
})
export class LinkService {
  get(): Promise<Link[]> {
    return Promise.resolve([
      { id: 'o', source: 'A', target: 'B', type: '0' }
    ]);
  }
}
