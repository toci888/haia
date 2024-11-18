import React, { useEffect, useState } from 'react';
import { getPostsByGroup, createPostInGroup } from './api';

interface Post {
    id: number; // Assuming post ID is a number
    content: string; // Assuming content is a string
}

interface PostsProps {
    groupId: number; // Assuming groupId is a number
}

const Posts: React.FC<PostsProps> = ({ groupId }) => {
    const [posts, setPosts] = useState<Post[]>([]);
    const [content, setContent] = useState<string>("");

    useEffect(() => {
        fetchPosts();
    }, [groupId]);

    const fetchPosts = async () => {
        try {
            const data = await getPostsByGroup(groupId);
            setPosts(data);
        } catch (error) {
            console.error("Error fetching posts:", error);
        }
    };

    const handleCreatePost = async () => {
        try {
            await createPostInGroup(groupId, { content });
            setContent("");
            fetchPosts();
        } catch (error) {
            console.error("Error creating post:", error);
        }
    };

    return (
        <div>
            <h2>Posts in Group</h2>
            <input 
                value={content} 
                onChange={(e) => setContent(e.target.value)} 
                placeholder="Post content" 
            />
            <button onClick={handleCreatePost}>Add Post</button>
            <ul>
                {posts.map((post) => (
                    <li key={post.id}>{post.content}</li>
                ))}
            </ul>
        </div>
    );
};

export default Posts;