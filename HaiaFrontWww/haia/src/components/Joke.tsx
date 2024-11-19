import React, { useState } from 'react';
import { addComment, reactToJoke, reactToComment } from '../apiService';

interface Comment {
    id: number;
    jokeId: number;
    text: string;
}

interface Joke {
    id: number;
    text: string;
    comments?: Comment[]; // Pole opcjonalne
}

interface JokeProps {
    joke: Joke;
    userId: number; // Zakładam, że userId jest liczbą. Zmień typ, jeśli to string.
}

const Joke: React.FC<JokeProps> = ({ joke, userId }) => {
    const [commentText, setCommentText] = useState<string>('');
    const [comments, setComments] = useState<Comment[]>(joke.comments || []);

    const handleAddComment = async () => {
        try {
            const newComment: Comment = await addComment(joke.id, commentText);
            setComments([...comments, newComment]);
            setCommentText('');
        } catch (error) {
            console.error('Error adding comment:', error);
        }
    };

    const handleReaction = async (reactionType: string) => {
        try {
            await reactToJoke(joke.id, reactionType, userId);
            alert(`You reacted with: ${reactionType}`);
        } catch (error) {
            console.error('Error reacting to joke:', error);
        }
    };

    const handleCommentReaction = async (commentId: number, reactionType: string) => {
        try {
            await reactToComment(commentId, reactionType, userId);
            alert(`You reacted to comment with: ${reactionType}`);
        } catch (error) {
            console.error('Error reacting to comment:', error);
        }
    };

    return (
        <div className="joke-card">
            <h3>{joke.text}</h3>
            <div className="reaction-buttons">
                <button onClick={() => handleReaction('like')}>Like</button>
                <button onClick={() => handleReaction('superlike')}>Super Like</button>
                <button onClick={() => handleReaction('meh')}>Meh</button>
                <button onClick={() => handleReaction('dislike')}>Not for Me</button>
            </div>
            <div className="comments-section">
                {comments.map((comment) => (
                    <div key={comment.id} className="comment">
                        <p>{comment.text}</p>
                        <div className="reaction-buttons">
                            <button onClick={() => handleCommentReaction(comment.id, 'like')}>Like</button>
                            <button onClick={() => handleCommentReaction(comment.id, 'superlike')}>Super Like</button>
                            <button onClick={() => handleCommentReaction(comment.id, 'meh')}>Meh</button>
                            <button onClick={() => handleCommentReaction(comment.id, 'dislike')}>Not for Me</button>
                        </div>
                    </div>
                ))}
            </div>
            <input
                type="text"
                value={commentText}
                onChange={(e) => setCommentText(e.target.value)}
                placeholder="Add a comment..."
            />
            <button onClick={handleAddComment}>Add Comment</button>
        </div>
    );
};

export default Joke;
