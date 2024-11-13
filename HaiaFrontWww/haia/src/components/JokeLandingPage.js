import React, { useState, useEffect } from 'react';
import { addJoke, getJokes } from '../apiService';
import './JokeLandingPage.css';

import  JokeListGoovno  from './JokeListGoovno';
import { getCategories } from '../apiService';

const JokeLandingPage = () => {

    const [categories, setCategories] = useState([]);
    const [jokeText, setJokeText] = useState('');
    const [message, setMessage] = useState('');
    const [jokesHook, setJokes] = useState([]);

     useEffect(() => {

        const fetchCategories = async () => {
            try {
                const data = await getCategories();
                setCategories(data);
            } catch (error) {
                console.error("Błąd podczas pobierania kategorii", error);
            }
        };

        fetchCategories();

        const handlegetJokes = async (e) => {
            //e.preventDefault();
        
            const jokes = await getJokes();

        setJokes(jokes.data);
           // return jokes.data;
        }
        
        handlegetJokes();
      }, []);

      const fillCategoryCombo =  () =>  {

      const selectElement = document.getElementById('category-select');

        for (const category of categories) {
            const option = document.createElement('option');
            option.value = category.id;
            option.textContent = category.name;
            selectElement.appendChild(option);
    
            
        }
    }

    const handleAddJoke = async (e) => {
        e.preventDefault();

        try {

            const categoryId = document.getElementById('category-select').value;

            const response = await addJoke({ "categoryId": categoryId,
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

console.log(document.getElementById('category-select'));

    if (!document.getElementById('category-select')?.hasChildNodes())
    {
        fillCategoryCombo();   
    }

    return (
        <div>
        <div className="landing-container">
            <h1>Witaj na Haia!</h1>
            <p>Podziel się swoim dowcipem i spraw, by inni się uśmiechnęli!</p>

            <div className="joke-form-container">
                <h2>Dodaj dowcip</h2>
                <form onSubmit={handleAddJoke}>
                <label for="category-select">Wybierz kategorię:</label>
                <input type="hidden" id="CategoryId"></input>
                    <select id="category-select" name="category" onchange="document.getElementById('CategoryId').value = 7;">
                  
                       
                    </select>

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
