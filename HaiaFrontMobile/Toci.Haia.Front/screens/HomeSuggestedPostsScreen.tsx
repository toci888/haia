import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import axios from 'axios';
import { appSelector } from '../store/Store';
import { Card, Text as PaperText, Avatar } from 'react-native-paper';
import { Post } from '../domains/models/Post';


const CommentScreen = () => {
  const { user: { currentUser } } = appSelector(s => s);


  return (
    <View style={styles.container}>
      <View>
        {currentUser && (
          <Text>Witaj {currentUser.login}</Text>
        )}
      </View>

      {/* <View style={styles.commentsContainer}>
        {comments
            .filter((c) => c.postId === post.id) // Filtrujemy komentarze dla danego posta
            .map((comment) => (
                <Text key={comment.id} style={styles.commentText}>
                    {comment.comment}
                </Text>
            ))}
      </View> */}

      {/* <FlatList
        data={comments}
        renderItem={renderComment}
        keyExtractor={(item) => item.id.toString()}
        contentContainerStyle={styles.commentList}
      /> */}
    </View>
  );
};

// Style aplikacji
const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
    padding: 10,
  },
  commentList: {
    paddingVertical: 20,
  },
 

  //comments styles
  // commentsContainer: {
  //   marginTop: 10,
  //   marginBottom: 10,
  // },
  // commentText: {
  //     fontSize: 14,
  //     color: '#555',
  //     marginVertical: 2,
  // },
});

export default CommentScreen;
