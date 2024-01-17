import { useEffect, useRef, useState } from "react";
import { Link, redirect } from "react-router-dom";
import { User } from "../models/user.model";
import SearchBar from "./SearchBar";
import SearchResultList from "./SearchResultList";
import DropDownMenu from "./DropDownMenu";
import image from "./../assets/noProfilePicture.png"


const Nav = (props: {userId: number, username:string, setUsername: (username: string) => void, setUserId: (userId: number) => void}) => {

  const [searchResults, setSearchResults] = useState<User[]>([]);
  const [showMenu, setShowMenu] = useState(false);
  
  const menuRef = useRef<HTMLDivElement>(null);
  const searchResultsRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setShowMenu(false);
      }

      if (searchResultsRef.current && !searchResultsRef.current.contains(event.target as Node)) {
        setSearchResults([]);
      }

      
    };

    document.addEventListener("click", handleClickOutside);

    return () => {
      document.removeEventListener("click", handleClickOutside);
    };
  }, []);

  const showHideMenu = () => {
    setShowMenu(!showMenu);
  };
  let menu;

  if(props.userId === -1){
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
      <div className="nav-menu" ref={menuRef}>
        <div className="nav-img-username" onClick={showHideMenu}>
          <img className="nav-profile-image" src={image} alt={props.username} />
          <label className="nav-username">{props.username}</label>
        </div>
        {showMenu && <DropDownMenu setUsername={props.setUsername} userId={props.userId} setUserId={props.setUserId} closeMenu={setShowMenu}/>}
      </div>
    )
  }

  return (
    <nav className="navbar navbar-expand-md navbar-dark bg-dark mb-4">
      <div className="container-fluid">
        <Link className="navbar-brand" to={ props.userId === -1 ? "/Login" : "/"} >Twitter</Link>
        <div className="collapse navbar-collapse" id="navbarCollapse">
          { props.userId !== -1 && <SearchBar username = {props.username} setResults={setSearchResults}/>}
          {props.userId !== -1 && searchResults.length > 0  && (
            <div ref={searchResultsRef}>
              <SearchResultList results={searchResults} userId={props.userId} setResultList={setSearchResults}/>
            </div>
          )}
        </div>
        <div className="d-flex">
            {menu}
        </div>
      </div>
    </nav>
  );
};

export default Nav;