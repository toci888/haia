import React, { useState, useEffect } from 'react';

interface PostComponentProps {
    postId: number; // Assuming postId is a number
    onInteractionEnd: (postId: number, timeSpent: number) => void; // Function type for the interaction end callback
}

const PostComponent: React.FC<PostComponentProps> = ({ postId, onInteractionEnd }) => {
    const [startTime, setStartTime] = useState<number | null>(null);

    // Save the start time when the component mounts
    useEffect(() => {
        setStartTime(Date.now());
        
        // When leaving the component (e.g., closing or navigating to another post)
        return () => {
            const endTime = Date.now();
            if (startTime !== null) {
                const timeSpent = endTime - startTime;
                // Call the function passing the time spent to the backend
                onInteractionEnd(postId, timeSpent);
            }
        };
    }, [postId, onInteractionEnd, startTime]);

    return (
        <div>
            {/* Post content */}
        </div>
    );
};

export default PostComponent;