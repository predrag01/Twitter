import { Link } from "react-router-dom";
import Profile from "../pages/Profile";

const DropDownMenu = (props: {userId:number, setUsername: (username: string) => void, setUserId: (userId: number) => void}) => {

    const logout = async () => {
          await fetch('https://localhost:7082' + '/User/Logout', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            credentials: 'include'
        });
    
        props.setUsername('');
        props.setUserId(-1)
        console.log(props.userId)
      }

    return (
        <div className="drop-down-menu">
            <Link to={ props.userId === -1 ? "/Login" : `Profile/${props.userId}?userId=${props.userId}`}>Profile</Link>
            <Link to={ props.userId === -1 ? "/Login" : `Settings/${props.userId}`}>Settings</Link>
            <Link to={"Login"} onClick={logout}>Logout</Link>
        </div>
    )
}

export default DropDownMenu;