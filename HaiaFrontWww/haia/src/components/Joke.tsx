import React, { useState } from 'react';
import { addComment, reactToJoke, reactToComment } from '../apiService';

interface Comment {
    id: number; // Assuming id is a number
    text: string; // Assuming text is a string
}

interface JokeProps {
    joke: {
        id: number; // Assuming id is a number
        text: string; // Assuming text is a string
        comments?: Comment[]; // Optional comments array
    };
    userId: string; // Assuming userId is a string, adjust if needed
}

const Joke: React.FC<JokeProps> = ({ joke, userId }) => {
    const [commentText, setCommentText] = useState<string>('');
    const [comments, setComments] = useState<Comment[]>(joke.comments || []);

    const handleAddComment = async () => {
        const newComment = await addComment(joke.id, commentText);
        setComments([...comments, newComment]);
        setCommentText('');
    };

    const handleReaction = async (reactionType: string) => {
        await reactToJoke(joke.id, reactionType, userId);
        alert(`You reacted with: ${reactionType}`);
    };

    const handleCommentReaction = async (commentId: number, reactionType: string) => {
        await reactToComment(commentId, reactionType, userId);
        alert(`You reacted to comment with: ${reactionType}`);
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