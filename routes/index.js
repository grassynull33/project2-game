var express = require('express');
var router = express.Router();
var userController = require('../controllers/userController.js');

router.get('/register', userController.registerForm);

module.exports = router;
