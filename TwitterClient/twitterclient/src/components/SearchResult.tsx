import { Link } from "react-router-dom";
import { User } from "../models/user.model";
import myimage from "./../assets/noProfilePicture.png"

const SearchResult = (props: { userId:number, result: User, setResultList: (list: User[]) => void}) => {
  const setResultListToEmpty =() =>
  {
    props.setResultList([])
  }
  return (
    <div className="searchResult" onClick={setResultListToEmpty}>
      <img className="searchResultImg" src={myimage} alt={props.result.username}/>
      <Link className="searchResultTitle" to={`Profile/${props.result.id}?userId=${props.userId}`}>{props.result.username}</Link>
    </div>
  );
};

export default SearchResult;