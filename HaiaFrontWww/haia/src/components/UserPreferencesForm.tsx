import React, { useState, useEffect } from 'react';
import { getCategories, createUserPreference } from '../apiService';
import './styles/UserPreferencesForm.css';

interface Category {
    id: number;
    name: string;
}

interface UserPreferencesFormProps {
    userId: number;
}

const UserPreferencesForm: React.FC<UserPreferencesFormProps> = ({ userId }) => {
    const [categories, setCategories] = useState<Category[]>([]);
    const [preferences, setPreferences] = useState<Record<number, number>>({});
    const [message, setMessage] = useState<string>('');

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const data = await getCategories();
                setCategories(data);
                const initialPreferences = data.reduce((acc: Record<number, number>, category: Category) => {
                    acc[category.id] = 0; // Set default preference level to 0 for each category
                    return acc;
                }, {});
                setPreferences(initialPreferences);
            } catch (error) {
                console.error("Error fetching categories", error);
                setMessage("Failed to fetch categories.");
            }
        };
        fetchCategories();
    }, []);

    // Handle star click to set preference level
    const handleStarClick = (categoryId: number, level: number) => {
        setPreferences(prevPreferences => ({
            ...prevPreferences,
            [categoryId]: level
        }));
    };

    // Handle form submission
    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        try {
            for (const [categoryId, preferenceLevel] of Object.entries(preferences)) {
                if (preferenceLevel > 0) { // Only for selected preferences
                    await createUserPreference({
                        userId,
                        categoryId: parseInt(categoryId),
                        preferenceLevel
                    });
                }
            }
            setMessage("Preferences have been saved!");
        } catch (error) {
            console.error("Error saving preferences", error);
            setMessage("Failed to save preferences.");
        }
    };

    return (
        <div className="preferences-form">
            <h2>User Preferences</h2>
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
                <button type="submit">Save Preferences</button>
            </form>
        </div>
    );
};

export default UserPreferencesForm;