// FriendInviteNotifier.js
import React, { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";

const FriendInviteNotifier = () => {
  const [inviteCount, setInviteCount] = useState(0);
  const [showInvites, setShowInvites] = useState(false);
  const [invites, setInvites] = useState([]);
  const [connection, setConnection] = useState(null);
  const [messages, setMessages] = useState([]);
  const [inputMessage, setInputMessage] = useState("");

  useEffect(() => {
    // Inicjalizacja połączenia z hubem SignalR
    const newConnection  = new signalR.HubConnectionBuilder()
      .withUrl("http://80.209.230.198:5295/friendRequestHub", {
        withCredentials: true,
        transport: signalR.HttpTransportType.WebSockets // Enable credentials in the SignalR client
      })
      .configureLogging(signalR.LogLevel.Information)
      .withAutomaticReconnect()
      .build();

      setConnection(newConnection);

    // Startowanie połączenia
    newConnection
      .start();
      //.then(() => ;
      //connection.invoke("SendFriendInviteNotification", 17, 'Waryjot'))
      //.catch((error) => console.error("SignalR Connection Error :( ):", error));


      try {
        newConnection.invoke("SendFriendInviteNotification", 23, 'Waryjot');
        console.log(`Friend invite sent to user 17`);
      } catch (error) {
        console.error("Error calling SendFriendInviteNotification: ", error);
      }

      newConnection.onclose(error => {
        console.error("Connection closed with error:", error);
        if (error) {
          console.error("Detailed error:", error.message);
        }
      });


        // Obsługa zdarzenia 'ReceiveFriendInvite'
        newConnection.on("SendFriendInviteNotification", (inviterId, inviterName) => {
        setInviteCount((prevCount) => prevCount + 1);
        setInvites((prevInvites) => [...prevInvites, inviterName]);
        });

   
    return () => {
      //connection.stop();
    };
  }, []);

  const handleIconClick = () => {
    setShowInvites(!showInvites);

    if (showInvites) {
      setInviteCount(0); // Resetowanie licznika po zamknięciu listy
    }
  };
  

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log("Connected to SignalR");

          // Listen for messages from the hub
          connection.on("ReceiveMessage", (user, message) => {
            setMessages((prevMessages) => [...prevMessages, `${user}: ${message}`]);
          });
        })
        .catch((error) => console.error("Connection failed: ", error));
    }
  }, [connection]);

  // Function to call the SignalR method on the hub
  const sendMessage = async () => {
    if (connection && connection.state === signalR.HubConnectionState.Connected) {
      try {
        await connection.invoke("SendFriendInviteNotification", 24, inputMessage); // Replace with your method
        setInputMessage(""); // Clear the input field
      } catch (error) {
        console.error("Error sending message xd: ", error);
      }
    } else {
      console.warn("Connection is not in the 'Connected' state.");
    }
  };

sendMessage();

//   // Function to call the SignalR method on the hub
//   async function sendFriendInvite(recipientUserId, inviterName) {
    
//   }

//   sendFriendInvite(17, "waryjot");

  return (
    <div className="notification-icon">
      <span onClick={handleIconClick}>🔔</span>
      {inviteCount > 0 && <span className="invite-counter">{inviteCount}</span>}
      {showInvites && (
        <div className="invite-list">
          {invites.length > 0 ? (
            invites.map((inviterName, index) => (
              <div key={index} className="invite-item">
                Zaproszenie od: {inviterName}
              </div>
            ))
          ) : (
            <div>Brak nowych zaproszeń</div>
          )}
        </div>
      )}
    </div>
  );
};

export default FriendInviteNotifier;
