import React, { useState, useEffect } from 'react';
import { addJoke, getJokes } from '../apiService';
import './JokeLandingPage.css';

import  JokeListGoovno  from './JokeListGoovno';



console.log('ja pierdole');

const JokeLandingPage = () => {

    console.log('ja pierdole');

    const [jokeText, setJokeText] = useState('');
    const [message, setMessage] = useState('');
    const [jokesHook, setJokes] = useState([]);

     useEffect(() => {

        console.log('ja pierdole u e');

        const handlegetJokes = async (e) => {
            //e.preventDefault();
        
            const jokes = await getJokes();
        console.log(jokes.data, 'doopa');
        setJokes(jokes.data);
           // return jokes.data;
        }
        
        handlegetJokes();
      }, []);

    const handleAddJoke = async (e) => {
        e.preventDefault();

        try {
            const response = await addJoke({ "categoryId": 8,
  "text": jokeText,
  "author": "ghostrider",
  "userId": 1,
  "user": {
    "id": 1,
    "username": "warrior",
    "email": "string"
  } });
            setMessage("Dodano dowcip!");
            setJokeText('');
        } catch (error) {
            setMessage("Błąd dodawania dowcipu.");
            console.error(error);
        }
    };


    return (
        <div>
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
                    <button type="submit">Dodaj Dowcip</button>
                </form>
                {message && <p className="message">{message}</p>}
            </div>
        </div>

    <div>
        
    <JokeListGoovno jokes={jokesHook} />
        </div>
        </div>
    );
};

export default JokeLandingPage;
