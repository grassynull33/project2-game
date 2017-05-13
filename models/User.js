var Sequelize = require('sequelize');
var passportLocalSequelize = require('passport-local-sequelize');

module.exports = function (sequelize, DataTypes) {
  var User = sequelize.define('User', {
    email: Sequelize.STRING,
    hash: Sequelize.STRING,
    salt: Sequelize.STRING
  });

  passportLocalSequelize.attachToUser(User, {
    usernameField: 'email',
    hashField: 'hash',
    saltField: 'salt'
  });

  return User;
};
