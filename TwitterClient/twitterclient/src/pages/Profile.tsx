import { useParams } from "react-router-dom";
import { User } from "../models/user.model";
import { useEffect, useState } from "react";
import image from "./../assets/noProfilePicture.png"
import { FollowUnfollow } from "../models/followUnfollow.model";
import { Post } from "../models/post.model";
import OnePost from "../components/OnePost";


const Profile = ( props : {loggedUserId: number}) => {
    const { profileUserId } = useParams();
    const [userData, setUserData] = useState<User>();
    const [error, setError] = useState('');
    const [posts, setPosts] = useState<Post[]>([])
    const [noPost, setNoPost] = useState(false)
    
    const [followBtn, setFollowBtn] = useState(true);

    useEffect(() => {
      const fetchUserData = async () => {
        if (profileUserId === undefined ) {
            setError('User ID is undefined');
            return;
        }

        if(props.loggedUserId === parseInt(profileUserId, 10)){
          setFollowBtn(false)
        }
        else{
          setFollowBtn(true)
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

        const postsResponde = await fetch(`https://localhost:7082/Post/GetPostByAuthorId/${encodeURIComponent(profileUserId)}`, {
            method: 'GET',
            headers: {
              'Content-Type': 'application/json',
              'credentials': 'include',
            },
        });

        if (!postsResponde.ok) {
          throw new Error('Failed to fetch user data');
        }
        const postData = await postsResponde.json();

        postData.forEach((post: Post) => {
          const aa = post.posted.split("T");
          post.datum = new Date(aa[0]);
        });
        setPosts(postData);
        setNoPost(postData.length === 0);
        

      };
      fetchUserData();
    }, [profileUserId]);

    const followUnfollow = async () => {

      if (!profileUserId) {
        console.error('ProfileUserId is undefined');
        return;
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
      
      setUserData((userData) => ({
        ...userData,
        checkFollowing: !userData?.checkFollowing,
      }));
      
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
                    {followBtn && (
                        <div onClick={followUnfollow}>
                          <button className={`profile-follow-btn ${userData?.checkFollowing ? 'following-btn' : 'follow-btn'}`}>{userData?.checkFollowing ? 'Following' : 'Follow'} </button>
                        </div>
                    )}                    
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
              {noPost ?  <h3>No posts yet</h3> : (posts.map((post) => (
                <OnePost key={post.id} post={post} />
              )))}
            </div>
          </div>
        </div>
    );
};

export default Profile;