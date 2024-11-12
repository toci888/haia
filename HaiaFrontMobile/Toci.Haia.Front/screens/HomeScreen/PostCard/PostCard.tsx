import React, { useEffect, useState } from "react";
import { Avatar, Card, Chip } from "react-native-paper";
import { View, StyleSheet, Text } from "react-native";
import { Post } from "../../../domains/models/Post";
import PostCardCommentForm from "./PostCardCommentForm/PostCardCommentForm";
import PostCardComments from "./PostCardComments/PostCardComments";

type PostCardProps = {
  post: Post;
}

const PostCard = ({ post }: PostCardProps) => {
  const [showedComments, setShowedComments] = useState(false);

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
      <View>
      <Text style={styles.showCommentsText} onPress={() => setShowedComments(p => !p)}>
        Show comments for post
      </Text>
      </View>
      {showedComments && <PostCardComments postId={post.id} />}
      <PostCardCommentForm post={post} />

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
  showCommentsText : {
    marginTop: 20,
    width: 150,
    padding: 5,
    marginLeft: 10,
    fontSize: 15,
    color: '#333',
    fontWeight: '600',
    borderRadius: 10,
    height: 30,
    backgroundColor: '#5425',
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