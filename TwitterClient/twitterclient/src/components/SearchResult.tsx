import { User } from "../models/user.model";
import myimage from "./../assets/noProfilePicture.png"

const SearchResult = (props: {result: User}) => {
  return (
    <div className="searchResult">
      <img className="searchResultImg" src={myimage} alt={props.result.username}/>
      <label className="searchResultTitle">{props.result.username}</label>
    </div>
  );
};

export default SearchResult;