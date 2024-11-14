import React, { useState } from 'react';
import { generateJoke } from '../apiService';
//import './styles/GenerateJoke.css';

const GenerateJoke = ({ userId }) => {
    const [joke, setJoke] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const handleGenerateJoke = async () => {
        setLoading(true);
        setError(null);
        try {
            const generatedJoke = await generateJoke(userId);
            setJoke(generatedJoke); // Zakładamy, że `generatedJoke` zawiera tekst dowcipu
        } catch (err) {
            setError("Nie udało się wygenerować dowcipu. Spróbuj ponownie później.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h2>Generator Dowcipów</h2>
            <button onClick={handleGenerateJoke} disabled={loading}>
                {loading ? 'Generowanie...' : 'Wygeneruj dowcip'}
            </button>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {joke && (
                <div>
                    <h3>Wygenerowany Dowcip:</h3>
                    <p>{joke}</p>
                </div>
            )}
        </div>
    );
};

export default GenerateJoke;
