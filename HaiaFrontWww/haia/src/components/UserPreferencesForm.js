import React, { useState, useEffect } from 'react';
import { getCategories, createUserPreference } from '../apiService';
import './styles/UserPreferencesForm.css';

const UserPreferencesForm = ({ userId }) => {
    const [categories, setCategories] = useState([]);
    const [preferences, setPreferences] = useState({});
    const [message, setMessage] = useState('');

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const data = await getCategories();
                setCategories(data);
                const initialPreferences = data.reduce((acc, category) => {
                    acc[category.id] = 0; // Ustaw domyślny poziom preferencji na 0 dla każdej kategorii
                    return acc;
                }, {});
                setPreferences(initialPreferences);
            } catch (error) {
                console.error("Błąd podczas pobierania kategorii", error);
                setMessage("Nie udało się pobrać kategorii.");
            }
        };

        fetchCategories();
    }, []);

    // Obsługa kliknięcia gwiazdki do ustawienia poziomu preferencji
    const handleStarClick = (categoryId, level) => {
        setPreferences(prevPreferences => ({
            ...prevPreferences,
            [categoryId]: level
        }));
    };

    // Obsługa wysłania formularza
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            for (const [categoryId, preferenceLevel] of Object.entries(preferences)) {
                if (preferenceLevel > 0) { // Tylko dla wybranych preferencji
                    await createUserPreference({
                        userId,
                        categoryId: parseInt(categoryId),
                        preferenceLevel
                    });
                }
            }
            setMessage("Preferencje zostały zapisane!");
        } catch (error) {
            console.error("Błąd podczas zapisywania preferencji", error);
            setMessage("Nie udało się zapisać preferencji.");
        }
    };

    return (
        <div className="preferences-form">
            <h2>Preferencje Użytkownika</h2>
            {message && <p>{message}</p>}
            <form onSubmit={handleSubmit}>
                {categories.map(category => (
                    <div key={category.id} className="category-rating">
                        <label>{category.name}</label>
                        <div className="stars">
                            {[...Array(10)].map((_, index) => {
                                const level = index + 1;
                                return (
                                    <span
                                        key={level}
                                        className={`star ${level <= preferences[category.id] ? 'selected' : ''}`}
                                        onClick={() => handleStarClick(category.id, level)}
                                    >
                                        ★
                                    </span>
                                );
                            })}
                        </div>
                    </div>
                ))}
                <button type="submit">Zapisz Preferencje</button>
            </form>
        </div>
    );
};

export default UserPreferencesForm;
