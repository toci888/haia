import React, { useEffect, useState } from 'react';
import axios from 'axios';
import PostList from './PostList';

interface Post {
    id: number; // Assuming post ID is a number
    content: string; // Assuming content is a string
    // Add other properties as needed
}

const SuggestedPosts: React.FC = () => {
    const [posts, setPosts] = useState<Post[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchSuggestedPosts = async () => {
            try {
                setLoading(true);
                setError(null);
                // Perform a GET request to the API
                const response = await axios.get<Post[]>('http://80.209.230.198:5117/api/PostInteraction/suggested/34');
                // Update state with post data
                setPosts(response.data);
            } catch (err) {
                // Handle error
                setError('Error fetching suggested posts');
            } finally {
                setLoading(false);
            }
        };
        fetchSuggestedPosts();
    }, []); // Empty array means useEffect will run only once when the component mounts

    // Render view based on loading state and errors
    if (loading) return <p>Loading posts...</p>;
    if (error) return <p>{error}</p>;

    return (
        <div>
            <h2>Suggested Posts</h2>
            <PostList posts={posts} />
        </div>
    );
};

export default SuggestedPosts;