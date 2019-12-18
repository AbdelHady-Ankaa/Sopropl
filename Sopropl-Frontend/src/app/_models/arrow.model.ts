import { Activity } from './activity.model';

export enum ArrowType {
  FINISH_TO_START, START_TO_START, FINISH_TO_FINISH, START_TO_FINISH,
}

export class Arrow {
  type: number;
  toActivity: Activity;
  fromActivity: Activity;
  constraintValue: number;
}

