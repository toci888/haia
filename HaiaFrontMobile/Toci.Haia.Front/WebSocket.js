import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, Button, FlatList } from 'react-native';
import * as signalR from '@microsoft/signalr';

const SignalRChat = () => {
  const [connection, setConnection] = useState(null);
  const [user, setUser] = useState('');
  const [message, setMessage] = useState('');
  const [messages, setMessages] = useState([]);

  useEffect(() => {
    const connect = new signalR.HubConnectionBuilder()
      .withUrl('http://your-server-url/chathub')  // Zastąp URL serwera
      .build();

    connect.on('ReceiveMessage', (user, message) => {
      setMessages((prevMessages) => [...prevMessages, { user, message }]);
    });

    connect.start()
      .then(() => console.log('Connected to SignalR server'))
      .catch(err => console.log('Connection failed: ', err));

    setConnection(connect);

    return () => {
      connect.stop();
    };
  }, []);

  const sendMessage = () => {
    if (connection) {
      connection.invoke('SendMessage', user, message)
        .catch(err => console.error('Error sending message: ', err));
      setMessage('');
    }
  };

  return (
    <View style={{ padding: 20 }}>
      <TextInput
        placeholder="Username"
        value={user}
        onChangeText={setUser}
        style={{ borderBottomWidth: 1, marginBottom: 10 }}
      />
      <TextInput
        placeholder="Message"
        value={message}
        onChangeText={setMessage}
        style={{ borderBottomWidth: 1, marginBottom: 10 }}
      />
      <Button title="Send" onPress={sendMessage} />

      <FlatList
        data={messages}
        keyExtractor={(item, index) => index.toString()}
        renderItem={({ item }) => (
          <Text>{item.user}: {item.message}</Text>
        )}
      />
    </View>
  );
};

export default SignalRChat;
