import React, { useState, useEffect } from 'react';
import { getUserProfile, getUserJokes } from '../apiService';
import './styles/UserProfile.css';

const UserProfile = ({ userId }) => {
    const [user, setUser] = useState(null);
    const [jokes, setJokes] = useState([]);
    const [message, setMessage] = useState('');

    useEffect(() => {
        fetchUserProfile();
        fetchUserJokes();
    }, [userId]);

    const fetchUserProfile = async () => {
        try {
            const userData = await getUserProfile(userId);
            setUser(userData);
        } catch (error) {
            setMessage("Błąd podczas pobierania danych użytkownika.");
            console.error(error);
        }
    };

    const fetchUserJokes = async () => {
        try {
            const jokesData = await getUserJokes(userId);
            setJokes(jokesData);
        } catch (error) {
            setMessage("Błąd podczas pobierania żartów użytkownika.");
            console.error(error);
        }
    };

    if (!user) {
        return <p>Ładowanie danych użytkownika...</p>;
    }

    return (
        <div className="user-profile-container">
            <h2>Profil Użytkownika</h2>
            {message && <p className="profile-message">{message}</p>}
            <div className="user-info">
                <p><strong>Username:</strong> {user.username}</p>
                <p><strong>Imię:</strong> {user.firstName}</p>
                <p><strong>Nazwisko:</strong> {user.lastName}</p>
                <p><strong>Email:</strong> {user.email}</p>
            </div>
            <h3>Żarty użytkownika</h3>
            <ul className="jokes-list">
                {jokes.length > 0 ? (
                    jokes.map((joke) => (
                        <li key={joke.id} className="joke-item">
                            <p>{joke.text}</p>
                            <span className="joke-date">{new Date(joke.createdAt).toLocaleString()}</span>
                        </li>
                    ))
                ) : (
                    <p>Brak żartów do wyświetlenia.</p>
                )}
            </ul>
        </div>
    );
};

export default UserProfile;
