import React, { useState } from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import { Button, ActivityIndicator } from 'react-native';
import axios from 'axios';
import { appDispatch, appSelector } from './store/Store';
import { mockUser1, mockUser2 } from './common/mocks/mockUser';
import { userAction } from './store/user/userSlice';
import { useNavigation } from '@react-navigation/native';

export default function MainScreen() {
  const navigation = useNavigation();
  const dispatch = appDispatch();
  const { user: { currentUser } } = appSelector(s => s);

  const [joke, setJoke] = useState('');
  const [loading, setLoading] = useState(false);

  const fetchJoke = async () => {
    setLoading(true);
    try {
      const response = await axios.post(
        'http://192.168.191.47:7113/api/Comment/2/generate-joke',
        {},
        {
          headers: {
            'Content-Type': 'application/json',
            //Authorization: `Bearer YOUR_OPENAI_API_KEY`,
          },
        }
      );
      const generatedJoke = response.data.gptJoke;
      setJoke(generatedJoke);
    } catch (error) {
      console.error('Error fetching the joke:', error);
      setJoke('Failed to fetch a joke');
    }

    setLoading(false);
  };

  // Funkcje, które będą obsługiwać kliknięcia przycisków
  const handleButton1Press = () => {
    fetchJoke();
  };

  const changeUser = () => {
    const userId = currentUser!.id;
    dispatch(userAction.setUser(userId == 1 ? mockUser2 : mockUser1));
  }

  return (
    <View style={styles.container}>
      <View>
        <Button
          title="Change user"
          onPress={changeUser}
        />
          {currentUser && (
            <Text>Witaj {currentUser.login}</Text>
          )}
        </View>
      <Text style={styles.title}>{joke}</Text>
      {loading ? (
        <ActivityIndicator size="large" color="#0000ff" />
      ) : (
        <Text style={styles.jokeText}>{joke || 'Press the button to generate a joke'}</Text>
      )}
      
      <TouchableOpacity style={styles.button} onPress={handleButton1Press}>
        <Text style={styles.buttonText}>sprzedam lemiesz</Text>
      </TouchableOpacity>

      <TouchableOpacity style={styles.button} onPress={handleButton1Press}>
        <Text style={styles.buttonText}>gdzie mieszkają menelinhowie ?</Text>
      </TouchableOpacity>

      <TouchableOpacity style={styles.button} onPress={handleButton1Press}>
        <Text style={styles.buttonText}>czy Pawel ?</Text>
      </TouchableOpacity>

      <Button
        title="Go to Facebook Login"
        onPress={() => navigation.navigate('FriendRequests')}
      />
    </View>
  );
};

// Style dla komponentu
const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#f5f5f5',
  },
  title: {
    fontSize: 24,
    marginBottom: 20,
    textAlign: 'center',
  },
  jokeText: {
    fontSize: 18,
    textAlign: 'center',
    marginVertical: 20,
  },
  button: {
    width: '80%',
    padding: 20,
    backgroundColor: '#007bff',
    borderRadius: 10,
    marginVertical: 10,
    alignItems: 'center',
  },
  buttonText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: 'bold',
  },
});

