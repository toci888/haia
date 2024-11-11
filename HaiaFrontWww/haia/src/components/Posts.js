import React, { useEffect, useState } from 'react';
import { getPostsByGroup, createPostInGroup } from './api';

const Posts = ({ groupId }) => {
  const [posts, setPosts] = useState([]);
  const [content, setContent] = useState("");

  useEffect(() => {
    fetchPosts();
  }, [groupId]);

  const fetchPosts = async () => {
    const data = await getPostsByGroup(groupId);
    setPosts(data);
  };

  const handleCreatePost = async () => {
    await createPostInGroup(groupId, { content });
    setContent("");
    fetchPosts();
  };

  return (
    <div>
      <h2>Posty w Grupie</h2>
      <input value={content} onChange={(e) => setContent(e.target.value)} placeholder="Treść postu" />
      <button onClick={handleCreatePost}>Dodaj Post</button>
      <ul>
        {posts.map((post) => (
          <li key={post.id}>{post.content}</li>
        ))}
      </ul>
    </div>
  );
};

export default Posts;
