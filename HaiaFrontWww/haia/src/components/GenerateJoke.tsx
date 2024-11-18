import React, { useState } from 'react';
import { generateJoke } from '../apiService';

interface GenerateJokeProps {
    userId: number; // Zakładam, że userId jest liczbą. Jeśli jest inny typ, zmień go odpowiednio
}

interface Joke {
    id: number;
    text: string;
    createdAt: string; // Assuming createdAt is in string format (ISO date)
}


const GenerateJoke: React.FC<GenerateJokeProps> = ({ userId }) => {
    const [joke, setJoke] = useState<Joke>();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const handleGenerateJoke = async () => {
        setLoading(true);
        setError(null);

        try {
            const generatedJoke: Joke = await generateJoke(userId); // Typujemy odpowiedź jako string
            setJoke(generatedJoke); // Przechowujemy dowcip w stanie
        } catch (err) {
            setError('Failed to generate a joke. Please try again later.');
            console.error('Error generating joke:', err);
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
                    <p>{joke.text}</p>
                </div>
            )}
        </div>
    );
};

export default GenerateJoke;
