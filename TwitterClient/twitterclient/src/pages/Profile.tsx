import { useParams } from "react-router-dom";
import { User } from "../models/user.model";
import { useEffect, useState } from "react";
import image from "./../assets/noProfilePicture.png"


const Profile = ( props : {loggedUserId: number}) => {
    // Access parameters from the URL
    const { profileUserId } = useParams();
    const [userData, setUserData] = useState<User>();
    const [error, setError] = useState('');
    
    const [following, setFollowing] = useState(false);

  
    useEffect(() => {
      const fetchUserData = async () => {
        if (profileUserId === undefined ) {
            setError('User ID is undefined');
            return;
        }

        const response = await fetch(`https://localhost:7082/User/Profile/${encodeURIComponent(profileUserId)}/${encodeURIComponent(props.loggedUserId)}`, {
            method: 'GET',
            headers: {
              'Content-Type': 'application/json',
              'credentials': 'include',
            },
          });
  
          if (!response.ok) {
            throw new Error('Failed to fetch user data');
          }
          
          const user : User = await response.json();
          if(user)
          setUserData(user);
        console.log(user?.checkFollowing)
          
      };
      fetchUserData();
    }, [profileUserId]);

    return (
        <div className="profile-main">
          <div className="profile-content">
            <div className="profile-info">
              <img className="profile-image" src={image} alt={userData?.username} />
              <div className="profile-personality">
                <div className="profile-details">
                  <div className="profile-username-btn">
                    <label className="profile-title">{userData?.username}</label>
                    <button className={`profile-follow-btn ${userData?.checkFollowing ? 'following-btn' : 'follow-btn'}`}>{userData?.checkFollowing ? 'Following' : 'Follow'}</button>
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