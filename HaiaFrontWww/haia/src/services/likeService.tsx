import axios, { AxiosError } from 'axios';

const API_URL = 'http://80.209.230.198:5117/api/Like';

export interface Like {
    commentId: number; // Assuming you have a commentId to like a specific comment
    userId?: number; // Optional userId if applicable
    // Add other properties as necessary
}

export const likeComment = async (like: Like): Promise<any> => {
    try {
        const response = await axios.post<any>(API_URL, like);
        return response.data;
    } catch (error) {
        const axiosError = error as AxiosError;
        console.error('Error liking comment:', axiosError);
        throw axiosError;
    }
};