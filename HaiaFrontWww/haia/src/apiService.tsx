import axios from 'axios';

const API_URL = 'http://80.209.230.198:5117/api';

// Define interfaces for the expected data structures
interface Comment {
    id: number;
    text: string;
    user: User;
    jokeId: number;
    reactions?: Record<string, number>;
}


interface Category {
    id: number;
    name: string;
    description?: string;
}

interface UserPreference {
    userId: number;
    categoryId: number;
}

interface Joke {
    id: number;
    jokeId: number;
    user: User;
    createdAt: string;
    text: string;
}

interface User {
    id: number;
    username: string;
    firstName: string;
    lastName: string;
    email: string;
}

// Function to add a comment to a joke
export const addComment = async (jokeId: number, commentText: string): Promise<Comment> => {
    const response = await axios.post(`${API_URL}/comments`, {
        jokeId,
        text: commentText
    });
    return response.data;
};

// Fetch all categories
export const getCategories = async (): Promise<Category[]> => {
    try {
        const response = await axios.get(`${API_URL}/Category`);
        return response.data;
    } catch (error) {
        console.error("Error fetching categories", error);
        throw error;
    }
};

// Create a user preference for a category
export const createUserPreference = async (preferenceData: UserPreference): Promise<void> => {
    const response = await axios.post(`${API_URL}/UserCategoryPreference`, preferenceData);
    return response.data;
};

// Fetch a category by ID
export const getCategoryById = async (id: number): Promise<Category> => {
    try {
        const response = await axios.get(`${API_URL}/Category/${id}`);
        return response.data;
    } catch (error) {
        console.error("Error fetching category by ID:", id, error);
        throw error;
    }
};

// Create a new category
export const createCategory = async (categoryData: Category): Promise<Category> => {
    try {
        const response = await axios.post(`${API_URL}/Category`, categoryData);
        return response.data;
    } catch (error) {
        console.error("Error creating category", error);
        throw error;
    }
};

// Update an existing category
export const updateCategory = async (id: number, categoryData: Category): Promise<Category> => {
    try {
        const response = await axios.put(`${API_URL}/Category/${id}`, categoryData);
        return response.data;
    } catch (error) {
        console.error("Error updating category by ID:", id, error);
        throw error;
    }
};

// Delete a category
export const deleteCategory = async (id: number): Promise<void> => {
    try {
        const response = await axios.delete(`${API_URL}/Category/${id}`);
        return response.data;
    } catch (error) {
        console.error("Error deleting category by ID:", id, error);
        throw error;
    }
};

// Generate a joke based on a post ID
export const generateJoke = async (postId: number): Promise<Joke> => {
    try {
        const response = await axios.post(`${API_URL}/Jokes/${postId}/generate-joke`);
        return response.data; // Assuming the response contains the generated joke
    } catch (error) {
        console.error("Error generating joke", error);
        throw error;
    }
};

// Create a reaction
export const createReaction = async (data: any): Promise<any> => {
    const response = await fetch(`${API_URL}/Reaction`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
    });
    return await response.json();
};

// React to a joke
export const reactToJoke = async (jokeId: number, reactionType: string, userId: number): Promise<any> => {
    const response = await axios.post(`${API_URL}/jokes/${jokeId}/react`, {
        reactionType,
        userId
    });
    return response.data;
};

// React to a comment
export const reactToComment = async (commentId: number, reactionType: string, userId: number): Promise<any> => {
    const response = await axios.post(`${API_URL}/comments/${commentId}/react`, {
        reactionType,
        userId
    });
    return response.data;
};

// Search for users
export const searchUsers = async (query: string): Promise<User[]> => {
    const response = await axios.get(`${API_URL}/Friendship/search-users`, {
        params: { query }
    });
    return response.data;
};

// Fetch user profile
export const getUserProfile = async (userId: number): Promise<User> => {
    const response = await axios.get(`${API_URL}/User/${userId}`);
    return response.data;
};

// Fetch jokes by user ID
export const getUserJokes = async (userId: number): Promise<Joke[]> => {
    const response = await axios.get(`${API_URL}/User/${userId}/jokes`);
    return response.data;
};

// Fetch pending friend invitations
export const getPendingInvitations = async (userId: number): Promise<any[]> => {
    const response = await axios.get(`${API_URL}/Friendship/invitations/${userId}`);
    return response.data;
};

// Accept a friend invitation
export const acceptInvitation = async (friendshipId: number): Promise<any> => {
    const response = await axios.post(`${API_URL}/Friendship/accept/${friendshipId}`);
    return response.data;
};

// Reject a friend invitation
export const rejectInvitation = async (friendshipId: number): Promise<any> => {
    const response = await axios.delete(`${API_URL}/Friendship/reject/${friendshipId}`);
    return response.data;
};

// Fetch friends list
export const getFriends = async (userId: number): Promise<User[]> => {
    const response = await axios.get(`${API_URL}/Friendship/user/${userId}`);
    return response.data;
};

// Add a friend
export const addFriend = async (userId: number, friendId: number): Promise<any> => {
    const response = await axios.post(`${API_URL}/Friendship/Invite`, { userId, friendId });
    return response.data;
};

// Remove a friend
export const removeFriend = async (Id: number, FriendId: number): Promise<void> => {
    const response = await axios.delete(`${API_URL}/Friendship/${Id}`);
    return response.data;
};

// Login user
export const loginUser = async (credentials: any): Promise<any> => {
    try {
        const response = await axios.post(`${API_URL}/User/login`, credentials);
        return response; // You can return `response.data` for user data
    } catch (error) {
        console.error("Login error:", error);
        throw error; // Throw error to be handled in the login component
    }
};

// Register user
export const registerUser = async (userData: any): Promise<any> => {
    return await axios.post(`${API_URL}/User/register`, userData);
};

// Social login
export const socialLogin = async (provider: string, providerUserId: string): Promise<any> => {
    return await axios.post(`${API_URL}/social-login`, { provider, providerUserId });
};

// Fetch user by ID
export const getUserById = async (userId: number): Promise<User> => {
    return await axios.get(`${API_URL}/${userId}`);
};

// Fetch all jokes
export const getJokes = async (): Promise<Joke[]> => {
    return await axios.get(`${API_URL}/Jokes`);
};

// Add a joke
export const addJoke = async (jokeData: Joke): Promise<Joke> => {
    return await axios.post(`${API_URL}/Jokes`, jokeData);
};

// Helper function for GET requests
export const apiGet = async (endpoint: string): Promise<any> => {
    const response = await fetch(`${API_URL}/${endpoint}`);
    return await response.json();
};

// Helper function for POST requests
export const apiPost = async (endpoint: string, data: any): Promise<any> => {
    const response = await fetch(`${API_URL}/${endpoint}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
    });
    return await response.json();
};

// Helper function for PUT requests
export const apiPut = async (endpoint: string, data: any): Promise<any> => {
    const response = await fetch(`${API_URL}/${endpoint}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
    });
    return await response.json();
};

// Helper function for DELETE requests
export const apiDelete = async (endpoint: string): Promise<void> => {
    await fetch(`${API_URL}/${endpoint}`, { method: "DELETE" });
};

// Create a comment
export const createComment = async (data: Comment): Promise<Comment> => {
    const response = await fetch(`${API_URL}/Comment`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
    });
    return await response.json();
};

// Get comments by post ID
export const getCommentsByPost = async (postId: number): Promise<Comment[]> => {
    const response = await fetch(`${API_URL}/Comment/postComments/${postId}`);
    return await response.json();
};

// User groups
export const getUserGroups = async (): Promise<any[]> => apiGet("UserGroup");
export const getUserGroupById = async (id: number): Promise<any> => apiGet(`UserGroup/${id}`);
export const createUserGroup = async (data: any): Promise<any> => apiPost("UserGroup", data);
export const updateUserGroup = async (id: number, data: any): Promise<any> => apiPut(`UserGroup/${id}`, data);
export const deleteUserGroup = async (id: number): Promise<void> => apiDelete(`UserGroup/${id}`);

// Posts in group
export const getPostsByGroup = async (groupId: number): Promise<any[]> => apiGet(`UserGroup/${groupId}/posts`);
export const createPostInGroup = async (groupId: number, data: any): Promise<any> => apiPost(`UserGroup/${groupId}/posts`, data);

// User category reactions
export const reactToCategory = async (data: any): Promise<any> => apiPost("UserCategoryPreference/React", data);
export const getTopCategoriesForUser = async (userId: number): Promise<any[]> => apiGet(`UserCategoryPreference/TopCategories/${userId}`);