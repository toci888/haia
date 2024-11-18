import React, { useState, useEffect } from 'react';
import { createReaction, createComment, getCommentsByPost } from '../apiService';
import './styles/PostList.css';

interface User {
    id: number;
    username: string;
}

interface Reaction {
    reactionType: string;
}

interface Comment {
    id: number;
    text: string;
    user: User;
    reactions?: Record<string, number>; // Assuming reactions are stored as a record
}

interface Post {
    id: number;
    user: User;
    createdAt: string;
    content: string;
    jokeId: number;
    reactions: Reaction[];
}

interface PostListProps {
    posts: Post[];
}

const PostList: React.FC<PostListProps> = ({ posts }) => {
    const [reactions, setReactions] = useState<Record<number, Record<string, number>>>({});
    const [comments, setComments] = useState<Record<number, Comment[]>>({});
    const [newComment, setNewComment] = useState<Record<number, string>>({});

    useEffect(() => {
        // Initialize reactions for each post
        const initialReactions = posts.reduce((acc: Record<number, Record<string, number>>, post: Post) => {
            acc[post.id] = {
                funny: post.reactions.filter(r => r.reactionType === 'funny').length,
                super: post.reactions.filter(r => r.reactionType === 'super').length,
                dry: post.reactions.filter(r => r.reactionType === 'dry').length,
                dontCare: post.reactions.filter(r => r.reactionType === 'dontCare').length,
            };
            return acc;
        }, {});
        setReactions(initialReactions);

        // Initialize comments for each post
        posts.forEach(post => fetchComments(post.id));
    }, [posts]);

    // Function to fetch comments for a specific post
    const fetchComments = async (postId: number) => {
        const postComments = await getCommentsByPost(postId);
        setComments(prevComments => ({
            ...prevComments,
            [postId]: postComments,
        }));
    };

    // Handle reactions on posts
    const handleReaction = (userId: number, jokeId: number, postId: number, reactionType: string) => {
        createReaction({
            reactionType,
            jokeId,
            userId
        });
        setReactions(prevReactions => ({
            ...prevReactions,
            [postId]: {
                ...prevReactions[postId],
                [reactionType]: (prevReactions[postId][reactionType] || 0) + 1,
            },
        }));
    };

    // Handle adding a new comment
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

    // Handle reactions on comments
    const handleCommentReaction = (userId: number, commentId: number, reactionType: string) => {
        createReaction({
            reactionType,
            commentId,
            userId
        });
        // You can update the state for comment reactions similarly to post reactions
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
                    {/* Reactions section */}
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
                        <span onClick={() => handleReaction(1, post.jokeId, post.id, 'dontCare')} role="img" aria-label="Dont Care">
                            😒 {reactions[post.id]?.dontCare || 0}
                        </span>
                    </div>
                    <div className="post-content">
                        <p>{post.content || "No content"}</p>
                    </div>
                    {/* Comments section */}
                    <div className="post-comments">
                        <h5>Comments:</h5>
                        {(comments[post.id] || []).map(comment => (
                            <div key={comment.id} className="comment">
                                <span><strong>{comment.user.username}</strong>: {comment.text}</span>
                                <div className="comment-reactions">
                                    <span onClick={() => handleCommentReaction(1, comment.id, 'funny')} role="img" aria-label="Funny">
                                        😂 {comment.reactions?.funny || 0}
                                    </span>
                                    <span onClick={() => handleCommentReaction(1, comment.id, 'super')} role="img" aria-label="Super">
                                        👍 {comment.reactions?.super || 0}
                                    </span>
                                    <span onClick={() => handleCommentReaction(1, comment.id, 'dry')} role="img" aria-label="Dry">
                                        🥱 {comment.reactions?.dry || 0}
                                    </span>
                                    <span onClick={() => handleCommentReaction(1, comment.id, 'dontCare')} role="img" aria-label="Dont Care">
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
            ))}
        </div>
    );
};

export default PostList;