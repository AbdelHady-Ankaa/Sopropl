import { Photo } from './Photo';

export interface Project {
  name: string;
  shortName: string;
  description: string;
  isActive: boolean;
  logo: Photo;
  dateCreated: Date;
  dateUpdated: Date;
}
