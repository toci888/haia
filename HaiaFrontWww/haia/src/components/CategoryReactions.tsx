import React, { useEffect, useState } from 'react';
import { getCategories, reactToCategory, getTopCategoriesForUser } from '../apiService';

interface Category {
    id: number;
    name: string;
}

interface CategoryReactionsProps {
    userId: number; // Assuming userId is a number, adjust if needed
}

const CategoryReactions: React.FC<CategoryReactionsProps> = ({ userId }) => {
    const [categories, setCategories] = useState<Category[]>([]);
    const [topCategories, setTopCategories] = useState<Category[]>([]);

    useEffect(() => {
        fetchCategories();
        fetchTopCategories();
    }, [userId]);

    const fetchCategories = async () => {
        const data = await getCategories();
        setCategories(data);
    };

    const fetchTopCategories = async () => {
        const data = await getTopCategoriesForUser(userId);
        setTopCategories(data);
    };

    const handleReactToCategory = async (categoryId: number) => {
        await reactToCategory({ userId, categoryId, isPositive: true });
        fetchTopCategories();
    };

    return (
        <div>
            <h2>Reakcje na Kategorie Humoru</h2>
            <h3>Wszystkie Kategorie</h3>
            <ul>
                {categories.map((category) => (
                    <li key={category.id}>
                        {category.name}
                        <button onClick={() => handleReactToCategory(category.id)}>Lubię to!</button>
                    </li>
                ))}
            </ul>
            <h3>Ulubione Kategorie</h3>
            <ul>
                {topCategories.map((category) => (
                    <li key={category.id}>{category.name}</li>
                ))}
            </ul>
        </div>
    );
};

export default CategoryReactions;