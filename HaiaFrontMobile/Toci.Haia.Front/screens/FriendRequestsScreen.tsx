import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, Alert } from 'react-native';
import * as SignalR from '@microsoft/signalr';

const FriendRequestsScreen = () => {
  const [friendRequest, setFriendRequest] = useState(null);
  const [connection, setConnection] = useState(null);

  useEffect(() => {
    // Połącz się z serwerem SignalR
    const connectToSignalR = async () => {
      const hubConnection = new SignalR.HubConnectionBuilder()
        .withUrl('https://your-api-url/friendRequestHub') // Adres URL do SignalR Hub
        .configureLogging(SignalR.LogLevel.Information)
        .build();

      try {
        await hubConnection.start();
        console.log('SignalR connected.');
        setConnection(hubConnection);

        // Nasłuchuj na event "ReceiveFriendRequest"
        hubConnection.on('ReceiveFriendRequest', (message) => {
          setFriendRequest(message);
          Alert.alert('New Friend Request', message);
        });
      } catch (err) {
        console.error('Error while connecting to SignalR', err);
      }
    };

    connectToSignalR();

    // Zakończ połączenie podczas unmountu komponentu
    return () => {
      if (connection) {
        connection.stop();
      }
    };
  }, []);

  return (
    <View style={styles.container}>
      {friendRequest ? (
        <Text style={styles.requestText}>New Friend Request: {friendRequest}</Text>
      ) : (
        <Text style={styles.noRequestText}>No new friend requests</Text>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  requestText: {
    fontSize: 18,
    fontWeight: 'bold',
  },
  noRequestText: {
    fontSize: 16,
    color: 'gray',
  },
});

export default FriendRequestsScreen;
