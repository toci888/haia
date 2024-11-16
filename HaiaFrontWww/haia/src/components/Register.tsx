import React, { useState } from 'react';
import { registerUser, loginUser } from '../apiService';

import SocialLogin from './SocialLogin';
import './styles/Register.css';
import UserPreferencesForm from './UserPreferencesForm';

    

const Register = () => {
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [message, setMessage] = useState('');



    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await loginUser({ email, password });
            setMessage(`Zalogowano pomyślnie: ${response.data.username}`);
        } catch (error) {
            setMessage("Błąd logowania. Sprawdź dane i spróbuj ponownie.");
            console.error(error);
        }
    };

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            const response = await registerUser({ email, username, password });
            setMessage(`Rejestracja powiodła się: ${response.data.username}`);
        } catch (error) {
            setMessage("Błąd rejestracji");
            console.error(error);
        }
    };

    return (
        <div>
            { !message && <UserPreferencesForm userId="34" /> }
        <div className="login-container">
        <h2>Logowanie</h2>
        { !message && <form onSubmit={handleLogin} className="login-form">
            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
            />
            <input
                type="password"
                placeholder="Hasło"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
            />
            <button type="submit">Zaloguj się</button>
        </form> }
        {message && <p className="login-message">{message}</p>}
    </div>

        <div className="register-container">
            <h2>Rejestracja</h2>
            <form onSubmit={handleRegister} className="register-form">
                <input
                    type="text"
                    placeholder="Nazwa użytkownika"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    required
                />
                <input
                    type="email"
                    placeholder="Email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Hasło"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <button type="submit">Zarejestruj się</button>
            </form>
            {message && <p className="register-message">{message}</p>}

            <SocialLogin />
        </div>
        </div>
    );
};

export default Register;
