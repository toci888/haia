import React from 'react';
import ComedyTextList from './components/ComedyTextList';
import UserList from './components/UserList';
import 'bootstrap/dist/css/bootstrap.min.css'; // Import Bootstrap CSS

function App() {
    return (
        <div className="container mt-4">
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
