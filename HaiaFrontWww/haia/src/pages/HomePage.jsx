import Header from "../layout/Header";
import JokeLandingPage from "../components/JokeLandingPage";
import ComedyTextList from "../components/ComedyTextList";
import UserList from "../components/UserList";
import Joke from "../components/Joke";

function HomePage() {
  return (
    <div>
      <Header />

            <div className="home-body__container">
                <div>
                    <JokeLandingPage />
                </div>
                <h1 className="text-center mb-4">Comedy App</h1>
                <div className="row">
                    <div className="col-md-6">
                        <div className="card p-3 mb-4">
                            <h2 className="text-primary">Comedy Texts</h2>
                            <ComedyTextList />
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div className="card p-3 mb-4">
                            <h2 className="text-success">Users</h2>
                            <UserList />
                        </div>
                    </div>
                </div>
            </div>
    </div>
  );
}

export default HomePage;
