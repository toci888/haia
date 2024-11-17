import { addComment, reactToJoke, reactToComment } from '../../apiService';
import React, { useEffect, useState } from 'react'

const API_URL = 'http://80.209.230.198:5117/api';

export default function PostJokeReaction({ post }) {

console.log(post, 'Post here');

      const [reactions, setReactions] = useState({});

      const createReaction = async (data) => {
      
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


    const handleReaction = (userId, jokeId, postId, reactionType) => {
        createReaction({
          "reactionType": reactionType,
          "jokeId": jokeId,
          "userId": userId
        });
    
        setReactions((prevReactions) => ({
          ...prevReactions,
          [postId]: {
            ...prevReactions[postId],
            [reactionType]: prevReactions[postId][reactionType] + 1,
          },
        }));
      };

      return (<div className="post-reactions">
            <span onClick={() => handleReaction(1, post.jokeId, post.id, 'funny')} role="img" aria-label="Turbo Śmieszne">
            😂 {reactions[post.id]?.funny || 0}
            </span>
            <span onClick={() => handleReaction(1, post.jokeId, post.id, 'super')} role="img" aria-label="Super">
            👍 {reactions[post.id]?.super || 0}
            </span>
            <span onClick={() => handleReaction(1, post.jokeId, post.id, 'dry')} role="img" aria-label="Suchar">
            🥱 {reactions[post.id]?.dry || 0}
            </span>
            <span onClick={() => handleReaction(1, post.jokeId, post.id, 'dontCare')} role="img" aria-label="Wali mnie to">
            😒 {reactions[post.id]?.dontCare || 0}
            </span>

</div>);

      }