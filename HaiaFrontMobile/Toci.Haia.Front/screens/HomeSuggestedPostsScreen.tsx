import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, FlatList } from 'react-native';
import axios, { AxiosError } from 'axios';
import { appSelector } from '../store/Store';
import { Card, Text as PaperText, Avatar, Chip } from 'react-native-paper';
import { Post } from '../domains/models/Post';


const CommentScreen = () => {
  const { user: { currentUser } } = appSelector(s => s);
  const [suggestedPosts, setSuggestedPosts] = useState<Post[]>([]);
  const [comments, setComments] = useState<{postId: number, comment: string; }[]>([]);
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

  const fetchSuggestedPosts = async () => {
    try {
      const response = await axios.get(`http://80.209.230.198:5117/api/PostInteraction/suggested/${currentUser!.id}`);
      console.log(response);
      setSuggestedPosts(response.data);
    } catch (error) {
      console.error('Error fetching comments:', error);
    }
  };

  // Funkcja do wysyłania nowego komentarza do API
  // const submitComment = async () => {
  //   if (newComment.trim()) {
  //     try {
  //       const response = await axios.post('https://example.com/api/comments', {
  //         text: newComment,
  //       });
  //       setComments([...comments, response.data]);
  //       setNewComment('');
  //     } catch (error) {
  //       console.error('Error submitting comment:', error);
  //     }
  //   }
  // };

  // Funkcja do polubienia komentarza
  // const handleLike = (id) => {
  //   //todo post axios

  //   setLikedComments((prevState) => ({
  //     ...prevState,
  //     [id]: !prevState[id],
  //   }));
  // };

  useEffect(() => {
    // fetchComments();
    fetchSuggestedPosts();
  }, []);

  // Komponent renderujący komentarz
  // const renderComment = ({ item }) => (
  //   <View style={styles.commentContainer}>
  //     <Text style={styles.commentText}>{item.text}</Text>
  //     <TouchableOpacity
  //       style={styles.likeButton}
  //       onPress={() => handleLike(item.id)}
  //     >
  //       <Text style={styles.likeButtonText}>
  //         {likedComments[item.id] ? 'Unlike' : 'Like'}
  //       </Text>
  //     </TouchableOpacity>
  //   </View>
  // );

  const handleSubmitComment = async (id: number) => {
    console.log(id);
    const nowPost = suggestedPosts.find(p => p.id === id)!;
  console.log(nowPost);
  // console.log(JSON.stringify(comments));
  // console.log(comments.find(c => c.postId === id)?.comment);
  // return;

    try {
      const response = await axios.post('http://80.209.230.198:5117/api/Comment', {
        "userId": nowPost.user.id,
        "text": comments.find(c => c.postId === id)?.comment,
        // "author": "string",
        // "commentTimestamp": "2024-11-11T21:00:07.687Z"
      });
      setComments([...comments, response.data]);
    } catch (error) {
      if (axios.isAxiosError(error)) {
        // Wyświetlenie szczegółów błędu axios
        console.error('Błąd Axios:', {
            message: error.message,
            status: error.response?.status,
            data: error.response?.data,
        });
    } else {
        // Obsługa innych błędów
        console.error('Inny błąd:', error);
    }
      console.error('Error submitting comment:', error);
    }
  }

  const handleSetNewComment = (id: number, text: string) => {
    console.log(id);
    console.log(text);
    const newComments = [...comments];
    const nowComment = newComments.find(c => c.postId === id);
    if(nowComment) {
      nowComment.comment = text;
      setComments(newComments);
    } else {
      newComments.push({ postId: id, comment: text });
      setComments(newComments);
    }
  }

  return (
    <View style={styles.container}>
      <View>
        {currentUser && (
          <Text>Witaj {currentUser.login}</Text>
        )}
      </View>
      {suggestedPosts.length > 0 && (
        <>
          {suggestedPosts.map((post: Post) => (
            <Card key={post.id} style={styles.card}>
              <Card.Title
                title={post.group.name}
                subtitle={`Author: ${post.user.username} - ${post.group.description}`}
                left={(props) => <Avatar.Icon {...props} icon="account-circle" />}
              />
              <Card.Content>
                <Text style={styles.jokeContent}>{post.content}</Text>
                <View style={styles.infoContainer}>
                  <Chip style={styles.categoryChip}>{post.category.name}</Chip>
                  <Text style={styles.dateText}>{new Date(post.createdAt).toLocaleDateString()}</Text>
                </View>
              </Card.Content>
              {/* <View style={styles.commentsContainer}>
                {comments
                    .filter((c) => c.postId === post.id) // Filtrujemy komentarze dla danego posta
                    .map((comment) => (
                        <Text key={comment.id} style={styles.commentText}>
                            {comment.comment}
                        </Text>
                    ))}
              </View> */}
              <View style={styles.inputContainer}>
                <TextInput
                  style={styles.input}
                  placeholder="Add a comment..."
                  value={comments.find((c) => c.postId === post.id)?.comment ?? ""}
                  onChangeText={(e) => handleSetNewComment(post.id, e)}
                />
                <TouchableOpacity
                  style={styles.submitButton}
                  onPress={() => handleSubmitComment(post.id)}
                >
                  <Text style={styles.submitButtonText}>Submit</Text>
                </TouchableOpacity>
              </View>
            </Card>
          ))}
        </>
      )}

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
  commentContainer: {
    backgroundColor: '#ffffff',
    padding: 15,
    marginBottom: 10,
    borderRadius: 10,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
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



  // post card
  card: {
    margin: 10,
    borderRadius: 10,
    backgroundColor: '#fff',
    elevation: 4,
  },
  jokeContent: {
    marginTop: 10,
    fontSize: 16,
    color: '#333',
    fontWeight: '400',
  },
  infoContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginTop: 10,
  },
  categoryChip: {
    backgroundColor: '#f5f5f5',
  },
  dateText: {
    fontSize: 12,
    color: '#888',
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
