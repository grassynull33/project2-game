var db = require('../models');

exports.checkGuestBook = function (req, res, next) {
  var data = {
    items: []
  };

  db.Guestbooks.findAll().then(function (results) {
    for (var i = 0; i < results.length; i++) {
      data.items.push(results[i].dataValues);
    }

    console.log(data.items);

    console.log('guestbook controller working');

    res.locals.items = data.items;

    next();
  }).catch(function (error) {
    console.log(error);

    console.log('guestbook controller error');
    next();
  });
};

