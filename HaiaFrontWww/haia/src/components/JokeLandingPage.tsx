import React, { useState, useEffect } from 'react';
import { addJoke, getJokes, getCategories } from '../apiService';
import './JokeLandingPage.css';
import JokeListGoovno from './JokeListGoovno';

interface Category {
    id: number;
    name: string;
}

interface Joke {
    categoryId: number;
    text: string;
    author: string;
    userId: number;
    user: {
        id: number;
        username: string;
        email: string;
    };
}

const JokeLandingPage: React.FC = () => {
    const [categories, setCategories] = useState<Category[]>([]);
    const [jokeText, setJokeText] = useState<string>('');
    const [message, setMessage] = useState<string>('');
    const [jokesHook, setJokes] = useState<Joke[]>([]);

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const data = await getCategories();
                setCategories(data);
            } catch (error) {
                console.error("Error fetching categories", error);
            }
        };
        
        const handleGetJokes = async () => {
            try {
                const jokes = await getJokes();
                setJokes(jokes.data);
            } catch (error) {
                console.error("Error fetching jokes", error);
            }
        };

        fetchCategories();
        handleGetJokes();
    }, []);

    const fillCategoryCombo = () => {
        const selectElement = document.getElementById('category-select') as HTMLSelectElement;
        if (selectElement) {
            for (const category of categories) {
                const option = document.createElement('option');
                option.value = category.id.toString();
                option.textContent = category.name;
                selectElement.appendChild(option);
            }
        }
    };

    const handleAddJoke = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        try {
            const categoryId = (document.getElementById('category-select') as HTMLSelectElement).value;
            const response = await addJoke({
                categoryId: Number(categoryId),
                text: jokeText,
                author: "ghostrider",
                userId: 1,
                user: {
                    id: 1,
                    username: "warrior",
                    email: "string"
                }
            });
            setMessage("Joke added!");
            setJokeText('');
        } catch (error) {
            setMessage("Error adding joke.");
            console.error(error);
        }
    };

    if (!document.getElementById('category-select')?.hasChildNodes()) {
        fillCategoryCombo();
    }

    return (
        <div>
            <div className="landing-container">
                <h1>Welcome to Haia!</h1>
                <p>Share your joke and make others smile!</p>
                <div className="joke-form-container">
                    <h2>Add a Joke</h2>
                    <form onSubmit={handleAddJoke}>
                        <label htmlFor="category-select">Select a category:</label>
                        <input type="hidden" id="CategoryId" />
                        <select id="category-select" name="category" onChange={() => document.getElementById('CategoryId')!.value = '7'}>
                        </select>
                        <textarea
                            value={jokeText}
                            onChange={(e) => setJokeText(e.target.value)}
                            placeholder="Type your joke here..."
                            required
                        />
                        <button type="submit">Add Joke</button>
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