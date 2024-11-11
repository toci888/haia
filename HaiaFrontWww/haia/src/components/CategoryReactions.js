import React, { useEffect, useState } from 'react';
import { getCategories, reactToCategory, getTopCategoriesForUser } from './api';

const CategoryReactions = ({ userId }) => {
  const [categories, setCategories] = useState([]);
  const [topCategories, setTopCategories] = useState([]);

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

  const handleReactToCategory = async (categoryId) => {
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
