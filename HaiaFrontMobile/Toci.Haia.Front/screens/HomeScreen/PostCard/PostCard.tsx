import React from "react";
import { Avatar, Card, Chip } from "react-native-paper";
import { View, StyleSheet, Text, TextInput, TouchableOpacity } from "react-native";
import { Post } from "../../../domains/models/Post";

type PostCardProps = {
  post: Post;
}

const PostCard = ({ post }: PostCardProps) => {

  return(
    <Card key={post.id} style={styles.card}>
      <Card.Title
        title={post.group?.name}
        subtitle={`Author: ${post.user.username} - ${post.group?.description}`}
        left={(props) => <Avatar.Icon {...props} icon="account-circle" />}
      />
      <Card.Content>
        <Text style={styles.jokeContent}>{post.content}</Text>
        <Text style={styles.jokeContent}>{post.id}</Text>
        <View style={styles.infoContainer}>
          <Chip style={styles.categoryChip}>{post.category?.name}</Chip>
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
      {/* <View style={styles.inputContainer}>
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
      </View> */}
    </Card>
  )
}

const styles = StyleSheet.create({
  card: {
    margin: 10,
    borderRadius: 10,
    backgroundColor: '#fff',
    elevation: 4,
  },
  infoContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginTop: 10,
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
  dateText: {
    fontSize: 12,
    color: '#888',
  },
});

export default PostCard;