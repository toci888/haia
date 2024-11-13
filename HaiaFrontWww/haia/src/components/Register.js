import React, { useState } from 'react';
import { registerUser } from '../apiService';
import SocialLogin from './SocialLogin';
import './styles/Register.css';

const Register = () => {
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [message, setMessage] = useState('');

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
    );
};

export default Register;
