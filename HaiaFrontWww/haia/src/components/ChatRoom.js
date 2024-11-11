import React, { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';

const ChatRoom = ({ roomId, userId }) => {
  const [connection, setConnection] = useState(null);
  const [messages, setMessages] = useState([]);
  const [message, setMessage] = useState('');

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://your-backend-url/chatHub")
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection.start()
        .then(() => {
          console.log("Connected to SignalR");
          connection.invoke("JoinRoom", roomId); // Dołączenie do pokoju
          connection.on("ReceiveMessage", (userId, content, timestamp) => {
            setMessages((messages) => [...messages, { userId, content, timestamp }]);
          });
        })
        .catch(error => console.error("Connection failed:", error));
    }
  }, [connection]);

  const sendMessage = async () => {
    if (connection && message) {
      await connection.invoke("SendMessage", roomId, userId, message);
      setMessage('');
    }
  };

  return (
    <div>
      <h2>Pokój {roomId}</h2>
      <div>
        {messages.map((msg, index) => (
          <div key={index}>
            <strong>{msg.userId}:</strong> {msg.content} <em>({new Date(msg.timestamp).toLocaleTimeString()})</em>
          </div>
        ))}
      </div>
      <input
        type="text"
        value={message}
        onChange={(e) => setMessage(e.target.value)}
        placeholder="Wpisz wiadomość..."
      />
      <button onClick={sendMessage}>Wyślij</button>
    </div>
  );
};

export default ChatRoom;
