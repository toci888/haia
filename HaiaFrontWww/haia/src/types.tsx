interface Comment {
    id: number;
    jokeId: number;
    text: string;
}

// interface Joke {
//     id: number;
//     text: string;
//     comments?: Comment[]; // Pole opcjonalne
// }

interface JokeProps {
    joke: Joke;
    userId: number; // Zakładam, że userId jest liczbą. Zmień typ, jeśli to string.
}
