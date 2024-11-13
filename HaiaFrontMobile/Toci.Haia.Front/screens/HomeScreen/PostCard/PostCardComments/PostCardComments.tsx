import React, { useEffect, useState } from 'react'
import { View, Text, StyleSheet } from 'react-native';
import { getCommentsByPostIdRequest } from '../../../../domains/api/postsComments/getCommentsByPostIdRequest';

export interface PostCardCommentsProps {
  postId: number;
}

export default function PostCardComments({ postId }: PostCardCommentsProps) {
  const [comments, setComments] = useState<any[]>([]);

  const fetchPostComments = async () => {
    const commentsResp: any = await getCommentsByPostIdRequest(postId);
    console.log(comments.length);
    setComments(commentsResp);
  }
  useEffect(() => {
    fetchPostComments();
  }, [])

  return (
    <View style={styles.commentsContainer}>
      {comments.map((comment) => (
        <Text key={comment.id} style={styles.commentText}>
          {comment.user.username} napisal:  {comment.text}
        </Text>
      ))}
    </View>
  );
};

const styles = StyleSheet.create({
  commentsContainer: {
    flexDirection: 'column',
    margin: 10,
  },
  jokeContent: {
    marginTop: 10,
    fontSize: 16,
    color: '#333',
    fontWeight: '400',
  },
  categoryChip: {
    backgroundColor: '#f5f5f5',
  },
  commentText: {
    fontSize: 12,
    margin: 10,
    color: 'black',
  },
});
