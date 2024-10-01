// LiveTextScreen.tsx
import React, { useState } from 'react';
import { View, Text, ScrollView, TouchableOpacity, StyleSheet, Button, TextInput, Alert } from 'react-native';

const LiveTextScreen = () => {
  const [selectedText, setSelectedText] = useState<string | null>(null);
  const [comment, setComment] = useState('');
  const [liveText, setLiveText] = useState('This is an example of live spoken text. You can select any part to comment on.');

  // Function to handle comment submission
  const submitComment = async () => {
    if (selectedText && comment) {
      try {
        const response = await fetch('http://your-api-url/api/comments', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            text: comment,
            snippet: selectedText,
            author: 'User123',
            snippetAuthor: 'Conference Speaker',
            commentTimestamp: new Date().toISOString(),
            snippetTimestamp: new Date().toISOString(),
          }),
        });
        if (response.ok) {
          Alert.alert('Success', 'Comment submitted successfully!');
          setComment('');
          setSelectedText(null);
        } else {
          throw new Error('Failed to submit comment');
        }
      } catch (error) {
        Alert.alert('Error', 'Failed to submit comment: ' + error.message);
      }
    } else {
      Alert.alert('Error', 'Please select text and enter a comment.');
    }
  };

  // Example function to select part of the text
  const selectText = (text: string) => {
    setSelectedText(text);
  };

  return (
    <View style={styles.container}>
      <ScrollView style={styles.textContainer}>
        <TouchableOpacity onPress={() => selectText('This is an example')}>
          <Text style={styles.liveText}>This is an example of live spoken text.</Text>
        </TouchableOpacity>
        <TouchableOpacity onPress={() => selectText('You can select any part')}>
          <Text style={styles.liveText}>You can select any part to comment on.</Text>
        </TouchableOpacity>
      </ScrollView>

      {selectedText && (
        <View style={styles.commentSection}>
          <Text style={styles.selectedText}>Selected Text: {selectedText}</Text>
          <TextInput
            style={styles.input}
            placeholder="Enter your comment"
            value={comment}
            onChangeText={setComment}
          />
          <Button title="Submit Comment" onPress={submitComment} />
        </View>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
  },
  textContainer: {
    flex: 1,
  },
  liveText: {
    fontSize: 18,
    marginVertical: 10,
  },
  commentSection: {
    padding: 10,
    borderTopWidth: 1,
    borderTopColor: '#ccc',
  },
  selectedText: {
    fontSize: 16,
    fontWeight: 'bold',
    marginBottom: 10,
  },
  input: {
    borderColor: '#ccc',
    borderWidth: 1,
    padding: 10,
    marginBottom: 10,
  },
});

export default LiveTextScreen;
