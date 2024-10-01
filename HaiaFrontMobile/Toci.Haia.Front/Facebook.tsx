// Facebook.tsx
import React, { useEffect, useState } from 'react';
import { View, Text, Button, StyleSheet } from 'react-native';
import * as Facebook from 'expo-auth-session/providers/facebook';
import * as WebBrowser from 'expo-web-browser';
import { useNavigation } from '@react-navigation/native';

WebBrowser.maybeCompleteAuthSession();

export default function FacebookScreen() {
  const [token, setToken] = useState(null);
  const navigation = useNavigation();
  const [request, response, promptAsync] = Facebook.useAuthRequest({
    clientId: 'YOUR_FACEBOOK_APP_ID',
  });

  useEffect(() => {
    if (response?.type === 'success') {
      const { access_token } = response.params;
      setToken(access_token);
    }
  }, [response]);

  const goToComments = () => {
    // Navigate to the Comment screen after login
    navigation.navigate('Comment');
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Facebook Login</Text>
      <Button
        disabled={!request}
        title="Login with Facebook"
        onPress={() => {
          promptAsync();
        }}
      />
      
        <Button
          title="Proceed to Comments"
          onPress={goToComments} // Navigate to Comment screen
        />
      
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  title: {
    fontSize: 24,
    marginBottom: 20,
  },
});
