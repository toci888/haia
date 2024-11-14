import React, { useState, useEffect } from 'react';
import './styles/PostList.css';
//import { createReaction } from '../apiService';
import { createReaction, createComment, getCommentsByPost } from '../apiService';
import GenerateJoke from './GenerateJoke';
import { generateJoke } from '../apiService';
import JokeItem from './JokeItem';



const JokeListGoovno = ({ jokes }) => {

  useEffect(() => {
    // Inicjalizacja reakcji dla każdego posta
    const initialReactions = jokes.reduce((acc, post) => {
      acc[post.id] = {
        funny: post.reactions.filter(r => r.reactionType === 'funny').length,
        super: post.reactions.filter(r => r.reactionType === 'super').length,
        dry: post.reactions.filter(r => r.reactionType === 'dry').length,
        dontCare: post.reactions.filter(r => r.reactionType === 'dontCare').length,
      };
      return acc;
    }, {});
    // setReactions(initialReactions);
    
    // Inicjalizacja komentarzy dla każdego posta
    // jokes.forEach(post => fetchComments(post.id));
  }, [jokes]);


  // Funkcja do pobierania komentarzy dla konkretnego posta


  // Obsługa reakcji na posty


  // Obsługa dodawania nowego komentarza


  return (
    <div className="post-list">
      {jokes.map((joke) => (
        <JokeItem key={joke.id} post={joke} />
      ))}
    </div>
  );
};

export default JokeListGoovno;