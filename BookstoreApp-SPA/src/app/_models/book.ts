import { Photo } from './photo';

export interface Book {
    id: number;
    title: string;
    dateReleased: Date;
    dateAdded: any;
    dateModified: Date;
    photoUrl: string;
    author?: string;
    publisher?: string;
    description?: string;
    photos?: Photo[];
}
