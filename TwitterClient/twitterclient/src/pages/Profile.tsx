import { useParams } from "react-router-dom";
import { User } from "../models/user.model";
import { useEffect, useState } from "react";
import image from "./../assets/noProfilePicture.png"
import { FollowUnfollow } from "../models/followUnfollow.model";


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

    const followUnfollow = async () => {

      if (!profileUserId) {
        console.error('ProfileUserId is undefined');
        return;
      }

       var obj: FollowUnfollow = {
        followingId : props.loggedUserId,
        followedId : parseInt(profileUserId, 10),
        following : !userData?.checkFollowing
       }

       await fetch('https://localhost:7082' + '/FollowingList/ChangeState', {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({
          followingId : props.loggedUserId,
          followedId : parseInt(profileUserId, 10),
          following : !userData?.checkFollowing
        }),
        credentials: 'include',
        mode: 'cors'
      });
    }

    return (
        <div className="profile-main">
          <div className="profile-content">
            <div className="profile-info">
              <img className="profile-image" src={image} alt={userData?.username} />
              <div className="profile-personality">
                <div className="profile-details">
                  <div className="profile-username-btn">
                    <label className="profile-title">{userData?.username}</label>
                    <div onClick={followUnfollow}>
                      <button className={`profile-follow-btn ${userData?.checkFollowing ? 'following-btn' : 'follow-btn'}`}>{userData?.checkFollowing ? 'Following' : 'Follow'} </button>
                    </div>
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