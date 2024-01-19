import { useEffect, useState } from "react"
import { User } from "../models/user.model"
import { UserUpdate } from "../models/userUpdate.model";

const Settings = ( props: {setUsername: (username: string) => void, userId: number, }) => {

    const [user, setUser] = useState<User>();
    const [changePassword, setChangePassword] = useState(false);
    const [username, setUsername] = useState('')
    const [name, setName] = useState('')
    const [lastName, setLastName] = useState('')
    const [email, setEmail] = useState('')
    const [oldPass, setOldPass] = useState('')
    const [newPass, setNewPass] = useState('')

    useEffect(() => {
        const fetchUserData = async () => {
  
          const response = await fetch(`https://localhost:7082/User/Profile/${encodeURIComponent(props.userId)}/${encodeURIComponent(props.userId)}`, {
              method: 'GET',
              headers: {
                'Content-Type': 'application/json'},
              credentials: 'include'
            });
    
            if (!response.ok) {
              throw new Error('Failed to fetch user data');
            }
            
            const loaded : User = await response.json();
            if(loaded){
                setUser(loaded);
                setUsername(loaded.username || '');
                setName(loaded.name || '');
                setLastName(loaded.lastName || '');
                setEmail(loaded.email || '');
            }
        };
        fetchUserData();
      }, [props.userId]);

      const handleCheckBox = () => {
        setChangePassword(!changePassword)
    }

    const update = async () => {

        const updatedUser: UserUpdate = {
            id: user?.id,
            username: username,
            name: name,
            lastName: lastName,
            email: email,
            profilePicture: '',
            changePass: changePassword,
            oldPass: oldPass,
            newPass: newPass
          };

        const response = await fetch(`https://localhost:7082/User/UpdateProfile`, {
            method: 'PUT', 
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedUser),
            credentials: 'include',
            mode: 'cors'
       });
        props.setUsername(username);      
    }

    return(
        <div className="settings-main">
            <div className="settings-content">
                <div className="settings-title-div">
                    <label className="setting-title">Profile settings</label>
                </div>
                <div className="settings-row">
                    <input type="username" className="form-control settings-input" placeholder="Username" required value={username} onChange={(e) => setUsername(e.target.value)}/>
                </div>
                <div className="settings-row">
                    <input type="name" className="form-control settings-input" placeholder="Name" required value={name} onChange={(e) => setName(e.target.value)}/>
                    <input type="lastname" className="form-control settings-input" placeholder="Last Name" required value={lastName} onChange={(e) => setLastName(e.target.value)}/>
                </div>
                <div className="settings-row">
                    <input type="email" className="form-control settings-email" placeholder="name@example.com" required value={email} onChange={(e) => setEmail(e.target.value)}/>
                </div>
                <div className="settings-row">
                    <input type="checkbox" value="checkBoxValue" onChange={handleCheckBox}/>
                    <label className="settings-checkBox-lab">Change password</label>
                </div>
                <div className="settings-row">
                    <input type="password" className="form-control settings-input" placeholder="Old password" disabled={!changePassword} value={oldPass} onChange={(e) => setOldPass(e.target.value)}/>
                    <input type="password" className="form-control settings-input" placeholder="New password" disabled={!changePassword} value={newPass} onChange={(e) => setNewPass(e.target.value)}/>
                </div>
                <div className="settings-save">
                    <button className="btn btn-primary" onClick={update}>Save</button>
                </div>
            </div>
        </div>
    )
}

export default Settings