import React from 'react';
import { socialLogin } from '../apiService';
import './styles/SocialLogin.css';

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
        <div className="social-login-container">
            <h2>Logowanie przez Social Media</h2>
            <div className="social-buttons">
                <button onClick={() => handleSocialLogin("Google")} className="google">Zaloguj przez Google</button>
                <button onClick={() => handleSocialLogin("Facebook")} className="facebook">Zaloguj przez Facebook</button>
                <button onClick={() => handleSocialLogin("Microsoft")} className="microsoft">Zaloguj przez Microsoft</button>
                <button onClick={() => handleSocialLogin("GitHub")} className="github">Zaloguj przez GitHub</button>
                <button onClick={() => handleSocialLogin("Apple")} className="apple">Zaloguj przez Apple</button>
                <button onClick={() => handleSocialLogin("LinkedIn")} className="linkedin">Zaloguj przez LinkedIn</button>
            </div>
        </div>
    );
};

export default SocialLogin;
