// FriendInviteNotifier.js
import React, { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";

const FriendInviteNotifier = () => {
  const [inviteCount, setInviteCount] = useState(0);
  const [showInvites, setShowInvites] = useState(false);
  const [invites, setInvites] = useState([]);

  useEffect(() => {
    // Inicjalizacja połączenia z hubem SignalR
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://80.209.230.198:5295/friendRequestHub", {
        withCredentials: true // Enable credentials in the SignalR client
      })
      .withAutomaticReconnect()
      .build();

    // Startowanie połączenia
    connection
      .start()
      .then(() => console.log("SignalR Connected"))
      .catch((error) => console.error("SignalR Connection Error:", error));

    // Obsługa zdarzenia 'ReceiveFriendInvite'
    connection.on("ReceiveFriendInvite", (inviterName) => {
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
