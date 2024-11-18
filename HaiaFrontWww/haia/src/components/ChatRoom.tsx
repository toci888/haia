import React, { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';

interface Message {
    userId: string; // Adjust type based on your userId structure
    content: string;
    timestamp: string; // or Date, depending on how you handle timestamps
}

interface ChatRoomProps {
    roomId: string; // Adjust type based on your roomId structure
    userId: string; // Adjust type based on your userId structure
}

const ChatRoom: React.FC<ChatRoomProps> = ({ roomId, userId }) => {
    const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
    const [messages, setMessages] = useState<Message[]>([]);
    const [message, setMessage] = useState<string>('');

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
                    connection.invoke("JoinRoom", roomId); // Join the chat room
                    connection.on("ReceiveMessage", (userId: string, content: string, timestamp: string) => {
                        setMessages((messages) => [...messages, { userId, content, timestamp }]);
                    });
                })
                .catch(error => console.error("Connection failed:", error));
        }
    }, [connection, roomId]);

    const sendMessage = async () => {
        if (connection && message) {
            await connection.invoke("SendMessage", roomId, userId, message);
            setMessage('');
        }
    };

    return (
        <div>
            <h2>Room {roomId}</h2>
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
                placeholder="Type a message..."
            />
            <button onClick={sendMessage}>Send</button>
        </div>
    );
};

export default ChatRoom;