import React, { useEffect, useState } from 'react';
import { getUserGroups, createUserGroup, deleteUserGroup } from './api';

const UserGroups = () => {
  const [groups, setGroups] = useState([]);
  const [groupName, setGroupName] = useState("");

  useEffect(() => {
    fetchGroups();
  }, []);

  const fetchGroups = async () => {
    const data = await getUserGroups();
    setGroups(data);
  };

  const handleCreateGroup = async () => {
    await createUserGroup({ name: groupName, description: "Nowa grupa" });
    setGroupName("");
    fetchGroups();
  };

  const handleDeleteGroup = async (id) => {
    await deleteUserGroup(id);
    fetchGroups();
  };

  return (
    <div>
      <h2>Grupy Użytkowników</h2>
      <input value={groupName} onChange={(e) => setGroupName(e.target.value)} placeholder="Nazwa grupy" />
      <button onClick={handleCreateGroup}>Dodaj Grupę</button>
      <ul>
        {groups.map((group) => (
          <li key={group.id}>
            {group.name}
            <button onClick={() => handleDeleteGroup(group.id)}>Usuń</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default UserGroups;
