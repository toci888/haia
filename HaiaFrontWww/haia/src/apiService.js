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

export const registerUser = async (userData) => {
    return await axios.post(`${API_URL}/register`, userData);
};

export const socialLogin = async (provider, providerUserId) => {
    return await axios.post(`${API_URL}/social-login`, { provider, providerUserId });
};

export const getUserById = async (userId) => {
    return await axios.get(`${API_URL}/${userId}`);
};


export const addJoke = async (jokeData) => {
    return await axios.post(`${API_URL}/jokes`, jokeData);
};
