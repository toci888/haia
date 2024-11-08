import React from 'react';
import ComedyTextList from './components/ComedyTextList';
import UserList from './components/UserList';
import 'bootstrap/dist/css/bootstrap.min.css'; // Import Bootstrap CSS
import Register from './components/Register';
import SocialLogin from './components/SocialLogin';
import UserProfile from './components/UserProfile';
import './App.css';
import JokeLandingPage from './components/JokeLandingPage';
import FriendInviteNotifier from './FriendInviteNotifier';

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

        <FriendInviteNotifier/>

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
