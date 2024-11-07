import Header from "../layout/Header";
import Register from "../components/Register";
import SocialLogin from "../components/SocialLogin";
import UserProfile from "../components/UserProfile";

function LoginPage() {
    return (
        <div>
            <Header/>
            <div className="container mt-4">
                <div className="container">
                    <h1>Portal Haia</h1>
                    <Register />
                    <hr />
                    <SocialLogin />
                    <hr />
                    <UserProfile />
                </div>
            </div>
        </div>
    )
}

export default LoginPage;