// onePost.tsx
import React from "react";
import { Post } from "../models/post.model";
import { time } from "console";

const OnePost = (props: { post: Post }) => {
  if (!props.post) {
    return <p>Loading...</p>;
  }

  return (
    <div>
      <p>{props.post.author?.username}</p>
      <p>{props.post.content}</p>
      <p>
        {props.post.datum.toDateString()}{" "}
        {props.post.datum.toLocaleTimeString()}
      </p>
      <p>{props.post.likeCounter}</p>
    </div>
  );
};

export default OnePost;
