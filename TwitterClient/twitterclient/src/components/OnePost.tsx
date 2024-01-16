// onePost.tsx
import React from "react";
import { Post } from "../models/post.model";
import "../App1.css"; // Dodaj CSS fajl

const OnePost = (props: { post: Post }) => {
  if (!props.post) {
    return <p>Loading...</p>;
  }
  const handleDelete = async () => {
    try {
      const response = await fetch(
        `https://localhost:7082/Post/DeletePost?postId=${encodeURIComponent(
          props.post.id
        )}`,
        {
          method: "DELETE",
        }
      );

      if (response.ok) {
        console.log(`Post with id ${props.post.id} deleted successfully.`);
        // Dodajte logiku za ažuriranje stanja nakon brisanja
        // Na primer, ponovno učitajte postove
      } else {
        console.error(`Error deleting post with id ${props.post.id}.`);
        console.log(props.post.id);
      }
    } catch (error) {
      console.error("Error deleting post:", error);
    }
    window.location.reload();
  };

  const handleUpdate = async () => {
    const updatedContent = prompt("Enter updated content:", props.post.content);

    if (updatedContent !== null) {
      const updatedPost = {
        id: props.post.id,
        content: updatedContent,
      };

      try {
        const response = await fetch(`https://localhost:7082/Post/UpdatePost`, {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(updatedPost),
        });

        if (response.ok) {
          console.log(`Post with id ${props.post.id} updated successfully.`);
          // Dodajte logiku za ažuriranje stanja nakon ažuriranja posta
          // Na primer, ponovno učitajte postove
        } else {
          console.error(`Error updating post with id ${props.post.id}.`);
        }
      } catch (error) {
        console.error("Error updating post:", error);
      }
    }
    window.location.reload();
  };

  return (
<div className="post-container">
  <div className="top-row">
    <div className="icon">
      <img src={props.post.author?.profilePicture} alt="Profile" />
    </div>
    <p className="username">{props.post.author?.username}</p>
  </div>
  <p className="content">{props.post.content}</p>
  <p className="datetime">
    {props.post.datum.toDateString()} {props.post.datum.toLocaleTimeString()}
  </p>
  <p className="like-counter">Likes: {props.post.likeCounter}</p>
  <div className="button-container">
    <button onClick={handleDelete}>Delete</button>
    <button onClick={handleUpdate}>Update</button>
  </div>
</div>

  );
};

export default OnePost;
