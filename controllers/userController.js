var db = require('../models');
var express = require('express');
var router = express.Router();
var passport = require('../config/passport');
var isAuthenticated = require('../config/middleware/isAuthenticated');

router.get('/signout', function (req, res) {
  req.logout();
  res.redirect('/');
});

module.exports = router;
