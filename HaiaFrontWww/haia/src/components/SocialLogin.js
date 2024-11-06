import React from 'react';
import { socialLogin } from '../apiService';

const SocialLogin = () => {
    const handleSocialLogin = async (provider) => {
        // Na potrzeby przykładu ustawiamy przykładowy ID użytkownika z platformy
        const providerUserId = `sample_${provider}_user_id`;

        try {
            const response = await socialLogin(provider, providerUserId);
            alert(`Zalogowano: ${response.data.username}`);
        } catch (error) {
            alert("Błąd logowania przez " + provider);
            console.error(error);
        }
    };

    return (
        <div>
            <h2>Logowanie przez Social Media</h2>
            <button onClick={() => handleSocialLogin("Google")}>Zaloguj przez Google</button>
            <button onClick={() => handleSocialLogin("Facebook")}>Zaloguj przez Facebook</button>
            <button onClick={() => handleSocialLogin("Microsoft")}>Zaloguj przez Microsoft</button>
            <button onClick={() => handleSocialLogin("GitHub")}>Zaloguj przez GitHub</button>
            <button onClick={() => handleSocialLogin("Apple")}>Zaloguj przez Apple</button>
            <button onClick={() => handleSocialLogin("LinkedIn")}>Zaloguj przez LinkedIn</button>
        </div>
    );
};

export default SocialLogin;
