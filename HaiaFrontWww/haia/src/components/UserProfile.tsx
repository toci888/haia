import React, { useState, useEffect } from 'react';
import { getUserProfile, getUserJokes } from '../apiService';
import './styles/UserProfile.css';

interface User {
    username: string;
    firstName: string;
    lastName: string;
    email: string;
    // Add other user properties as needed
}

interface Joke {
    id: number;
    text: string;
    createdAt: string; // Assuming createdAt is a string (ISO date format)
}

interface UserProfileProps {
    userId: number; // Assuming userId is a number
}

const UserProfile: React.FC<UserProfileProps> = ({ userId }) => {
    const [user, setUser] = useState<User | null>(null);
    const [jokes, setJokes] = useState<Joke[]>([]);
    const [message, setMessage] = useState<string>('');

    useEffect(() => {
        fetchUserProfile();
        fetchUserJokes();
    }, [userId]);

    const fetchUserProfile = async () => {
        try {
            const userData = await getUserProfile(userId);
            setUser(userData);
        } catch (error) {
            setMessage("Error fetching user data.");
            console.error(error);
        }
    };

    const fetchUserJokes = async () => {
        try {
            const jokesData = await getUserJokes(userId);
            setJokes(jokesData);
        } catch (error) {
            setMessage("Error fetching user jokes.");
            console.error(error);
        }
    };

    if (!user) {
        return <p>Loading user data...</p>;
    }

    return (
        <div className="user-profile-container">
            <h2>User Profile</h2>
            {message && <p className="profile-message">{message}</p>}
            <div className="user-info">
                <p><strong>Username:</strong> {user.username}</p>
                <p><strong>First Name:</strong> {user.firstName}</p>
                <p><strong>Last Name:</strong> {user.lastName}</p>
                <p><strong>Email:</strong> {user.email}</p>
            </div>
            <h3>User Jokes</h3>
            <ul className="jokes-list">
                {jokes.length > 0 ? (
                    jokes.map((joke) => (
                        <li key={joke.id} className="joke-item">
                            <p>{joke.text}</p>
                            <span className="joke-date">{new Date(joke.createdAt).toLocaleString()}</span>
                        </li>
                    ))
                ) : (
                    <p>No jokes to display.</p>
                )}
            </ul>
        </div>
    );
};

export default UserProfile;