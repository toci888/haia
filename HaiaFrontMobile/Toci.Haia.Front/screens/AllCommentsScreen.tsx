import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, FlatList } from 'react-native';
import axios from 'axios';
import { appSelector } from '../store/Store';

const CommentScreen = () => {
  const { user: { currentUser } } = appSelector(s => s);
  const [comments, setComments] = useState([]); // Lista komentarzy
  const [newComment, setNewComment] = useState(''); // Nowy komentarz
  const [likedComments, setLikedComments] = useState({}); // Komentarze polubione

  // Funkcja do pobrania komentarzy z API
  const fetchComments = async () => {
    try {
      const response = await axios.get('http://192.168.191.47:7113/api/Comment');
      setComments(response.data);
    } catch (error) {
      console.error('Error fetching comments:', error);
    }
  };

  // Funkcja do wysyłania nowego komentarza do API
  const submitComment = async () => {
    if (newComment.trim()) {
      try {
        const response = await axios.post('https://example.com/api/comments', {
          text: newComment,
        });
        setComments([...comments, response.data]);
        setNewComment('');
      } catch (error) {
        console.error('Error submitting comment:', error);
      }
    }
  };

  // Funkcja do polubienia komentarza
  const handleLike = (id) => {
    //todo post axios

    setLikedComments((prevState) => ({
      ...prevState,
      [id]: !prevState[id],
    }));
  };

  useEffect(() => {
    fetchComments();
  }, []);

  // Komponent renderujący komentarz
  const renderComment = ({ item }) => (
    <View style={styles.commentContainer}>
      <Text style={styles.commentText}>{item.text}</Text>
      <TouchableOpacity
        style={styles.likeButton}
        onPress={() => handleLike(item.id)}
      >
        <Text style={styles.likeButtonText}>
          {likedComments[item.id] ? 'Unlike' : 'Like'}
        </Text>
      </TouchableOpacity>
    </View>
  );

  return (
    <View style={styles.container}>
      <View>
        {currentUser && (
          <Text>Witaj {currentUser.login}</Text>
        )}
      </View>
      <FlatList
        data={comments}
        renderItem={renderComment}
        keyExtractor={(item) => item.id.toString()}
        contentContainerStyle={styles.commentList}
      />

      <View style={styles.inputContainer}>
        <TextInput
          style={styles.input}
          placeholder="Add a comment..."
          value={newComment}
          onChangeText={setNewComment}
        />
        <TouchableOpacity
          style={styles.submitButton}
          onPress={submitComment}
        >
          <Text style={styles.submitButtonText}>Submit</Text>
        </TouchableOpacity>
      </View>
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
  commentContainer: {
    backgroundColor: '#ffffff',
    padding: 15,
    marginBottom: 10,
    borderRadius: 10,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  commentText: {
    fontSize: 16,
    color: '#333',
    flex: 1,
  },
  likeButton: {
    paddingHorizontal: 10,
    paddingVertical: 5,
    backgroundColor: '#007bff',
    borderRadius: 5,
  },
  likeButtonText: {
    color: '#fff',
    fontWeight: 'bold',
  },
  inputContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 20,
    paddingHorizontal: 10,
  },
  input: {
    flex: 1,
    padding: 15,
    backgroundColor: '#ffffff',
    borderRadius: 10,
    marginRight: 10,
    fontSize: 16,
  },
  submitButton: {
    backgroundColor: '#28a745',
    padding: 15,
    borderRadius: 10,
  },
  submitButtonText: {
    color: '#fff',
    fontWeight: 'bold',
  },
});

export default CommentScreen;
