import React, { useEffect, useState } from "react";
import { Post } from "../models/post.model";
import "../App1.css"; // Dodaj CSS fajl
import LikeComponent from "./Like";
import { Comment } from "../models/comment.model";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPencilAlt, faTrash } from "@fortawesome/free-solid-svg-icons";


const OnePost = (props: { post: Post, userId: number}) => {
  const [isDropdownOpen, setDropdownOpen] = useState(false);
  const [newComment, setNewComment] = useState("");
  const [comments, setComments] = useState<Comment[]>([]);
  const [loadingComments, setLoadingComments] = useState(false);
  const [showAllComments, setShowAllComments] = useState(false);

  
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
  });



  


  const handleCommentUpdate = async (comment: Comment) => {
    const updatedContent = prompt("Izmeni komentar:", comment.commentContent);
  
    if (updatedContent !== null) {
      try {
        const response = await fetch(`https://localhost:7082/Comment/UpdateComment`, {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            id: comment.id,
            commentContent: updatedContent,
          }),
        });
  
        if (response.ok) {
          console.log(`Comment with id ${comment.id} updated successfully.`);
          fetchComments();
        } else {
          console.error(`Error updating comment with id ${comment.id}.`);
        }
      } catch (error) {
        console.error("Error updating comment:", error);
      }
    }
  };
  
  const handleCommentDelete = async (commentId:number) => {
    try {
      const response = await fetch(
        `https://localhost:7082/Comment/DeleteComment?comId=${encodeURIComponent(commentId)}`,
        {
          method: "DELETE",
        }
      );
  
      if (response.ok) {
        console.log(`Comment with id ${commentId} deleted successfully.`);
        // Assuming fetchComments is a function that fetches and updates the comments list
        fetchComments();
      } else {
        console.error(`Error deleting comment with id ${commentId}.`);
      }
    } catch (error) {
      console.error("Error deleting comment:", error);
    }
  };
  
  
  
  return (
    <div className="post-container">
      <div className="top-row">
        <div className="icon">
          <img src={props.post.author?.profilePicture} alt="Profilna slika" />
        </div>
        <p className="username">{props.post.author?.username}</p>
      </div>
      <p className="content">{props.post.content}</p>
      <p className="datetime">
        {props.post.datum.toDateString()} {props.post.datum.toLocaleTimeString()}
      </p>
      <div className="button-container">
        {showDropdown ? (
          <div className="dropdown">
            <div className="dropdown-toggle" onClick={handleDropdownClick}>
              Opcije
            </div>
            {isDropdownOpen && (
              <div className="dropdown-menu">
                <div onClick={() => handleDropdownOptionClick("Delete")}>Obriši</div>
                <div onClick={() => handleDropdownOptionClick("Update")}>Izmeni</div>
              </div>
            )}
          </div>
        ) : null}
      </div>
      <div className="LikeCommentDiv">
        <LikeComponent postId={props.post.id} userId={props.userId} />
        <p className="comments-toggle" onClick={() => setShowAllComments(!showAllComments)}>
          Komentari
        </p>
      </div>
      {/* Comments Section */}
      {showAllComments && (
        <div className="comments-container">
          {loadingComments ? (
            <p>Učitavanje komentara...</p>
          ) : (
            comments.map((comment) => (
              <div key={comment.id} className="comment">
                <div className="icon">
                  <img src={comment.user?.profilePicture} alt="Profilna slika" />
                </div>
                <p className="comment-author">{comment.user?.username}</p>
                <p className="comment-content">{comment.commentContent}</p>
                {comment.user?.id === props.userId && (
                  <div className="Izmena">
                    <button onClick={() => handleCommentUpdate(comment)}>
                      <FontAwesomeIcon icon={faPencilAlt} /> 
                    </button>
                    <button onClick={() => handleCommentDelete(comment.id)}>
                      <FontAwesomeIcon icon={faTrash} /> 
                    </button>
                  </div>
                )}
              </div>
            ))
          )}
        </div>
      )}

      {/* Add Comment Section */}
      {showAllComments && (
        <div className="add-comment-container">
          <textarea
            rows={3}
            placeholder="Dodaj komentar..."
            value={newComment}
            onChange={(e) => setNewComment(e.target.value)}
          />
          <button onClick={handleCommentSubmit}>Dodaj komentar</button>
        </div>
      )}
    </div>
  );

  
  
};

export default OnePost;