import axios, { AxiosError } from 'axios';

const API_URL = 'http://80.209.230.198:5117/api/ComedyText';

export interface ComedyText {
    id?: number; // Assuming id is optional for creation
    content: string; // Adjust the type based on actual structure
    // Add other properties as necessary
}

export const getAllComedyTexts = async (): Promise<ComedyText[]> => {
    try {
        const response = await axios.get<ComedyText[]>(API_URL);
        return response.data;
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error('Error fetching comedy texts:', axiosError);
        throw axiosError;
    }
};

export const getComedyTextById = async (id: number): Promise<ComedyText> => {
    try {
        const response = await axios.get<ComedyText>(`${API_URL}/${id}`);
        return response.data;
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error(`Error fetching comedy text with id ${id}:`, axiosError);
        throw axiosError;
    }
};

export const createComedyText = async (comedyText: ComedyText): Promise<ComedyText> => {
    try {
        const response = await axios.post<ComedyText>(API_URL, comedyText);
        return response.data;
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error('Error creating comedy text:', axiosError);
        throw axiosError;
    }
};

export const updateComedyText = async (id: number, comedyText: ComedyText): Promise<void> => {
    try {
        await axios.put(`${API_URL}/${id}`, comedyText);
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error(`Error updating comedy text with id ${id}:`, axiosError);
        throw axiosError;
    }
};

export const deleteComedyText = async (id: number): Promise<void> => {
    try {
        await axios.delete(`${API_URL}/${id}`);
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error(`Error deleting comedy text with id ${id}:`, axiosError);
        throw axiosError;
    }
};