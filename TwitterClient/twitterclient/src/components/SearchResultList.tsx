import { User } from "../models/user.model";
import SearchResult from "./SearchResult";


const SearchResultList = (props: { userId:number, results: User[]}) => {
  
  return (
    <div className="result-list">
       {props.results.map((result, id) => {
        return <SearchResult result={result} key={id} userId={props.userId}/>
       })}
    </div>
  );
};

export default SearchResultList;