import axios from 'axios';

const API_URL = 'http://80.209.230.198:5117/api';

export const addComment = async (jokeId, commentText) => {
    const response = await axios.post(`${API_URL}/comments`, {
        jokeId,
        text: commentText
    });
    return response.data;
};


// Pobranie listy wszystkich kategorii
export const getCategories = async () => {
  try {
      const response = await axios.get(`${API_URL}/Category`);
      return response.data;
  } catch (error) {
      console.error("Błąd podczas pobierania kategorii", error);
      throw error;
  }
};

export const createUserPreference = async (preferenceData) => {
  const response = await axios.post(`${API_URL}/UserCategoryPreference`, preferenceData);
  return response.data;
};


// Pobranie jednej kategorii po ID
export const getCategoryById = async (id) => {
  try {
      const response = await axios.get(`${API_URL}/Category/${id}`);
      return response.data;
  } catch (error) {
      console.error("Błąd podczas pobierania kategorii o ID:", id, error);
      throw error;
  }
};

// Dodanie nowej kategorii
export const createCategory = async (categoryData) => {
  try {
      const response = await axios.post(`${API_URL}/Category`, categoryData);
      return response.data;
  } catch (error) {
      console.error("Błąd podczas tworzenia kategorii", error);
      throw error;
  }
};

// Aktualizacja istniejącej kategorii
export const updateCategory = async (id, categoryData) => {
  try {
      const response = await axios.put(`${API_URL}/Category/${id}`, categoryData);
      return response.data;
  } catch (error) {
      console.error("Błąd podczas aktualizacji kategorii o ID:", id, error);
      throw error;
  }
};

// Usunięcie kategorii
export const deleteCategory = async (id) => {
  try {
      const response = await axios.delete(`${API_URL}/Category/${id}`);
      return response.data;
  } catch (error) {
      console.error("Błąd podczas usuwania kategorii o ID:", id, error);
      throw error;
  }
};

// Funkcja generująca dowcip
export const generateJoke = async (postId) => {
  try {
      const response = await axios.post(`${API_URL}/Jokes/${postId}/generate-joke`);
      return response.data; // Zakładamy, że odpowiedź zawiera wygenerowany dowcip
  } catch (error) {
      console.error("Błąd podczas generowania dowcipu", error);
      throw error;
  }
};

export const createReaction = async (data) => {

console.log(data);

  const response = await fetch(`${API_URL}/Reaction`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
  });
  return await response.json();
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

export const searchUsers = async (query) => {
  const response = await axios.get(`${API_URL}/Friendship/search-users`, {
      params: { query }
  });
  return response.data;
};


// Pobranie danych użytkownika
export const getUserProfile = async (userId) => {
  const response = await axios.get(`${API_URL}/User/${userId}`);
  return response.data;
};

// Pobranie żartów użytkownika
export const getUserJokes = async (userId) => {
  const response = await axios.get(`${API_URL}/User/${userId}/jokes`);
  return response.data;
};


// Pobranie listy oczekujących zaproszeń do znajomych
export const getPendingInvitations = async (userId) => {
  const response = await axios.get(`${API_URL}/Friendship/invitations/${userId}`);
  return response.data;
};

// Akceptacja zaproszenia do znajomych
export const acceptInvitation = async (friendshipId) => {
  const response = await axios.post(`${API_URL}/Friendship/accept/${friendshipId}`);
  return response.data;
};

// Odrzucenie zaproszenia do znajomych
export const rejectInvitation = async (friendshipId) => {
  const response = await axios.delete(`${API_URL}/Friendship/reject/${friendshipId}`);
  return response.data;
};


// Pobranie listy znajomych
export const getFriends = async (userId) => {
  const response = await axios.get(`${API_URL}/Friendship/user/${userId}`);
  return response.data;
};

// Dodanie znajomego
export const addFriend = async (userId, friendId) => {
  const response = await axios.post(`${API_URL}/Friendship/Invite`, { userId, friendId });
  return response.data;
};

// Usunięcie znajomego
export const removeFriend = async (Id) => {
  const response = await axios.delete(`${API_URL}/Friendship/${Id}`);
  return response.data;
};


export const loginUser = async (credentials) => {
  try {
      const response = await axios.post(`${API_URL}/User/login`, credentials);
      return response; // Możesz tutaj zwrócić `response.data` dla danych użytkownika
  } catch (error) {
      console.error("Błąd logowania:", error);
      throw error; // Rzuca błąd, który zostanie obsłużony w komponencie logowania
  }
};

export const registerUser = async (userData) => {
    return await axios.post(`${API_URL}/User/register`, userData);
};

export const socialLogin = async (provider, providerUserId) => {
    return await axios.post(`${API_URL}/social-login`, { provider, providerUserId });
};

export const getUserById = async (userId) => {
    return await axios.get(`${API_URL}/${userId}`);
};

export const getJokes = async () => {
  return await axios.get(`${API_URL}/Jokes`);
};

export const addJoke = async (jokeData) => {
    return await axios.post(`${API_URL}/Jokes`, jokeData);
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

  export const createComment = async (data) => {
    const response = await fetch(`${API_URL}/Comment`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });
    return await response.json();
  };
  
  export const getCommentsByPost = async (postId) => {
    const response = await fetch(`${API_URL}/Comment/postComments/${postId}`);

    console.log(response);

    return await response.json();
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
//export const getCategories = () => apiGet("UserGroup/categories");

// Reakcje Użytkownika na Kategorie
export const reactToCategory = (data) => apiPost("UserCategoryPreference/React", data);
export const getTopCategoriesForUser = (userId) => apiGet(`UserCategoryPreference/TopCategories/${userId}`);

