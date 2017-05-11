var db = require('../models');

exports.registerForm = function (req, res) {
  res.render('index');
};

exports.register = function (req, res) {
  console.log(req.body);
  db.User.create({
    email: req.body.email,
    password: req.body.password
  }).then(function () {
    res.send('WHOA');
  });
};
