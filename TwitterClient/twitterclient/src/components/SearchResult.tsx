import { Link } from "react-router-dom";
import { User } from "../models/user.model";
import myimage from "./../assets/noProfilePicture.png"

const SearchResult = (props: { userId:number, result: User}) => {
  return (
    <div className="searchResult">
      <img className="searchResultImg" src={myimage} alt={props.result.username}/>
      <Link className="searchResultTitle" to={`Profile/${props.result.id}?userId=${props.userId}`}>{props.result.username}</Link>
    </div>
  );
};

export default SearchResult;