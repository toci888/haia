import React, { useState, useEffect } from 'react';
import { getFriends, addFriend, removeFriend, searchUsers } from '../apiService';
import { getPendingInvitations, acceptInvitation, rejectInvitation } from '../apiService';

import './styles/Friends.css';

const Friends = ({ userId }) => {
    const [friends, setFriends] = useState([]);
    const [newFriendId, setNewFriendId] = useState('');
    const [searchQuery, setSearchQuery] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [message, setMessage] = useState('');
    const [invitations, setInvitations] = useState([]);


    useEffect(() => {
        fetchFriends();
        fetchInvitations();
    }, []);

    const fetchInvitations = async () => {
        try {
            const pendingInvitations = await getPendingInvitations(userId);
            setInvitations(pendingInvitations);
            setMessage('');
        } catch (error) {
            setMessage("Błąd podczas pobierania zaproszeń.");
            console.error(error);
        }
    };

    const handleAccept = async (friendshipId) => {
        try {
            await acceptInvitation(friendshipId);
            setMessage("Zaproszenie zostało zaakceptowane.");
            fetchInvitations(); // Odśwież listę zaproszeń
        } catch (error) {
            setMessage("Błąd podczas akceptowania zaproszenia.");
            console.error(error);
        }
    };

    const handleReject = async (friendshipId) => {
        try {
            await rejectInvitation(friendshipId);
            setMessage("Zaproszenie zostało odrzucone.");
            fetchInvitations(); // Odśwież listę zaproszeń
        } catch (error) {
            setMessage("Błąd podczas odrzucania zaproszenia.");
            console.error(error);
        }
    };

    const fetchFriends = async () => {
        try {
            const friendsList = await getFriends(userId);
            setFriends(friendsList);
        } catch (error) {
            setMessage("Błąd podczas pobierania listy znajomych");
            console.error(error);
        }
    };

    const handleAddFriend = async (friendId) => {
        try {
            await addFriend(userId, friendId);
            setMessage("Znajomy został zaproszony!");
            fetchFriends(); // Odśwież listę znajomych
            setSearchResults([]); // Wyczyść wyniki wyszukiwania
        } catch (error) {
            setMessage("Błąd podczas dodawania znajomego");
            console.error(error);
        }
    };

    const handleRemoveFriend = async (friendId) => {
        try {
            await removeFriend(userId, friendId);
            setMessage("Znajomy został usunięty!");
            fetchFriends(); // Odśwież listę znajomych
        } catch (error) {
            setMessage("Błąd podczas usuwania znajomego");
            console.error(error);
        }
    };

    const handleSearch = async (e) => {
        e.preventDefault();
        try {
            const results = await searchUsers(searchQuery);
            setSearchResults(results);
            setMessage('');
        } catch (error) {
            setMessage("Błąd podczas wyszukiwania użytkowników");
            console.error(error);
        }
    };

    return (
        <div>
        <div className="friends-container">
            <h2>Twoi znajomi</h2>
            {message && <p className="friends-message">{message}</p>}
            
            <div className="friends-list">
                {friends.map((friend) => (

                <span key={friend.id}>
                     {friend.userName} 
                    <button onClick={() => handleRemoveFriend(friend.id)}>Usuń</button>
                </span>
                ))}
            </div>

            <div className="add-friend">
                <h3>Wyszukaj użytkowników</h3>
                <form onSubmit={handleSearch}>
                    <input
                        type="text"
                        placeholder="Wpisz imię lub nazwisko"
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                    />
                    <button type="submit">Szukaj</button>
                </form>
            </div>

            <ul className="search-results">
                {searchResults.map((user) => (
                    <li key={user.id}>
                        {user.firstName} {user.lastName} ({user.username})
                        <button onClick={() => handleAddFriend(user.id)}>Zaproś znajomego</button>
                    </li>
                ))}
            </ul>
        </div>
        <div className="invitations-container">
            <h2>Oczekujące zaproszenia</h2>
            {message && <p className="invitations-message">{message}</p>}
            <ul className="invitations-list">
                {invitations.map((invitation) => (
                    <li key={invitation.id}>
                        {invitation.userName}
                        <div className="invitation-actions">
                            <button onClick={() => handleAccept(invitation.id)}>Akceptuj</button>
                            <button onClick={() => handleReject(invitation.id)}>Odrzuć</button>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
        </div>
    );
};

export default Friends;
