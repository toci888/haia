import axios from 'axios';

const API_URL = 'http://localhost:5117/api/Like';

export const likeComment = async (like) => {
    try {
        const response = await axios.post(API_URL, like);
        return response.data;
    } catch (error) {
        console.error('Error liking comment:', error);
        throw error;
    }
};
