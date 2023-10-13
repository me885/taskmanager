import React from 'react';
import './App.css';
import {Routes, Route, BrowserRouter} from 'react-router-dom';
import Home from './pages/home';
import SignUp from './pages/signup';
import SignIn from './pages/signin';
import Navbar from './components/navbar/navbar';
 
function App() {
    return (
    <BrowserRouter>
    <Navbar />
    <Routes>
        <Route exact path='/' element={<Home />} />
        <Route path='/signup' element={<SignUp />} />
        <Route path='/signin' element={<SignIn />} />
    </Routes>
    </BrowserRouter>
    );
}
 
export default App;