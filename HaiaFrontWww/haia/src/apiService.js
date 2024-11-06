import axios from 'axios';

const API_URL = 'http://80.209.230.198:5117/api';

export const addComment = async (jokeId, commentText) => {
    const response = await axios.post(`${API_URL}/comments`, {
        jokeId,
        text: commentText
    });
    return response.data;
};

export const reactToJoke = async (jokeId, reactionType, userId) => {
    const response = await axios.post(`${API_URL}/jokes/${jokeId}/react`, {
        reactionType,
        userId
    });
    return response.data;
};

export const reactToComment = async (commentId, reactionType, userId) => {
    const response = await axios.post(`${API_URL}/comments/${commentId}/react`, {
        reactionType,
        userId
    });
    return response.data;
};
