// Home.tsx
import React, { useEffect, useState } from 'react';
import AddPost from './AddPost';
import ShowAllPosts from './ShowAllPosts';
import { Navigate } from 'react-router-dom';

const Home = (props: { username: string, userId: number }) => {
  return (
    <div className="home-div">
      <div className="home">
        <AddPost currentUserId={props.userId} /> {/* Dodajte trenutni userId kao prop */}
        {/*props.username ? 'Hi ' + props.username : 'You are not logged in'*/}
        <ShowAllPosts />
      </div>
    </div>
  );
};

export default Home;
