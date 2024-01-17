import { User } from "../models/user.model";
import SearchResult from "./SearchResult";


const SearchResultList = (props: { userId:number, results: User[], setResultList: (list: User[]) => void}) => {
  
  return (
    <div className="result-list">
       {props.results.map((result, id) => {
        return <SearchResult result={result} key={id} userId={props.userId} setResultList={props.setResultList}/>
       })}
    </div>
  );
};

export default SearchResultList;