import { AmenityInterface } from "./amenity-interface";
import { AvailabilityInterface } from "./availability-interface";

export interface WorkspaceInterface {
    id:number;
    worksapceTypeName: string;
    description: string;
    capacity: string;
    amenities: AmenityInterface[];
    photoUrls: string[];
    availabilities: AvailabilityInterface[];
}
