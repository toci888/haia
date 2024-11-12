import React, { useState, useEffect } from 'react';
import './styles/PostList.css';
//import { createReaction } from '../apiService';
import { createReaction, createComment, getCommentsByPost } from '../apiService';


const PostList = ({ posts }) => {


  const [reactions, setReactions] = useState({});
  const [commendReactions, setCommentReactions] = useState({});
  const [comments, setComments] = useState({});
  const [newComment, setNewComment] = useState({});

  useEffect(() => {
    // Inicjalizacja reakcji dla każdego posta
    const initialReactions = posts.reduce((acc, post) => {
      acc[post.id] = {
        funny: post.reactions.filter(r => r.reactionType === 'funny').length,
        super: post.reactions.filter(r => r.reactionType === 'super').length,
        dry: post.reactions.filter(r => r.reactionType === 'dry').length,
        dontCare: post.reactions.filter(r => r.reactionType === 'dontCare').length,
      };
      return acc;
    }, {});
    setReactions(initialReactions);
    
    // Inicjalizacja komentarzy dla każdego posta
    posts.forEach(post => fetchComments(post.id));
  }, [posts]);

  // Funkcja do pobierania komentarzy dla konkretnego posta
  const fetchComments = async (postId) => {
    const postComments = await getCommentsByPost(postId);

    console.log(postComments, 'lala');

    setComments(prevComments => ({
      ...prevComments,
      [postId]: postComments,
    }));
  };

  // Obsługa reakcji na posty
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

  // Obsługa dodawania nowego komentarza
  const handleAddComment = async (postId) => {
    const commentText = newComment[postId];
    if (commentText) {
      const newCommentData = await createComment({
        "text": commentText,
        "postId": postId,
        "userId": 1, // zakładamy, że mamy ID zalogowanego użytkownika
      });
console.log(newCommentData, 'LUKLU');
      setComments(prevComments => ({
        ...prevComments,
        [postId]: [...(prevComments[postId] || []), newCommentData],
      }));
      
      setNewComment(prevNewComment => ({
        ...prevNewComment,
        [postId]: "",
      }));
    }
  };

  // Obsługa reakcji na komentarze
  const handleCommentReaction = (userId, commentId, reactionType) => {
    createReaction({
      "reactionType": reactionType,
      "commentId": commentId,
      "userId": userId
    });

    // Można tutaj zaktualizować stan reakcji dla komentarzy w podobny sposób, jak dla postów
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

          {/* Sekcja komentarzy */}
          <div className="post-comments">
            <h5>Komentarze:</h5>
            {(comments[post.id] || []).map(comment => (
              <div key={comment.id} className="comment">
                <span><strong>{comment.user.username}</strong>: {comment.text}</span>
                <div className="comment-reactions">
                  {/* <span onClick={() => handleCommentReaction(1, comment.id, 'like')} role="img" aria-label="Lubię to">
                    👍 {comment.reactions?.like || 0}
                  </span>
                  <span onClick={() => handleCommentReaction(1, comment.id, 'haha')} role="img" aria-label="Haha">
                    😂 {comment.reactions?.haha || 0}
                  </span> */}

                       <span onClick={() =>  handleCommentReaction(1, comment.id, post.id, 'funny')} role="img" aria-label="Turbo Śmieszne">
                          😂 {comment.reactions?.funny || 0}
                        </span>
                        <span onClick={() => handleCommentReaction(1, comment.id, post.id, 'super')} role="img" aria-label="Super">
                          👍 {reactions[post.id]?.super || 0}
                        </span>
                        <span onClick={() => handleCommentReaction(1, comment.id, post.id, 'dry')} role="img" aria-label="Suchar">
                          🥱 {reactions[post.id]?.dry || 0}
                        </span>
                        <span onClick={() => handleCommentReaction(1, comment.id, post.id, 'dontCare')} role="img" aria-label="Wali mnie to">
                          😒 {reactions[post.id]?.dontCare || 0}
                        </span>

                </div>
              </div>
            ))}
            
            <div className="add-comment">
              <input
                type="text"
                value={newComment[post.id] || ""}
                onChange={(e) => setNewComment(prev => ({ ...prev, [post.id]: e.target.value }))}
                placeholder="Dodaj komentarz"
              />
              <button onClick={() => handleAddComment(post.id)}>Wyślij</button>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};

export default PostList;