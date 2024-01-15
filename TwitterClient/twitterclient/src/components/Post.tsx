import React, { useEffect, useState } from 'react';

interface Post {
  id: number;
  content: string;
  dateTime: Date;
  authorId: number;
  likeCounter: number;
}

const Post: React.FC = () => {
  const [posts, setPosts] = useState<Post[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const getAllPosts = async () => {
      try {
        const response = await fetch('https://localhost:7082/Post/GetAllPosts');

        if (response.ok) {
          const data = await response.json();
          setPosts(data);
        } else {
          setError('Failed to fetch posts.');
        }
      } catch (error) {
        setError('An error occurred while fetching posts.');
      } finally {
        setLoading(false);
      }
    };

    getAllPosts();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div>
      <h1>All Posts</h1>
      <ul>
        {posts.map((post) => (
          <li key={post.id}>
            <p>{post.content}</p>
            <p>{post.authorId}</p>
            <p>Likes: {post.likeCounter}</p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Post;
