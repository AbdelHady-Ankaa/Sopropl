import { Photo } from './Photo';

export interface User {
  userName?: string;
  photo?: Photo;
  name?: string;
  city?: string;
  country?: string;
  bio?: string;
  lastActive: Date;
}
