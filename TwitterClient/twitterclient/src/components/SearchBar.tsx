// ... (previous imports)

import { useEffect, useState } from "react";
import { User } from "../models/user.model";

const SearchBar = (props: { username: string; setResults: (users: User[]) => void }) => {
    const [find, setSearch] = useState('');
  
    const searching = props.username;
  
    useEffect(() => {
      const search = async () => {
        if (find.trim() === '') {
          props.setResults([]);
          return;
        }
  
        //const response = await fetch(`https://localhost:44348/User/Search/${encodeURIComponent(find)}/${encodeURIComponent(searching)}`, {
        const response = await fetch(`https://localhost:7082/User/Search/${encodeURIComponent(find)}/${encodeURIComponent(searching)}`, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'credentials': 'include'
          },
        });
  
        const users: User[] = await response.json();
  
        if (response.ok) {
          props.setResults(users);
        }
      };
  
      search();
    }, [find, searching, props.setResults]); // Only include necessary dependencies
  
    const handleChange = (value: string) => {
      setSearch(value);
    }
  
    return (
      <div className="d-flex" role="search">
        <input className="form-control me-2" placeholder="Search" value={find} onChange={e => handleChange(e.target.value)} />
      </div>
    );
  };
  
  export default SearchBar;
  