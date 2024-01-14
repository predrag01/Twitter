import { useParams } from "react-router-dom";
import { User } from "../models/user.model";
import { useEffect, useState } from "react";
import image from "./../assets/noProfilePicture.png"


const Profile = () => {
    // Access parameters from the URL
    const { userId } = useParams();
    const [userData, setUserData] = useState<User>();
    const [error, setError] = useState('');
  
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
                    <img className="profile-image" src={image} alt={userData?.username}/>
                    <div className="profile-personal-info">
                        <h2>{userData?.username}</h2>
                        <label>{userData?.name + ' ' + userData?.lastName}</label>
                        <label>{userData?.name}</label>
                    </div>
                </div>
                <div className="profile-posts">

                </div>
            </div>
            
        </div>
    );
};

export default Profile;