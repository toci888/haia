export interface Comment {
    jokeId: number;
    text: string;
}

export interface Category {
    id: number;
    name: string;
}

export interface PreferenceData {
    userId: number;
    categoryId: number;
    preferenceLevel: number;
}

export interface User {
    id: number;
    username: string;
    email: string;
}

export interface Joke {
    id: number;
    text: string;
    userId: number;
    categoryId: number;
}

export interface Reaction {
    jokeId?: number;
    commentId?: number;
    reactionType: string;
    userId: number;
}

export interface Credentials {
    email: string;
    password: string;
}

export interface Group {
    id: number;
    name: string;
    description: string;
}

export interface Post {
    id: number;
    text: string;
    groupId: number;
}
