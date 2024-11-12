import React from 'react';
import './styles/PostList.css';

const PostList = ({ posts }) => {
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
          <div className="post-content">
            <p>{post.content || "Brak treści"}</p>
          </div>
        </div>
      ))}
    </div>
  );
};

export default PostList;
