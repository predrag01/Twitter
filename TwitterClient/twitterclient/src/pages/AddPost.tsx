import React, { useState } from 'react';

const AddPost = ({ currentUserId }: { currentUserId: number }) => {
  const [content, setContent] = useState('');

  const handleContentChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    setContent(event.target.value);
  };

  const handleAddPost = async () => {
    try {
      const response = await fetch('https://localhost:7082/Post/Create', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ content, authorId: currentUserId }), // Dodajte trenutnog korisnika kao authorId
      });

      if (response.ok) {
        console.log('Post uspešno dodat!');
        setContent(''); // Opciono, čistimo textarea nakon uspešnog dodavanja
      } else {
        console.error('Nešto je pošlo po zlu prilikom dodavanja posta.');
      }
    } catch (error) {
      console.error('Greška prilikom slanja zahteva:', error);
    }
    window.location.reload();
  };

  return (
    <div className="add-post-container">
      <h2 className="add-post-title">Add New Post</h2>
      <textarea
        className="add-post-textarea"
        placeholder="Enter post text..."
        value={content}
        onChange={handleContentChange}
      />
      <button className="add-post-button" onClick={handleAddPost}>
        Add Post
      </button>
    </div>
  );
};

export default AddPost;
