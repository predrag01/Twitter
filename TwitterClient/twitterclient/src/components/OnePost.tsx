import React, { useEffect, useState } from "react";
import { Post } from "../models/post.model";
import "../App1.css"; // Dodaj CSS fajl
import LikeComponent from "./Like";
import { Comment } from "../models/comment.model";

const OnePost = (props: { post: Post, userId: number}) => {
  const [isDropdownOpen, setDropdownOpen] = useState(false);
  const [newComment, setNewComment] = useState("");
  const [comments, setComments] = useState<Comment[]>([]);
  const [loadingComments, setLoadingComments] = useState(false);

  
  const handleDropdownClick = () => {
    setDropdownOpen(!isDropdownOpen);
  };

  const handleDropdownOptionClick = (option: string) => {
    setDropdownOpen(false);

    if (props.userId === props.post.author?.id) {
      if (option === "Delete") {
        handleDelete();
      } else if (option === "Update") {
        handleUpdate();
      }
    } else {
      console.log("Unauthorized action. You can only update or delete your own posts.");
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
        } else {
          console.error(`Error updating post with id ${props.post.id}.`);
        }
      } catch (error) {
        console.error("Error updating post:", error);
      }
    }
    window.location.reload();
  };
  const showDropdown = props.userId === props.post.author?.id;





  const fetchComments = async () => {
    try {
      setLoadingComments(true);
      const response = await fetch(
        `https://localhost:7082/Comment/GetCommentByPostId/${props.post.id}`
      );
      if (response.ok) {
        const commentsData = await response.json();
        setComments(commentsData);
      } else {
        console.error("Greška prilikom dohvatanja komentara.");
      }
    } catch (error) {
      console.error("Greška prilikom dohvatanja komentara:", error);
    } finally {
      setLoadingComments(false);
    }
  };

  const handleCommentSubmit = async () => {
    if (newComment.trim() === "") {
      console.log("Komentar ne može biti prazan.");
      return;
    }

    const commentData = {
      userId: props.userId,
      postId: props.post.id,
      commentContent: newComment,
    };

    try {
      const response = await fetch("https://localhost:7082/Comment/CreateCom", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(commentData),
      });

      if (response.ok) {
        console.log("Komentar uspešno dodat.");
        fetchComments();
        setNewComment("");
      } else {
        console.error("Greška prilikom dodavanja komentara.");
      }
    } catch (error) {
      console.error("Greška prilikom dodavanja komentara:", error);
    }
  };

  useEffect(() => {
    fetchComments();
  }, []);


  
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
      <LikeComponent postId={props.post.id} userId={props.userId} />
      <div className="button-container">
        {showDropdown ? (
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
        ) : null}
      </div>
  
      {/* Prikaz komentara */}
      <div className="comments-container">
        <h3>Komentari</h3>
        {loadingComments ? (
          <p>Učitavanje komentara...</p>
        ) : (
          comments.map((comment) => (
            <div key={comment.id} className="comment">
              <p className="comment-author">{comment.user?.username}</p>
              <p className="comment-content">{comment.commentContent}</p>
            </div>
          ))
        )}
      </div>
  
      {/* Dodavanje novog komentara */}
      <div className="add-comment-container">
        <textarea
          rows={3}
          placeholder="Dodaj komentar..."
          value={newComment}
          onChange={(e) => setNewComment(e.target.value)}
        />
        <button onClick={handleCommentSubmit}>Dodaj komentar</button>
      </div>
    </div>
  );
};

export default OnePost;
