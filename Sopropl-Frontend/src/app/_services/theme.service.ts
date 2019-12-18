import { Injectable } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';
import { OverlayContainer } from '@angular/cdk/overlay';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private darkTheme: Subject<boolean> = new BehaviorSubject<boolean>(false);

  isDarkTheme = this.darkTheme.asObservable();
  DARK = 'Dark';
  LIGHT = 'Light';
  constructor(private overlayContainer: OverlayContainer) {}

  setDarkTheme(isDarkTheme: boolean) {
    this.setNgMaterialTheme(isDarkTheme);
    this.isDarkTheme.subscribe(res => {
      this.switchBootstrapTheme(res ? this.DARK : this.LIGHT);
    });
  }

  setNgMaterialTheme(isDarkTheme: boolean) {
    this.darkTheme.next(isDarkTheme);
    if (isDarkTheme) {
      this.overlayContainer.getContainerElement().classList.add('dark-theme');
    } else {
      this.overlayContainer
        .getContainerElement()
        .classList.remove('dark-theme');
    }
  }

  findStyle(theme: string) {
    const links = document.getElementsByTagName('link');
    for (const key in links) {
      if (links.hasOwnProperty(key)) {
        if (
          links[key].rel.indexOf('stylesheet') !== -1 &&
          links[key].title === theme
        ) {
          return true;
        }
      }
    }
    return false;
  }

  switchBootstrapTheme(theme: string) {
    if (theme && this.findStyle(theme)) {
      const links = document.getElementsByTagName('link');
      for (const key in links) {
        if (links.hasOwnProperty(key)) {
          const link = links[key];
          if (link.rel.indexOf('stylesheet') !== -1 && link.title) {
            if (link.title === theme) {
              link.disabled = false;
            } else {
              link.disabled = true;
            }
          }
        }
      }
    }
  }
}
