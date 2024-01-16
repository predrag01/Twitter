import { Link } from "react-router-dom";
import Profile from "../pages/Profile";

const DropDownMenu = (props: {userId:number, setUsername: (username: string) => void}) => {

    const logout = async () => {
          await fetch('https://localhost:7082' + '/User/Logout', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            credentials: 'include'
        });
    
        props.setUsername('');
      }

    return (
        <div className="drop-down-menu">
            <Link to={`Profile/${props.userId}?userId=${props.userId}`}>Profile</Link>
            <Link to={`Settings/${props.userId}`}>Settings</Link>
            <Link to={"Login"} onClick={logout}>Logout</Link>
        </div>
    )
}

export default DropDownMenu;