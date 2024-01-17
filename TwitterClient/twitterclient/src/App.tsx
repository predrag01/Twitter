import { BrowserRouter, Navigate, Route, Routes} from 'react-router-dom'
import './App.css'
import Nav from './components/Nav'
import Home from './pages/Home'
import Register from './pages/Register'
import Login from './pages/Login'
import { useEffect, useState } from 'react'
import Profile from './pages/Profile'
import Settings from './pages/Settings'
import Cookies from 'js-cookie';

function App() {
  const [username, setUserName] = useState('');
  const [userId, setUserId] = useState(-1);
  
  useEffect(() => {
    (
      async () => {
        try {
          const response = await fetch('https://localhost:7082/User/GetUser', {
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            mode: 'cors',
          });

          if (response.status !== 200) {
            <Navigate to="/Login" />;
            return;
          }

          const content = await response.json();
          setUserName(content.username);
          setUserId(content.id);
        } catch (error) {
          console.error('Error fetching user data:', error);
        }
      }
    )();
  }, []);

  return (
    <div className="App">
      <BrowserRouter>
        <Nav username={username} setUsername={setUserName} userId={userId} setUserId={setUserId}/>

        <main className='main'>
          <Routes>
            <Route path='/' element={<Home username={username} userId={userId}/>} />
            <Route path='/Login' element={<Login setUsername={setUserName} setUserId={setUserId}/>}/>
            <Route path='/Register' element={<Register />}/>
            <Route path='/Profile/:profileUserId' element={<Profile loggedUserId={userId}/>}/>
            <Route path='/Settings/:userId' element={<Settings userId={userId}/>}/>
          </Routes>
        </main>
      </BrowserRouter>
    </div>
  );
}

export default App
