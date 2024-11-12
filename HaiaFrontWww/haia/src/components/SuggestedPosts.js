import React, { useEffect, useState } from 'react';
import axios from 'axios';
import PostList from './PostList';

const SuggestedPosts = () => {
  const [posts, setPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchSuggestedPosts = async () => {
      try {
        setLoading(true);
        setError(null);

        // Wykonanie zapytania GET do API
        const response = await axios.get('http://80.209.230.198:5117/api/PostInteraction/suggested/1');
        
        // Zaktualizowanie stanu o dane postów
        setPosts(response.data);
      } catch (err) {
        // Obsługa błędu
        setError('Błąd podczas pobierania sugerowanych postów');
      } finally {
        setLoading(false);
      }
    };

    fetchSuggestedPosts();
  }, []); // Pusty array oznacza, że useEffect uruchomi się tylko raz przy montowaniu komponentu

  // Renderowanie widoku na podstawie stanu pobierania i błędów
  if (loading) return <p>Ładowanie postów...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <h2>Sugerowane Posty</h2>
      <PostList posts={posts} />
    </div>
  );
};

export default SuggestedPosts;
