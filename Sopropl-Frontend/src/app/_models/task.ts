export class Task {
  // tslint:disable-next-line: variable-name
  constructor(id: string, start_date: string, text: string, duration: number, progress: number, parent?: number) {
    this.id = id;
    this.text = text;
    this.start_date = start_date;
    this.progress = progress;
    this.parent = parent;
    this.duration = duration;
  }
  id: string;
  // tslint:disable-next-line: variable-name
  start_date: string;
  text: string;
  progress: number;
  duration: number;
  parent?: number;
}
