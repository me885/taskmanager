import React from 'react';
import './App.css';
import {Routes, Route, BrowserRouter} from 'react-router-dom';
import Home from './pages/home';
import SignUp from './pages/signup';
import SignIn from './pages/signin';
import UserContext from './UserContext';
import Navbar from './components/navbar/navbar';
import { useState } from 'react';
 
function App() {
    const [isLoggedIn, setLoggedInState] = useState(false)

    return (
    <BrowserRouter>
    <UserContext.Provider value={{isLoggedIn, setLoggedInState, username: null}}>
    <Navbar />
    <Routes>
        <Route exact path='/' element={<Home />} />
        <Route path='/signup' element={<SignUp />} />
        <Route path='/signin' element={<SignIn />} />
    </Routes>
    </UserContext.Provider>
    </BrowserRouter>
    );
}
 
export default App;