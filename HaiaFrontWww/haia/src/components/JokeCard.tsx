import React from 'react';
import { Card, CardContent, Typography, Button } from '@mui/material';
import { Joke } from '../models';

interface User {
    username: string;
}

// interface Joke {
//     id: number;
//     jokeId: number;
//     user: User;
//     createdAt: string;
//     text: string;
//     jokeText: string; // Assuming jokeText is a string
    
// }

interface JokeCardProps {
    joke: Joke; // The joke prop should match the Joke interface
}

const JokeCard: React.FC<JokeCardProps> = ({ joke }) => {
    return (
        <Card sx={{ maxWidth: 345, margin: 2, boxShadow: 3 }}>
            <CardContent>
                <Typography variant="h6" color="text.secondary" gutterBottom>
                    Joke ID: {joke.id}
                </Typography>
                <Typography variant="body2" color="text.primary">
                    {joke.text}
                </Typography>
                <Typography variant="caption" color="text.secondary">
                    Created At: {new Date(joke.createdAt).toLocaleString()}
                </Typography>
            </CardContent>
            <Button variant="contained" color="primary" sx={{ margin: 1 }}>
                Like
            </Button>
            <Button variant="outlined" color="secondary" sx={{ margin: 1 }}>
                Share
            </Button>
        </Card>
    );
};

export default JokeCard;