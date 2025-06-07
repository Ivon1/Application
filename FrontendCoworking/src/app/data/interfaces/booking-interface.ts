import { WorkspaceInterface } from './workspace-interface';
import { AvailabilityInterface } from './availability-interface';

export interface BookingInterface {
    id:number | null;
    name: string | null;
    email: string | null;
    startDate: Date | null;
    endDate: Date | null;
    workspace: WorkspaceInterface | null;
    availability: AvailabilityInterface | null;
}
