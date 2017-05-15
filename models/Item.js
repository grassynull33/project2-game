'use strict';

module.exports = function (sequelize, DataTypes) {
  var Item = sequelize.define('Item', {
    name: {
      type: DataTypes.STRING,
      allowNull: false
    },
    description: {
      type: DataTypes.STRING,
      allowNull: false
    },
    slotID: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    moreThanOne: {
      type: DataTypes.STRING,
      allowNull: false
    },
    uniqueID: {
      type: DataTypes.STRING,
      allowNull: false,
      unique: true
    }
  });
  return Item;
};
