import React, { useState } from "react";
import { TextInput, TouchableOpacity, View, Text, StyleSheet } from "react-native";
import { Post } from "../../../../domains/models/Post";
import { addCommentToPostRequest } from "../../../../domains/api/postsComments/addCommentToPostRequest";
import { appSelector } from "../../../../store/Store";


type PostCardCommentFormProps = {
  post: Post;
}

const PostCardCommentForm = ({ post }: PostCardCommentFormProps) => {
  const [comments, setComments] = useState<string>("");
  const { user: { currentUser } } = appSelector(s => s);

  const handleSetNewComment = (id: number, text: string) => {
    console.log(id);
    console.log(text);
    setComments(text);
    
  }

  const handleSubmitComment = async (id: number) => {
    console.log(id);

    const resp = await addCommentToPostRequest({
      content: comments,
      postId: post.id,
      userId: currentUser!.id,
    });

    if (resp !== -1) {
      console.log('handleSubmitComment, Works');
    }

  }
  return (
    <View style={styles.inputContainer}>
      <TextInput
        style={styles.input}
        placeholder="Add a comment..."
        value={comments}
        onChangeText={(e) => handleSetNewComment(post.id, e)}
      />
      <TouchableOpacity
        style={styles.submitButton}
        onPress={() => handleSubmitComment(post.id)}
      >
        <Text style={styles.submitButtonText}>Submit</Text>
      </TouchableOpacity>
    </View>
  );
};

const styles = StyleSheet.create({
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
  submitButtonText: {
    color: '#fff',
    fontWeight: 'bold',
  },
  submitButton: {
    backgroundColor: '#28a745',
    padding: 15,
    borderRadius: 10,
  },
});

export default PostCardCommentForm;