'use strict';

module.exports = function (sequelize, DataTypes) {
  var Item = sequelize.define('Item', {
    name: {
      type: DataTypes.STRING,
      allowNull: false
    },
    description: {
      type: DataTypes.TEXT,
      allowNull: false
    },
    slotID: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    greaterThanOne: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    },
    uniqueID: {
      type: DataTypes.INTEGER,
      allowNull: false,
      unique: true
    },
    hasDurability: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    },
    isBlueprint: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    },
    isCraftable: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    }
  });
  return Item;
};
