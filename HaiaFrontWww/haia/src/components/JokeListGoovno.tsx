import React, { useEffect } from 'react';
import './styles/PostList.css';
import JokeItem from './JokeItem';

interface Reaction {
    reactionType: string;
}

interface Joke {
    id: number;
    reactions: Reaction[];
    // Add other properties as needed
}

interface JokeListGoovnoProps {
    jokes: Joke[];
}

const JokeListGoovno: React.FC<JokeListGoovnoProps> = ({ jokes }) => {
    useEffect(() => {
        // Initialize reactions for each post
        const initialReactions = jokes.reduce((acc: Record<number, Record<string, number>>, post: Joke) => {
            acc[post.id] = {
                funny: post.reactions.filter(r => r.reactionType === 'funny').length,
                super: post.reactions.filter(r => r.reactionType === 'super').length,
                dry: post.reactions.filter(r => r.reactionType === 'dry').length,
                dontCare: post.reactions.filter(r => r.reactionType === 'dontCare').length,
            };
            return acc;
        }, {});

        // You can set the reactions state here if needed
        // setReactions(initialReactions);
        // Initialize comments for each post
        // jokes.forEach(post => fetchComments(post.id));
    }, [jokes]);

    return (
        <div className="post-list">
            {jokes.map((joke) => (
                <JokeItem key={joke.id} post={joke} />
            ))}
        </div>
    );
};

export default JokeListGoovno;