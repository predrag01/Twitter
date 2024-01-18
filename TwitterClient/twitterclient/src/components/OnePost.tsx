import React, { useState } from "react";
import { Post } from "../models/post.model";
import "../App1.css"; // Dodaj CSS fajl
import LikeComponent from "./Like";

const OnePost = (props: { post: Post, userId: number}) => {
  const [isDropdownOpen, setDropdownOpen] = useState(false);

  const handleDropdownClick = () => {
    setDropdownOpen(!isDropdownOpen);
  };

  const handleDropdownOptionClick = (option: string) => {
    setDropdownOpen(false);
    if (option === "Delete") {
      handleDelete();
    } else if (option === "Update") {
      handleUpdate();
    }
  };

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
      <LikeComponent postId={props.post.id} userId={props.userId}/>
      <div className="button-container">
        <div className="dropdown">
          <div className="dropdown-toggle" onClick={handleDropdownClick}>
            Options
          </div>
          {isDropdownOpen && (
            <div className="dropdown-menu">
              <div onClick={() => handleDropdownOptionClick("Delete")}>Delete</div>
              <div onClick={() => handleDropdownOptionClick("Update")}>Update</div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default OnePost;
