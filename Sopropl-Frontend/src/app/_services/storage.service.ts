import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  constructor() {}
  getItem(key: string) {
    return JSON.parse(localStorage.getItem(key));
  }
  setItem(key: string, item: any) {
    localStorage.setItem(key, JSON.stringify(item));
  }

  removeItem(key: string) {
    localStorage.removeItem(key);
  }

  clear() {
    localStorage.clear();
  }
}
