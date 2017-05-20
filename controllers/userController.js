var db = require('../models');
var express = require('express');
var router = express.Router();
var passport = require('../config/passport');
var isAuthenticated = require('../config/middleware/isAuthenticated');

module.exports = router;
