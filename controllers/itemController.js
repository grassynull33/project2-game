var db = require('../models');

exports.checkItemsList = function (req, res, next) {
  var data = {
    items: []
  };

  db.Item.findAll().then(function (results) {
    for (var i = 0; i < results.length; i++) {
      data.items.push(results[i].dataValues);
    }

    // console.log(data.items);

    console.log('items list controller working');

    res.locals.items = data.items;

    next();
  }).catch(function (error) {
    console.log(error);

    console.log('items list controller error');
    next();
  });
};
