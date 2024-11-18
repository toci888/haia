import React, { useEffect, useState } from 'react';
import { createComment, createReaction, generateJoke, getCommentsByPost } from '../apiService';
import JokeCard from './JokeCard';

interface User {
    username: string;
}

interface Comment {
    id: number;
    text: string;
    user: User;
    reactions?: Record<string, number>; // Assuming reactions are stored as a record
}

interface Post {
    id: number;
    jokeId: number;
    user: User;
    createdAt: string;
    text: string;
}

interface JokeItemProps {
    post: Post;
}

const JokeItem: React.FC<JokeItemProps> = ({ post }) => {
    const [reactions, setReactions] = useState<Record<number, Record<string, number>>>({});
    const [comments, setComments] = useState<Record<number, Comment[]>>({});
    const [newComment, setNewComment] = useState<Record<number, string>>({});
    const [loading, setLoading] = useState<boolean>(false);
    const [generatedJoke, setGeneratedJoke] = useState<any | null>(null); // Adjust type as needed

    const fetchComments = async (postId: number) => {
        const postComments = await getCommentsByPost(postId);
        setComments(prevComments => ({
            ...prevComments,
            [postId]: postComments,
        }));
    };

    const handleReaction = async (userId: number, jokeId: number, postId: number, reactionType: string) => {
        await createReaction({
            reactionType,
            jokeId,
            userId
        });
        setReactions(prevReactions => ({
            ...prevReactions,
            [postId]: {
                ...prevReactions[postId],
                [reactionType]: (prevReactions[postId]?.[reactionType] || 0) + 1,
            },
        }));
    };

    const handleAddComment = async (postId: number) => {
        const commentText = newComment[postId];
        if (commentText) {
            const newCommentData = await createComment({
                text: commentText,
                postId,
                userId: 1, // Assuming we have the logged-in user's ID
            });
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

    const handleGenerateJoke = async (postId: number) => {
        setLoading(true);
        try {
            const joke = await generateJoke(postId); // Call with userId or jokeId as needed
            setGeneratedJoke(joke); // Save the generated joke
        } catch (error) {
            console.error("Error generating joke:", error);
        } finally {
            setLoading(false);
        }
    };

    const handleCommentReaction = async (userId: number, commentId: number, postId: number, reactionType: string) => {
        await createReaction({
            reactionType,
            commentId,
            userId
        });
        // Here you can update the state for comment reactions similarly to post reactions
    };

    return (
        <div key={post.id} className="post-card">
            <div className="post-header">
                <h4 className="post-username">{post.user.username}</h4>
                <span className="post-date">
                    {new Date(post.createdAt).toLocaleString()}
                </span>
            </div>
            <div className="post-reactions">
                <span onClick={() => handleReaction(1, post.jokeId, post.id, 'funny')} role="img" aria-label="Funny">
                    😂 {reactions[post.id]?.funny || 0}
                </span>
                <span onClick={() => handleReaction(1, post.jokeId, post.id, 'super')} role="img" aria-label="Super">
                    👍 {reactions[post.id]?.super || 0}
                </span>
                <span onClick={() => handleReaction(1, post.jokeId, post.id, 'dry')} role="img" aria-label="Dry">
                    🥱 {reactions[post.id]?.dry || 0}
                </span>
                <span onClick={() => handleReaction(1, post.jokeId, post.id, 'dontCare')} role="img" aria-label="Don't Care">
                    😒 {reactions[post.id]?.dontCare || 0}
                </span>
                <button onClick={() => handleGenerateJoke(post.id)} disabled={loading}>
                    {loading ? 'Generating...' : 'Generate Joke'}
                </button>
                {generatedJoke && (
                    <div className="generated-joke">
                        <h3>Generated Joke:</h3>
                        <JokeCard joke={generatedJoke} />
                    </div>
                )}
            </div>
            <div className="post-content">
                <p>{post.text || "No content"}</p>
            </div>
            <div className="post-comments">
                <h5>Comments:</h5>
                {(comments[post.id] || []).map(comment => (
                    <div key={comment.id} className="comment">
                        <span><strong>{comment.user.username}</strong>: {comment.text}</span>
                        <div className="comment-reactions">
                            <span onClick={() => handleCommentReaction(1, comment.id, post.id, 'funny')} role="img" aria-label="Funny">
                                😂 {comment.reactions?.funny || 0}
                            </span>
                            <span onClick={() => handleCommentReaction(1, comment.id, post.id, 'super')} role="img" aria-label="Super">
                                👍 {comment.reactions?.super || 0}
                            </span>
                            <span onClick={() => handleCommentReaction(1, comment.id, post.id, 'dry')} role="img" aria-label="Dry">
                                🥱 {comment.reactions?.dry || 0}
                            </span>
                            <span onClick={() => handleCommentReaction(1, comment.id, post.id, 'dontCare')} role="img" aria-label="Don't Care">
                                😒 {comment.reactions?.dontCare || 0}
                            </span>
                        </div>
                    </div>
                ))}
                <div className="add-comment">
                    <input
                        type="text"
                        value={newComment[post.id] || ""}
                        onChange={(e) => setNewComment(prev => ({ ...prev, [post.id]: e.target.value }))}
                        placeholder="Add a comment"
                    />
                    <button onClick={() => handleAddComment(post.id)}>Send</button>
                </div>
            </div>
        </div>
    );
};

export default JokeItem;