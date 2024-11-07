import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import AboutUsPage from './pages/AboutUsPage';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.scss';

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
