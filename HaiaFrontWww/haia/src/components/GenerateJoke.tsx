import React, { useState } from 'react';
import { generateJoke } from '../apiService';

interface GenerateJokeProps {
    userId: string; // Adjust type based on your userId structure
}

const GenerateJoke: React.FC<GenerateJokeProps> = ({ userId }) => {
    const [joke, setJoke] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const handleGenerateJoke = async () => {
        setLoading(true);
        setError(null);
        try {
            const generatedJoke = await generateJoke(userId);
            setJoke(generatedJoke); // Assuming `generatedJoke` contains the joke text
        } catch (err) {
            setError("Failed to generate a joke. Please try again later.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h2>Joke Generator</h2>
            <button onClick={handleGenerateJoke} disabled={loading}>
                {loading ? 'Generating...' : 'Generate Joke'}
            </button>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {joke && (
                <div>
                    <h3>Generated Joke:</h3>
                    <p>{joke}</p>
                </div>
            )}
        </div>
    );
};

export default GenerateJoke;