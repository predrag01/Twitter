import { useState } from "react";
import { Link } from "react-router-dom";
import { User } from "../models/user.model";
import SearchBar from "./SearchBar";
import SearchResultList from "./SearchResultList";
import DropDownMenu from "./DropDownMenu";
import image from "./../assets/noProfilePicture.png"


const Nav = (props: {userId: number, username:string, setUsername: (username: string) => void}) => {

  const [searchResults, setSearchResults] = useState<User[]>([]);
  const [showMenu, setShowMenu] = useState(false)
  
  const showHideMenu = () => {
    setShowMenu(!showMenu)
  }

  const logout = async () => {
    //await fetch('https://localhost:44348' + '/User/Logout', {
      await fetch('https://localhost:7082' + '/User/Logout', {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include'
    });

    props.setUsername('');
  }

  let menu;

  if(props.username === undefined){
    menu = (
      <ul className="navbar-nav me-auto mb-2 mb-md-0">
        <li className="nav-item active">
          <Link className="nav-link" to={"Login"} >Login</Link>
        </li>
        <li className="nav-item active">
          <Link className="nav-link" to={"Register"}>Register</Link>
        </li>
      </ul>
    )
  } else {
    menu = (
      <div className="nav-menu">
        <div className="nav-img-username" onClick={showHideMenu}>
          <img className="nav-profile-image" src={image} alt={props.username} />
          <label className="nav-username">{props.username}</label>
        </div>
        {showMenu && <DropDownMenu setUsername={props.setUsername} userId={props.userId}/>}
      </div>
    )
  }

  return (
    <nav className="navbar navbar-expand-md navbar-dark bg-dark mb-4">
      <div className="container-fluid">
        <Link className="navbar-brand" to={"/"} >Twitter</Link>
        <div className="collapse navbar-collapse" id="navbarCollapse">
          <SearchBar username = {props.username} setResults={setSearchResults} />
          {searchResults.length > 0 && <SearchResultList results={searchResults} userId={props.userId}/>}
        </div>
        <div className="d-flex">
            {menu}
        </div>
      </div>
    </nav>
  );
};

export default Nav;