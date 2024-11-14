import React, { useEffect, useState } from 'react'
import { createComment } from '../services/commentService';
import { createReaction, generateJoke, getCommentsByPost } from '../apiService';
import JokeCard from './JokeCard';

export default function JokeItem({ post }) {
  const [reactions, setReactions] = useState({});
  const [commendReactions, setCommentReactions] = useState({});
  const [comments, setComments] = useState({});
  const [newComment, setNewComment] = useState({});
  const [loading, setLoading] = useState(false);
    const [generatedJoke, setGeneratedJoke] = useState(null);

    // useEffect(() => {

      
    //   // Inicjalizacja komentarzy dla każdego posta
    //   // jokes.forEach(post => fetchComments(post.id));
    // }, [post]);

    const fetchComments = async (postId) => {
      const postComments = await getCommentsByPost(postId);
  
      setComments(prevComments => ({
        ...prevComments,
        [postId]: postComments,
      }));
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

  
  const handleGenerateJoke = async (postId) => {
    setLoading(true);
    try {
        const joke = await generateJoke(postId); // Wywołanie z userId lub jokeId
        console.log(joke);
        setGeneratedJoke(joke); // Zapisz wygenerowany dowcip
    } catch (error) {
        console.error("Błąd generowania dowcipu:", error);
    } finally {
        setLoading(false);
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

  console.log(post);

  return (
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


        {/* Przycisk generowania dowcipu */}
        <button onClick={() => handleGenerateJoke(post.jokeId)} disabled={loading}>
            {loading ? 'Generowanie...' : 'Wygeneruj dowcip'}
        </button>

        { post.gptJokes.map(joke => <JokeCard joke={joke} />) }

        {/* Wyświetlanie wygenerowanego dowcipu */}
        {generatedJoke && (
          <div className="generated-joke">
            <h3>Dowcip ChatGPT:</h3>
            {/* <p>{JSON.stringify(generatedJoke)}</p> */}
            {generatedJoke &&  <JokeCard joke={generatedJoke} />} 
          </div>
        )}
      </div>

      <div className="post-content">
        <p>{post.text || "Brak treści"}</p>
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
  )
}
