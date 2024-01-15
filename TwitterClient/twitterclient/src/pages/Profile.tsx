import { useParams } from "react-router-dom";
import { User } from "../models/user.model";
import { useEffect, useState } from "react";
import image from "./../assets/noProfilePicture.png"


const Profile = () => {
    // Access parameters from the URL
    const { userId } = useParams();
    const [userData, setUserData] = useState<User>();
    const [error, setError] = useState('');
    
    const [following, setFollowing] = useState(true);
  
    useEffect(() => {
      const fetchUserData = async () => {
        if (userId === undefined) {
            setError('User ID is undefined');
            return;
        }

        const response = await fetch(`https://localhost:7082/User/Profile/${encodeURIComponent(userId)}`, {
            method: 'GET',
            headers: {
              'Content-Type': 'application/json',
              'credentials': 'include',
            },
          });
  
          if (!response.ok) {
            throw new Error('Failed to fetch user data');
          }
          
          const userData : User = await response.json();
          setUserData(userData);
          
      };
      fetchUserData();
    }, [userId]);

    return (
        <div className="profile-main">
          <div className="profile-content">
            <div className="profile-info">
              <img className="profile-image" src={image} alt={userData?.username} />
              <div className="profile-personality">
                <div className="profile-details">
                  <div className="profile-username-btn">
                    <label className="profile-title">{userData?.username}</label>
                    <button className={`profile-follow-btn ${following ? 'following-btn' : 'follow-btn'}`}>{following ? 'Following' : 'Follow'}</button>
                  </div>
                  <label className="profile-text">{userData?.name + ' ' + userData?.lastName}</label>
                </div>
                <div className="profile-followers-counter">
                  <div className="profile-counter">
                    <label className="profile-count-label">Following</label>
                    <label>{userData?.followingCount}</label>
                  </div>
                  <div className="profile-counter">
                    <label className="profile-count-label">Followers</label>
                    <label>{userData?.followedCount}</label>
                  </div>
                </div>
              </div>
            </div>
            <div className="profile-posts">

            </div>
          </div>
        </div>
    );
};

export default Profile;