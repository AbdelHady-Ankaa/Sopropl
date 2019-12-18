import { Arrow } from './arrow.model';

export enum ActivityType {
  BUG, CHANGE_REQUEST, DEVELOPMENT, ENHANCEMENT, IDEA, MAINTENANCE,
  QUALITY_ASSURANCE, RELEASE, RESEARCH, UNIT_TESTING, UPstring, EPIC, STORY, OTHERS
}

export enum ActivityState {
  NEW, IN_PROGRESS, RESOLVED, CLOSED
}

export class Activity {
  // constructor(
  //   name?: string,
  //   type?: ActivityType,
  //   state?: ActivityState,
  //   earlyStart?: string,
  //   earlyFinish?: string,
  //   lateStart?: string,
  //   lateFinish?: string,
  //   freeFloat?: number,
  //   totalFloat?: number,
  //   duration?: number,
  //   priority?: number,
  //   estimatedHours?: number,
  //   hoursSpent?: number,
  //   description?: number,
  //   outArrows?: Arrow[],
  //   inArrows?: Arrow[],
  // ) { }

  name?: string;
  type?: ActivityType;
  state?: ActivityState;
  earlyStart?: string;
  earlyFinish?: string;
  lateStart?: string;
  lateFinish?: string;
  freeFloat?: number;
  totalFloat?: number;
  duration?: number;
  priority?: number;
  estimatedHours?: number;
  hoursSpent?: number;
  description?: number;
  outArrows?: Arrow[];
  inArrows?: Arrow[];
}
