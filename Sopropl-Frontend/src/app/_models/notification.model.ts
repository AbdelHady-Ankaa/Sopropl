export class NotificationType {
  public static NORMAL = 0;
  public static INVAITATION = 1;
}
export interface NotificationM {
  id: string;
  title: string;
  body: string;
  sentDate: Date;
  icon: string;
  data: string;
  type: number;
}
