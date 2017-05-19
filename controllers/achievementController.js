var db = require('../models');

exports.checkAchievements = function (req, res, next) {
  console.log('ACHIEVEMENTS MIDDLEWARE');

  var superRareItem = 'Archon_Helm';

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

  var results = res.locals.items;

  for (var i = 0; i < results.length; i++) {
    if (allItems.indexOf(results[i].name) === -1) {
      collectedAll = false;
    }

    if (collectionItems.indexOf(results[i].name) === -1) {
      achievements.collection = false;
    }

    // quantity overload
    if (results[i].greaterThanOne === true) {
      achievements.quantity = true;
    }

    // needle in a haystack
    if (results[i].name === superRareItem) {
      achievements.needle = true;
    }
  }

  // hoarder
  if (results.length >= 40) {
    achievements.hoarder = true;
  }

  // ultimate collector
  if (collectedAll) {
    achievements.ultimate = true;
  }

  // console.log(achievements);
  // res.render('index', achievements);
  res.locals.achievements = achievements;

  next();
};
