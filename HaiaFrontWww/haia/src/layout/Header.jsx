import { Link } from "react-router-dom";

function Header() {
  return (
    <div className="header__container">
      <p className="logo">HAIA</p>
      <nav className="navbar__container">
        <Link to="/" className="navbar__link">
          Home
        </Link>
        {/* <Link to="/community" className="navbar__link">
          Community
        </Link> */}
        <Link to="/humor" className="navbar__link">
          Humor
        </Link>
        <Link to="/about" className="navbar__link">
          About us
        </Link>
        <Link to="/contact" className="navbar__link">
          Contact
        </Link>
        <Link to="/login" className="navbar__link">
          Login
        </Link>
      </nav>
    </div>
  );
}

export default Header;
