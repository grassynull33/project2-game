'use strict';

module.exports = function (sequelize, DataTypes) {
  var Guestbook = sequelize.define('Guestbook', {
    name: {
      type: DataTypes.STRING,
      allowNull: false
    },
    feedback: {
      type: DataTypes.TEXT,
      allowNull: false
    },
    username: {
      type: DataTypes.STRING,
      allowNull: false
    }
  });
  return Guestbook;
};