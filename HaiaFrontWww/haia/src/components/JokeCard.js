import React from 'react'
import { Card, CardContent, Typography, Button } from '@mui/material';

export default function JokeCard({ joke }) {
  return (
    <Card sx={{ maxWidth: 345, margin: 2, boxShadow: 3 }}>
      <CardContent>
        <Typography variant="h6" color="text.secondary" gutterBottom>
          Joke ID: {joke.id}
        </Typography>
        <Typography variant="body2" color="text.primary">
          {joke.jokeText}
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
}
