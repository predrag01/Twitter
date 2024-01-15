// NewPage.tsx
import React, { useEffect, useState } from "react";
import OnePost from "../components/OnePost";
import { Post } from "../models/post.model";

const NewPage = (props: {}) => {
  const [posts, setPosts] = useState<Post[]>([]);

  useEffect(() => {
    const fetchPosts = async () => {
      try {
        const response = await fetch("https://localhost:7082/Post/GetAllPosts");
        const postData = await response.json();
        console.log(postData);

        postData.forEach((post: Post) => {
          const bomboclaat = post.posted.split("T");
          post.datum = new Date(bomboclaat[0]);
        });
        setPosts(postData);
      } catch (error) {
        console.error("Error fetching posts:", error);
      }
    };

    fetchPosts();
  }, []);

  return (
    <div className="newpagediv">
      <h2>Latest Posts</h2>
      {posts.map((post) => (
        <OnePost key={post.id} post={post} />
      ))}
    </div>
  );
};

export default NewPage;
