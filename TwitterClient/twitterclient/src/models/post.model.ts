import { User } from "./user.model";

export interface Post{
    id: number;
    content?: string;
    posted: string;
    authorId?:  number;
    author?:User;
    likeCounter?:number;
    datum: Date;
}