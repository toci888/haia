import React from 'react';
import { socialLogin } from '../apiService';
import './styles/SocialLogin.css';

const SocialLogin: React.FC = () => {
    const handleSocialLogin = async (provider: string) => {
        // For demonstration purposes, we set a sample user ID from the provider
        const providerUserId = `sample_${provider}_user_id`;
        try {
            const response = await socialLogin(provider, providerUserId);
            alert(`Logged in as: ${response.data.username}`);
        } catch (error) {
            alert("Error logging in with " + provider);
            console.error(error);
        }
    };

    return (
        <div className="social-login-container">
            <h2>Login via Social Media</h2>
            <div className="social-buttons">
                <button onClick={() => handleSocialLogin("Google")} className="google">Log in with Google</button>
                <button onClick={() => handleSocialLogin("Facebook")} className="facebook">Log in with Facebook</button>
                <button onClick={() => handleSocialLogin("Microsoft")} className="microsoft">Log in with Microsoft</button>
                <button onClick={() => handleSocialLogin("GitHub")} className="github">Log in with GitHub</button>
                <button onClick={() => handleSocialLogin("Apple")} className="apple">Log in with Apple</button>
                <button onClick={() => handleSocialLogin("LinkedIn")} className="linkedin">Log in with LinkedIn</button>
            </div>
        </div>
    );
};

export default SocialLogin;