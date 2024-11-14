import React from 'react'
import { Card, CardContent, Typography, Button } from '@mui/material';
import { addComment, reactToJoke, reactToComment } from '../apiService';


export default function JokeCard({ joke }) {

  const handleCommentReaction = async (commentId, reactionType) => {
    await reactToComment(commentId, reactionType, 34);
    alert(`You reacted to comment with: ${reactionType}`);
};

  return (
    <Card sx={{ maxWidth: 345, margin: 2, boxShadow: 3 }}>
      <CardContent>
        <Typography variant="h6" color="text.secondary" gutterBottom>
          
        </Typography>
        <Typography variant="body2" color="text.primary">
          {joke.jokeText}
        </Typography>
        <Typography variant="caption" color="text.secondary">
          {new Date(joke.createdAt).toLocaleString()}
        </Typography>
      </CardContent>

    </Card>
  );
}

