var db = require('../models');

exports.checkAchievements = function (req, res) {
  var achievements = {
    hoarder: false,
    needle: false,
    quantity: false,
    collection: true,
    ultimate: false
  };

  var data = {
    items: []
  };

  var collectedAll = true;

  var allItems = [
    'Cube of Destruction',
    'Archon_Helm',
    'Icelandic Glacial Spikes',
    'Popa Pola',
    'W00T'
    // fill in all possible items
  ];

  // collection of related items achievement
  var collectionItems = [
    'Baseball',
    'Bat',
    'Catcher\'s Mitt'
  ];

  db.Item.findAll().then(function (results) {
    for (var i = 0; i < results.length; i++) {
      data.items.push(results[i].dataValues);
      if (allItems.indexOf(results[i].dataValues.name) === -1) {
        collectedAll = false;
      }

      if (collectionItems.indexOf(results[i].dataValues.name) === -1) {
        achievements.collection = false;
      }

      // hoarder
      if (results.length >= 40) {
        achievements.hoarder = true;
      }

      // needle in a haystack
      if (results[i].dataValues.name === 'Super Rare Item') {
        achievements.needle = true;
      }

      db.Item.count({ where: {'name': results[i].dataValues.name}}).then(function (count) {
        console.log('there are ' + count);
        if (count > 4) {
          achievements.quantity = true;
        }
      });
    }
  }).then(function () {
    // ultimate collector
    if (collectedAll) {
      achievements.ultimate = true;
    }

    // console.log(data);
    console.log(achievements);
    res.render('index', achievements);
  });
};
