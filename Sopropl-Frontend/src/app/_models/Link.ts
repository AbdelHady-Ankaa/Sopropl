export class Link {
  constructor(id: string, fromActivityName: string, toActivityName: string, type: string) {
    this.id = id,
      this.source = fromActivityName;
    this.target = toActivityName;
    this.type = type;
  }
  id: string;
  source: string;
  target: string;
  type: string;
}
