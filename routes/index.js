var express = require('express');
var router = express.Router();
var userController = require('../controllers/userController.js');

router.get('/register', userController.registerForm);
router.post('/register', userController.register);

module.exports = router;
