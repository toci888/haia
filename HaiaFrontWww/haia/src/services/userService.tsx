import axios, { AxiosError } from 'axios';

const API_URL = 'http://80.209.230.198:5117/api/User';

export interface User {
    id?: number; // Assuming id is optional for creation
    name: string; // Adjust the type based on actual structure
    email: string; // Adjust the type based on actual structure
    // Add other properties as necessary
}

export const getAllUsers = async (): Promise<User[]> => {
    try {
        const response = await axios.get<User[]>(API_URL);
        return response.data;
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error('Error fetching users:', axiosError);
        throw axiosError;
    }
};

export const createUser = async (user: User): Promise<User> => {
    try {
        const response = await axios.post<User>(API_URL, user);
        return response.data;
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error('Error creating user:', axiosError);
        throw axiosError;
    }
};