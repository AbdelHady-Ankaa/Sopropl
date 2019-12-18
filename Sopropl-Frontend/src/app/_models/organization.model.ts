import { Photo } from './Photo';
import { Project } from './project.model';

export class Organization {
  name: string;
  logo: Photo;
  contactPhone: string;
  website: string;
  dateCreated: Date;
  dateUpdated: Date;
  isActive: boolean;
  isBeta: boolean;
  isDeactivated: boolean;
}
