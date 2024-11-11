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

// Funkcja pomocnicza do obsługi zapytań GET
export const apiGet = async (endpoint) => {
    const response = await fetch(`${API_URL}/${endpoint}`);
    return await response.json();
  };
  
  // Funkcja pomocnicza do obsługi zapytań POST
  export const apiPost = async (endpoint, data) => {
    const response = await fetch(`${API_URL}/${endpoint}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });
    return await response.json();
  };
  
  // Funkcja pomocnicza do obsługi zapytań PUT
  export const apiPut = async (endpoint, data) => {
    const response = await fetch(`${API_URL}/${endpoint}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });
    return await response.json();
  };
  
  // Funkcja pomocnicza do obsługi zapytań DELETE
  export const apiDelete = async (endpoint) => {
    await fetch(`${API_URL}/${endpoint}`, { method: "DELETE" });
  };

  export const getUserGroups = () => apiGet("UserGroup");
export const getUserGroupById = (id) => apiGet(`UserGroup/${id}`);
export const createUserGroup = (data) => apiPost("UserGroup", data);
export const updateUserGroup = (id, data) => apiPut(`UserGroup/${id}`, data);
export const deleteUserGroup = (id) => apiDelete(`UserGroup/${id}`);

// Posty w Grupie
export const getPostsByGroup = (groupId) => apiGet(`UserGroup/${groupId}/posts`);
export const createPostInGroup = (groupId, data) => apiPost(`UserGroup/${groupId}/posts`, data);

// Kategorie
export const getCategories = () => apiGet("UserGroup/categories");

// Reakcje Użytkownika na Kategorie
export const reactToCategory = (data) => apiPost("UserCategoryPreference/React", data);
export const getTopCategoriesForUser = (userId) => apiGet(`UserCategoryPreference/TopCategories/${userId}`);
