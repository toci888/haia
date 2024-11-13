import React from 'react';
import { Link } from 'react-router-dom';
import './styles/Navbar.css';

const Navbar = () => {
  return (
    <nav className="navbar">
      <h1>HAIA</h1>
      <ul className="nav-links">
        <li><Link to="/">Strona główna</Link></li>
        <li><Link to="/suggestedposts">Sugerowane posty</Link></li>
        <li><Link to="/jokes">Zarty</Link></li>
        <li><Link to="/registerlogin">Zarejestruj/Zaloguj</Link></li>
        <li><Link to="/contact">Kontakt</Link></li>
      </ul>
    </nav>
  );
};

export default Navbar;
