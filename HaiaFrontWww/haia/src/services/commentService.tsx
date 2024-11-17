import axios, { AxiosError } from 'axios';

const API_URL = 'http://localhost:5117/api/Comment';

export interface Comment {
    id?: number; // Assuming id is optional for creation
    content: string; // Adjust the type based on actual structure
    // Add other properties as necessary
}

export const getAllComments = async (): Promise<Comment[]> => {
    try {
        const response = await axios.get<Comment[]>(API_URL);
        return response.data;
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error('Error fetching comments:', axiosError);
        throw axiosError;
    }
};

export const getCommentById = async (id: number): Promise<Comment> => {
    try {
        const response = await axios.get<Comment>(`${API_URL}/${id}`);
        return response.data;
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error(`Error fetching comment with id ${id}:`, axiosError);
        throw axiosError;
    }
};

export const createComment = async (comment: Comment): Promise<Comment> => {
    try {
        const response = await axios.post<Comment>(API_URL, comment);
        return response.data;
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error('Error creating comment:', axiosError);
        throw axiosError;
    }
};

export const generateJokeForComment = async (id: number): Promise<string> => {
    try {
        const response = await axios.post<string>(`${API_URL}/${id}/generate-joke`);
        return response.data;
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error(`Error generating joke for comment with id ${id}:`, axiosError);
        throw axiosError;
    }
};

export const getLikesCountForComment = async (id: number): Promise<number> => {
    try {
        const response = await axios.get<number>(`${API_URL}/${id}/likes`);
        return response.data;
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error(`Error fetching likes count for comment with id ${id}:`, axiosError);
        throw axiosError;
    }
};