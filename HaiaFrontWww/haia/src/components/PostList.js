import React, { useState, useEffect } from 'react';
import './styles/PostList.css';
import { createReaction } from '../apiService';

const PostList = ({ posts }) => {

  const [reactions, setReactions] = useState({});


  useEffect(() => {
    // Inicjalizacja reakcji dla każdego posta przy pierwszym renderowaniu
    const initialReactions = posts.reduce((acc, post) => {


      acc[post.id] = { 
        funny: post.reactions.filter(r => r.reactionType === 'funny').length, 
        super: post.reactions.filter(r => r.reactionType === 'super').length,
        dry: post.reactions.filter(r => r.reactionType === 'dry').length, 
        dontCare: post.reactions.filter(r => r.reactionType === 'dontCare').length };
      return acc;
    }, {});
    setReactions(initialReactions);
  }, [posts]);




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

  
  return (
    
    <div className="post-list">
      {posts.map((post) => (

//console.log(post.reactions[post.id]);

        <div key={post.id} className="post-card">
          <div className="post-header">
            <h4 className="post-username">{post.user.username}</h4>
            <span className="post-date">
              {new Date(post.createdAt).toLocaleString()}
            </span>
          </div>
          
          {/* Sekcja reakcji */}
          <div className="post-reactions">
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
          </div>


          <div className="post-content">
            <p>{post.content || "Brak treści"}</p>
          </div>
        </div>
      ))}
    </div>
  );
};

export default PostList;
