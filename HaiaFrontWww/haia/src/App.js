import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import AboutUsPage from './pages/AboutUsPage';
import './App.css';

import ComedyTextList from './components/ComedyTextList';
import UserList from './components/UserList';
import 'bootstrap/dist/css/bootstrap.min.css'; // Import Bootstrap CSS
import Register from './components/Register';
import SocialLogin from './components/SocialLogin';
import UserProfile from './components/UserProfile';
import JokeLandingPage from './components/JokeLandingPage';

function App() {
    return (
        <>
            <div>
                <BrowserRouter>
                    <Routes>
                        <Route path="/" element={<HomePage/>}/>
                        <Route path="/about" element={<AboutUsPage/>}/>
                        {/* <Route path="/community" element={<Community/>}/> */}
                        {/* <Route path="/contact" element={<Contact/>}/> */}
                        <Route path="/login" element={<LoginPage/>}/>
                        {/* <Route path="/signin" element={<SignIn/>}/> */}
                    </Routes>
                </BrowserRouter>
            </div>
        </>
    );
}

export default App;
