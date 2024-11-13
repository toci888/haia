import ComedyTextList from './components/ComedyTextList';
import UserList from './components/UserList';
import 'bootstrap/dist/css/bootstrap.min.css'; // Import Bootstrap CSS
import Register from './components/Register';
import SocialLogin from './components/SocialLogin';
import UserProfile from './components/UserProfile';
import './App.css';
import JokeLandingPage from './components/JokeLandingPage';
import SuggestedPosts from './components/SuggestedPosts';
//import FriendInviteNotifier from './FriendInviteNotifier';

import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Navbar from './components/Navbar';


const App = () => {
  return (
    <Router>
      <div>
        <Navbar />
        <Routes>
          <Route path="/jokes" element={<JokeLandingPage />} />
          <Route path="/suggestedposts" element={<SuggestedPosts />} />
          
          <Route path="/register" element={<Register />} />
        </Routes>
      </div>
    </Router>
  );
};

export default App;


/*
function App() {
    return (
        
        <div className="container mt-4">
             <div className="container">
            <h1>Portal Haia</h1>
            <Register />
            <hr />
            <SocialLogin />
            <hr />
            <UserProfile />
        </div>

        <div className="App">
            <JokeLandingPage />
        </div>

        <div>
      <h1>Portal HAIA</h1>
      <SuggestedPosts />
    </div>


            <h1 className="text-center mb-4">Comedy App</h1>
            <div className="row">
                <div className="col-md-6">
                    <div className="card p-3 mb-4">
                        <h2 className="text-primary">Comedy Texts</h2>
                        <ComedyTextList />
                    </div>
                </div>
                <div className="col-md-6">
                    <div className="card p-3 mb-4">
                        <h2 className="text-success">Users</h2>
                        <UserList />
                    </div>
                </div>
            </div>
        </div>
    );
}

export default App;
*/