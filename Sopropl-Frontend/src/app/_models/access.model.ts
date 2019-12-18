import { Project } from './project.model';
import { Team } from './team.model';

export class Access {
  permission: number;
  project: Project;
  team: Team;
}
