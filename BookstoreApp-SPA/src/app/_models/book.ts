import { Photo } from './photo';

export interface Book {
    id: number;
    title: string;
    dateAdded: Date;
    dateModified: Date;
    photoUrl: string;
    author?: string;
    publisher?: string;
    description?: string;
    photos?: Photo[];
}
