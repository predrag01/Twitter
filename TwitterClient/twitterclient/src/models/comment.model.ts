import { User } from "./user.model";
import { Post } from "./post.model";

export interface Comment{
    id:number;
    userId?:  number;
    user?:User;
    postId?:number;
    post?:Post;
    commentContent?:string;
}