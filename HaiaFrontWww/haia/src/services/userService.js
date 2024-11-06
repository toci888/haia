import axios from 'axios';

const API_URL = 'http://80.209.230.198:5117/api/User';

export const getAllUsers = async () => {
    try {
        const response = await axios.get(API_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching users:', error);
        throw error;
    }
};

export const createUser = async (user) => {
    try {
        const response = await axios.post(API_URL, user);
        return response.data;
    } catch (error) {
        console.error('Error creating user:', error);
        throw error;
    }
};
