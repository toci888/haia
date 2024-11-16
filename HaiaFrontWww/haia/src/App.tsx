import Register from './components/Register';
import UserProfile from './components/UserProfile';
import './App.css';
import JokeLandingPage from './components/JokeLandingPage';
import SuggestedPosts from './components/SuggestedPosts';
import Friends from './components/Friends';
import 'bootstrap/dist/css/bootstrap.min.css'; // Import Bootstrap CSS

//import FriendInviteNotifier from './FriendInviteNotifier';

import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Navbar from './components/Navbar';
import { ThemeProvider, Typography, createTheme } from '@mui/material';

const App = () => {
  const theme = createTheme({
    palette: {
      primary: {
        main: '#1976d2',
      },
    },
  });

  return (
    <ThemeProvider theme={createTheme()}>
      <Typography style={{ color: theme.palette.primary.main }}>
        <Router>
          <div>
            <Navbar />
            <Routes>
              <Route path="/jokes" element={<JokeLandingPage />} />
              <Route path="/suggestedposts" element={<SuggestedPosts />} />
              <Route path="/registerlogin" element={<Register />} />
              <Route path="/friends" element={<Friends userId={1}/>} />
              <Route path="/profile" element={<UserProfile userId={1}/>} />
            </Routes>
          </div>
        </Router>
      </Typography>
    </ThemeProvider>
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