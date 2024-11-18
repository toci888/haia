import React, { useState, useEffect } from 'react';
import { getFriends, addFriend, removeFriend, searchUsers } from '../apiService';
import { getPendingInvitations, acceptInvitation, rejectInvitation } from '../apiService';
import './styles/Friends.css';

type FriendsProps = {
    userId: number;
};

interface User {
    id: number;
    username: string;
    firstName: string;
    lastName: string;
    email: string;
}

interface Invitation {
    id: number;
    userName: string;
}

const Friends = ({ userId }: FriendsProps) => {
    const [friends, setFriends] = useState<User[]>([]);
    const [newFriendId, setNewFriendId] = useState<string>('');
    const [searchQuery, setSearchQuery] = useState<string>('');
    const [searchResults, setSearchResults] = useState<User[]>([]);
    const [message, setMessage] = useState<string>('');
    const [invitations, setInvitations] = useState<Invitation[]>([]);

    useEffect(() => {
        fetchFriends();
        fetchInvitations();
    }, []);

    const fetchInvitations = async () => {
        try {
            const pendingInvitations: Invitation[] = await getPendingInvitations(userId);
            setInvitations(pendingInvitations);
            setMessage('');
        } catch (error) {
            setMessage('Błąd podczas pobierania zaproszeń.');
            console.error(error);
        }
    };

    const handleAccept = async (friendshipId: number) => {
        try {
            await acceptInvitation(friendshipId);
            setMessage('Zaproszenie zostało zaakceptowane.');
            fetchInvitations();
            fetchFriends();
        } catch (error) {
            setMessage('Błąd podczas akceptowania zaproszenia.');
            console.error(error);
        }
    };

    const handleReject = async (friendshipId: number) => {
        try {
            await rejectInvitation(friendshipId);
            setMessage('Zaproszenie zostało odrzucone.');
            fetchInvitations();
        } catch (error) {
            setMessage('Błąd podczas odrzucania zaproszenia.');
            console.error(error);
        }
    };

    const fetchFriends = async () => {
        try {
            const friendsList: User[] = await getFriends(userId);
            setFriends(friendsList);
        } catch (error) {
            setMessage('Błąd podczas pobierania listy znajomych.');
            console.error(error);
        }
    };

    const handleAddFriend = async (friendId: number) => {
        try {
            await addFriend(userId, friendId);
            setMessage('Znajomy został zaproszony!');
            fetchFriends();
            setSearchResults([]);
        } catch (error) {
            setMessage('Błąd podczas dodawania znajomego.');
            console.error(error);
        }
    };

    const handleRemoveFriend = async (friendId: number, FriendId: number) => {
        try {
            await removeFriend(friendId, FriendId);
            setMessage('Znajomy został usunięty!');
            fetchFriends();
        } catch (error) {
            setMessage('Błąd podczas usuwania znajomego.');
            console.error(error);
        }
    };

    const handleSearch = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const results: User[] = await searchUsers(searchQuery);
            setSearchResults(results);
            setMessage('');
        } catch (error) {
            setMessage('Błąd podczas wyszukiwania użytkowników.');
            console.error(error);
        }
    };

    return (
        <div className="friends-container">
            <h2>Twoi znajomi</h2>
            {message && <p className="friends-message">{message}</p>}

            <div className="friends-list">
                {friends.map((friend) => (
                    <span key={friend.id}>
                        <a href={`/profile?userId=${friend.id}`}>{friend.username}</a>
                        <button onClick={() => handleRemoveFriend(friend.id, 1)}>Usuń</button>
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

            <div className="invitations-container">
                <h2>Oczekujące zaproszenia</h2>
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
