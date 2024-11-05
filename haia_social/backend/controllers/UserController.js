// backend/controllers/UserController.js
const User = require('../models/User');
const bcrypt = require('bcryptjs');

module.exports = {
  async register(req, res) {
    const { username, email, password } = req.body;

    try {
      const hashedPassword = await bcrypt.hash(password, 10);
      const user = new User({ username, email, password: hashedPassword });
      await user.save();
      res.status(201).json({ message: 'Użytkownik zarejestrowany pomyślnie' });
    } catch (error) {
      res.status(400).json({ message: 'Błąd rejestracji', error });
    }
  },

  async login(req, res) {
    const { email, password } = req.body;

    try {
      const user = await User.findOne({ email });
      if (!user) return res.status(404).json({ message: 'Nie znaleziono użytkownika' });

      const isPasswordValid = await bcrypt.compare(password, user.password);
      if (!isPasswordValid) return res.status(401).json({ message: 'Błędne dane logowania' });

      res.status(200).json({ message: 'Logowanie udane', user });
    } catch (error) {
      res.status(500).json({ message: 'Błąd serwera', error });
    }
  },
};
