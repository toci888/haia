// frontend/src/App.js
import React, { useState } from 'react';
import axios from 'axios';

function App() {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loginEmail, setLoginEmail] = useState('');
  const [loginPassword, setLoginPassword] = useState('');

  const handleRegister = async () => {
    try {
      await axios.post('http://localhost:3000/api/users/register', {
        username,
        email,
        password,
      });
      alert('Użytkownik zarejestrowany pomyślnie');
    } catch (error) {
      alert('Błąd rejestracji');
      console.error(error);
    }
  };

  const handleLogin = async () => {
    try {
      const response = await axios.post('http://localhost:3000/api/users/login', {
        email: loginEmail,
        password: loginPassword,
      });
      alert('Logowanie udane');
      console.log(response.data);
    } catch (error) {
      alert('Błędne dane logowania');
      console.error(error);
    }
  };

  return (
    <div>
      <h1>Rejestracja</h1>
      <input
        type="text"
        placeholder="Nazwa użytkownika"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      <input
        type="email"
        placeholder="E-mail"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      />
      <input
        type="password"
        placeholder="Hasło"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <button onClick={handleRegister}>Zarejestruj się</button>

      <h1>Logowanie</h1>
      <input
        type="email"
        placeholder="E-mail"
        value={loginEmail}
        onChange={(e) => setLoginEmail(e.target.value)}
      />
      <input
        type="password"
        placeholder="Hasło"
        value={loginPassword}
        onChange={(e) => setLoginPassword(e.target.value)}
      />
      <button onClick={handleLogin}>Zaloguj się</button>
    </div>
  );
}

export default App;
