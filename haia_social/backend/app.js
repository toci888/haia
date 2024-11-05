// backend/app.js
const express = require('express');
const mongoose = require('mongoose');
const userRoutes = require('./routes/userRoutes');

const app = express();
app.use(express.json());

// Połączenie z MongoDB
mongoose.connect('mongodb://localhost:27017/haia_social', {
  useNewUrlParser: true,
  useUnifiedTopology: true,
});

// Routing
app.use('/api/users', userRoutes); // Upewnij się, że userRoutes jest poprawnie zdefiniowany

app.listen(3000, () => {
  console.log('Serwer działa na http://localhost:3000');
});
