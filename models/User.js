var Sequelize = require('sequelize');

module.exports = function (sequelize, DataTypes) {
  var User = sequelize.define('User', {
    email: Sequelize.STRING
  });

  return User;
};
