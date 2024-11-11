import React, { useState, useEffect } from 'react';

const PostComponent = ({ postId, onInteractionEnd }) => {
  const [startTime, setStartTime] = useState(null);

  // Zapisz czas rozpoczęcia przeglądania przy załadowaniu komponentu
  useEffect(() => {
    setStartTime(Date.now());

    // Przy opuszczaniu komponentu (np. zamykaniu lub przejściu na inny post)
    return () => {
      const endTime = Date.now();
      const timeSpent = endTime - startTime;

      // Wywołanie funkcji przekazującej czas na backend
      onInteractionEnd(postId, timeSpent);
    };
  }, [postId]);

  return (
    <div>
      {/* Treść postu */}
    </div>
  );
};

export default PostComponent;
