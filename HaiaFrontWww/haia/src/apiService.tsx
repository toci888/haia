import axios from 'axios';
import { Comment, Category, PreferenceData, Joke, Reaction, User, Credentials, Group, Post } from './models';
//import { Category } from './models';

const API_URL = 'http://80.209.230.198:5117/api';

// Dodawanie komentarza
export const addComment = async (jokeId: number, commentText: string): Promise<Comment> => {
    const response = await axios.post<Comment>(`${API_URL}/comments`, { jokeId, text: commentText });
    return response.data;
};

// Pobranie listy kategorii
export const getCategories = async (): Promise<Category[]> => {
    const response = await axios.get<Category[]>(`${API_URL}/Category`);
    return response.data;
};

// Tworzenie preferencji użytkownika
export const createUserPreference = async (preferenceData: PreferenceData): Promise<PreferenceData> => {
    const response = await axios.post<PreferenceData>(`${API_URL}/UserCategoryPreference`, preferenceData);
    return response.data;
};

// Pobranie kategorii po ID
export const getCategoryById = async (id: number): Promise<Category> => {
    const response = await axios.get<Category>(`${API_URL}/Category/${id}`);
    return response.data;
};

// Dodanie nowej kategorii
export const createCategory = async (categoryData: Category): Promise<Category> => {
    const response = await axios.post<Category>(`${API_URL}/Category`, categoryData);
    return response.data;
};

// Aktualizacja kategorii
export const updateCategory = async (id: number, categoryData: Category): Promise<Category> => {
    const response = await axios.put<Category>(`${API_URL}/Category/${id}`, categoryData);
    return response.data;
};

// Usunięcie kategorii
export const deleteCategory = async (id: number): Promise<void> => {
    await axios.delete(`${API_URL}/Category/${id}`);
};

// Generowanie dowcipu
export const generateJoke = async (postId: number): Promise<Joke> => {
    const response = await axios.post<Joke>(`${API_URL}/Jokes/${postId}/generate-joke`);
    return response.data;
};

// Tworzenie reakcji
export const createReaction = async (data: Reaction): Promise<Reaction> => {
    const response = await axios.post<Reaction>(`${API_URL}/Reaction`, data);
    return response.data;
};

// Reakcja na dowcip
export const reactToJoke = async (jokeId: number, reactionType: string, userId: number): Promise<void> => {
    await axios.post(`${API_URL}/jokes/${jokeId}/react`, { reactionType, userId });
};

// Reakcja na komentarz
export const reactToComment = async (commentId: number, reactionType: string, userId: number): Promise<void> => {
    await axios.post(`${API_URL}/comments/${commentId}/react`, { reactionType, userId });
};

// Wyszukiwanie użytkowników
export const searchUsers = async (query: string): Promise<User[]> => {
    const response = await axios.get<User[]>(`${API_URL}/Friendship/search-users`, { params: { query } });
    return response.data;
};

// Pobranie profilu użytkownika
export const getUserProfile = async (userId: number): Promise<User> => {
    const response = await axios.get<User>(`${API_URL}/User/${userId}`);
    return response.data;
};

// Pobranie dowcipów użytkownika
export const getUserJokes = async (userId: number): Promise<Joke[]> => {
    const response = await axios.get<Joke[]>(`${API_URL}/User/${userId}/jokes`);
    return response.data;
};

// Pobranie zaproszeń
export const getPendingInvitations = async (userId: number): Promise<any[]> => {
    const response = await axios.get(`${API_URL}/Friendship/invitations/${userId}`);
    return response.data;
};

// Akceptowanie zaproszenia
export const acceptInvitation = async (friendshipId: number): Promise<void> => {
    await axios.post(`${API_URL}/Friendship/accept/${friendshipId}`);
};

// Odrzucenie zaproszenia
export const rejectInvitation = async (friendshipId: number): Promise<void> => {
    await axios.delete(`${API_URL}/Friendship/reject/${friendshipId}`);
};

// Pobranie znajomych
export const getFriends = async (userId: number): Promise<User[]> => {
    const response = await axios.get<User[]>(`${API_URL}/Friendship/user/${userId}`);
    return response.data;
};

// Dodanie znajomego
export const addFriend = async (userId: number, friendId: number): Promise<void> => {
    await axios.post(`${API_URL}/Friendship/Invite`, { userId, friendId });
};

// Usunięcie znajomego
export const removeFriend = async (id: number): Promise<void> => {
    await axios.delete(`${API_URL}/Friendship/${id}`);
};

// Logowanie użytkownika
export const loginUser = async (credentials: Credentials): Promise<User> => {
    const response = await axios.post<User>(`${API_URL}/User/login`, credentials);
    return response.data;
};

// Rejestracja użytkownika
export const registerUser = async (userData: User): Promise<User> => {
    const response = await axios.post<User>(`${API_URL}/User/register`, userData);
    return response.data;
};

// Social Login
export const socialLogin = async (provider: string, providerUserId: string): Promise<User> => {
    const response = await axios.post<User>(`${API_URL}/social-login`, { provider, providerUserId });
    return response.data;
};

// Grupowe API
export const getUserGroups = async (): Promise<Group[]> => {
    const response = await axios.get<Group[]>(`${API_URL}/UserGroup`);
    return response.data;
};

// Posty w grupach
export const getPostsByGroup = async (groupId: number): Promise<Post[]> => {
    const response = await axios.get<Post[]>(`${API_URL}/UserGroup/${groupId}/posts`);
    return response.data;
};
