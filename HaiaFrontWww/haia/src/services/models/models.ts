interface User {
    username: string;
}

interface Comment {
    id: number;
    text: string;
    user: User;
    reactions?: Record<string, number>;
}

interface Post {
    id: number;
    jokeId: number;
    user: User;
    createdAt: string;
    text: string;
    jokeText: string;
}

interface Joke {
    id: number;
    jokeId: number;
    user: User;
    createdAt: string;
    text: string;
    jokeText: string;
}

interface JokeItemProps {
    post: Post;
}

interface Comment {
    id: number;
    jokeId: number;
    text: string;
}

interface Joke {
    id: number;
    text: string;
    comments?: Comment[]; // Pole opcjonalne
}

interface JokeProps {
    joke: Joke;
    userId: number; // Zakładam, że userId jest liczbą. Zmień typ, jeśli to string.
}
