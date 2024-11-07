import React, { useState } from 'react';
import { addJoke } from '../apiService';
import './JokeLandingPage.css';

const JokeLandingPage = () => {
    const [jokeText, setJokeText] = useState('');
    const [message, setMessage] = useState('');

    const handleAddJoke = async (e) => {
        e.preventDefault();

        try {
            const response = await addJoke({ text: jokeText, author: "warrior" });
            setMessage("Dodano dowcip!");
            setJokeText('');
        } catch (error) {
            setMessage("Błąd dodawania dowcipu.");
            console.error(error);
        }
    };

    return (
        <div className="landing-container">
            <h1>Witaj na Haia!</h1>
            <p>Podziel się swoim dowcipem i spraw, by inni się uśmiechnęli!</p>

            <div className="joke-form-container">
                <h2>Dodaj dowcip</h2>
                <form onSubmit={handleAddJoke}>
                    <textarea
                        value={jokeText}
                        onChange={(e) => setJokeText(e.target.value)}
                        placeholder="Wpisz swój dowcip tutaj..."
                        required
                    />
                    <button className="joke-form__button" type="submit">Dodaj Dowcip</button>
                </form>
                {message && <p className="message">{message}</p>}
            </div>
        </div>
    );
};

export default JokeLandingPage;
