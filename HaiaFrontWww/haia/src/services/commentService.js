import axios from 'axios';

const API_URL = 'http://localhost:5117/api/Comment';

export const getAllComments = async () => {
    try {
        const response = await axios.get(API_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching comments:', error);
        throw error;
    }
};

export const getCommentById = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/${id}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching comment with id ${id}:`, error);
        throw error;
    }
};

export const createComment = async (comment) => {
    try {
        const response = await axios.post(API_URL, comment);
        return response.data;
    } catch (error) {
        console.error('Error creating comment:', error);
        throw error;
    }
};

export const generateJokeForComment = async (id) => {
    try {
        const response = await axios.post(`${API_URL}/${id}/generate-joke`);
        return response.data;
    } catch (error) {
        console.error(`Error generating joke for comment with id ${id}:`, error);
        throw error;
    }
};

export const getLikesCountForComment = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/${id}/likes`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching likes count for comment with id ${id}:`, error);
        throw error;
    }
};
