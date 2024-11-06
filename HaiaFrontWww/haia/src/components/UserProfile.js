import React, { useState } from 'react';
import { getUserById } from '../apiService';

const UserProfile = () => {
    const [userId, setUserId] = useState('');
    const [userData, setUserData] = useState(null);

    const handleGetUser = async () => {
        try {
            const response = await getUserById(userId);
            setUserData(response.data);
        } catch (error) {
            console.error("Błąd pobierania danych użytkownika", error);
        }
    };

    return (
        <div>
            <h2>Profil Użytkownika</h2>
            <input
                type="text"
                placeholder="Wprowadź ID użytkownika"
                value={userId}
                onChange={(e) => setUserId(e.target.value)}
            />
            <button onClick={handleGetUser}>Pokaż Profil</button>

            {userData && (
                <div>
                    <p>ID: {userData.id}</p>
                    <p>Username: {userData.username}</p>
                    <p>Email: {userData.email}</p>
                    <h3>Połączone logowania:</h3>
                    <ul>
                        {userData.socialLogins.map((login, index) => (
                            <li key={index}>{login.provider}: {login.providerUserId}</li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default UserProfile;
