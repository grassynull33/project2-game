'use strict';

module.exports = function (sequelize, DataTypes) {
  var Wiki = sequelize.define('Wiki', {
    name: {
      type: DataTypes.STRING,
      allowNull: false
    },
    description: {
      type: DataTypes.TEXT,
      allowNull: false
    },
    itemID: {
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
    },
    isLegendary: {
      type: DataTypes.BOOLEAN,
      allowNull: false
    }
  });
  return Wiki;
};
