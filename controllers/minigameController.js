var db = require('../models');

exports.checkMinigame = function (req, res, next) {
  console.log('minigame controller passing thru');

  next();
};
