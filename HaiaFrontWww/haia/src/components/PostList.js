import React, { useState } from 'react';
import './styles/PostList.css';

const PostList = ({ posts }) => {
  const [reactions, setReactions] = useState(
    posts.reduce((acc, post) => {
      acc[post.id] = { funny: 0, super: 0, dry: 0, dontCare: 0 };
      return acc;
    }, {})
  );

  const handleReaction = (postId, reactionType) => {
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
        <div key={post.id} className="post-card">
          <div className="post-header">
            <h4 className="post-username">{post.user.username}</h4>
            <span className="post-date">
              {new Date(post.createdAt).toLocaleString()}
            </span>
          </div>
          
          {/* Sekcja reakcji */}
          <div className="post-reactions">
            <span onClick={() => handleReaction(post.id, 'funny')} role="img" aria-label="Turbo Śmieszne">
              😂 {reactions[post.id].funny}
            </span>
            <span onClick={() => handleReaction(post.id, 'super')} role="img" aria-label="Super">
              👍 {reactions[post.id].super}
            </span>
            <span onClick={() => handleReaction(post.id, 'dry')} role="img" aria-label="Suchar">
              🥱 {reactions[post.id].dry}
            </span>
            <span onClick={() => handleReaction(post.id, 'dontCare')} role="img" aria-label="Wali mnie to">
              😒 {reactions[post.id].dontCare}
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
