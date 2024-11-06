import React, { useEffect, useState } from 'react';
import { getAllUsers } from '../services/userService';
import styles from './styles/UserList.module.css';

const UserList = () => {
    const [users, setUsers] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await getAllUsers();
                setUsers(data);
            } catch (error) {
                console.error('Failed to fetch users:', error);
            }
        };

        fetchData();
    }, []);

    return (
        <ul className="list-group">
            {users.map((user) => (
                <li key={user.id} className={`list-group-item ${styles.userItem}`}>
                    <span className={styles.username}>{user.username}</span>
                </li>
            ))}
        </ul>
    );
};

export default UserList;
