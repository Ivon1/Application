import { AmenityInterface } from "./amenity-interface";

export interface WorkspaceInterface {
    id:number;
    worksapceTypeName: string;
    description: string;
    capacity: string;
    amenities: AmenityInterface[];
    photoUrls: string[];
    availabilities: string[];
}
