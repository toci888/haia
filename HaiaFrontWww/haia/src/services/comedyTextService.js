import axios from 'axios';

const API_URL = 'http://80.209.230.198:5117/api/ComedyText';

export const getAllComedyTexts = async () => {
    try {
        const response = await axios.get(API_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching comedy texts:', error);
        throw error;
    }
};

export const getComedyTextById = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/${id}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching comedy text with id ${id}:`, error);
        throw error;
    }
};

export const createComedyText = async (comedyText) => {
    try {
        const response = await axios.post(API_URL, comedyText);
        return response.data;
    } catch (error) {
        console.error('Error creating comedy text:', error);
        throw error;
    }
};

export const updateComedyText = async (id, comedyText) => {
    try {
        await axios.put(`${API_URL}/${id}`, comedyText);
    } catch (error) {
        console.error(`Error updating comedy text with id ${id}:`, error);
        throw error;
    }
};

export const deleteComedyText = async (id) => {
    try {
        await axios.delete(`${API_URL}/${id}`);
    } catch (error) {
        console.error(`Error deleting comedy text with id ${id}:`, error);
        throw error;
    }
};
