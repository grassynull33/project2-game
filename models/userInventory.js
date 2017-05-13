module.exports = function(sequelize, DataTypes) {
  var UserInventory = sequelize.define('UserInventory', {
    username: {
      type: DataTypes.STRING,
      allowNull: false,
      validate: {
        len: [6]
      }
    },
    item1: {
      type: DataTypes.STRING,
      allowNull: false
    },
    item2: {
      type: DataTypes.STRING,
      allowNull: false
    },
    item3: {
      type: DataTypes.STRING,
      allowNull: false
    },
    item4: {
      type: DataTypes.STRING,
      allowNull: false
    },
    item5: {
      type: DataTypes.STRING,
      allowNull: false
    },
    score: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    health: {
      type: DataTypes.INTEGER,
      allowNull: false
    }
  });
  return UserInventory;
};