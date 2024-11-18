import React, { useEffect, useState } from 'react';
import { getUserGroups, createUserGroup, deleteUserGroup } from './api';

interface UserGroup {
    id: number; // Assuming group ID is a number
    name: string; // Assuming group name is a string
    description: string; // Assuming group description is a string
}

const UserGroups: React.FC = () => {
    const [groups, setGroups] = useState<UserGroup[]>([]);
    const [groupName, setGroupName] = useState<string>("");

    useEffect(() => {
        fetchGroups();
    }, []);

    const fetchGroups = async () => {
        try {
            const data = await getUserGroups();
            setGroups(data);
        } catch (error) {
            console.error("Error fetching user groups:", error);
        }
    };

    const handleCreateGroup = async () => {
        try {
            await createUserGroup({ name: groupName, description: "Nowa grupa" });
            setGroupName("");
            fetchGroups();
        } catch (error) {
            console.error("Error creating user group:", error);
        }
    };

    const handleDeleteGroup = async (id: number) => {
        try {
            await deleteUserGroup(id);
            fetchGroups();
        } catch (error) {
            console.error("Error deleting user group:", error);
        }
    };

    return (
        <div>
            <h2>User Groups</h2>
            <input 
                value={groupName} 
                onChange={(e) => setGroupName(e.target.value)} 
                placeholder="Group Name" 
            />
            <button onClick={handleCreateGroup}>Add Group</button>
            <ul>
                {groups.map((group) => (
                    <li key={group.id}>
                        {group.name}
                        <button onClick={() => handleDeleteGroup(group.id)}>Delete</button>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default UserGroups;