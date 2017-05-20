var db = require('../models');

exports.checkAchievements = function (req, res, next) {
  console.log('ACHIEVEMENTS MIDDLEWARE');

  var superRareItem = 'Cha-Ching';

  var achievements = {
    hoarder: false,
    needle: false,
    quantity: false,
    collection: true,
    ultimate: true
  };

  var allItems = [];

  for (var i = 0; i < res.locals.wiki.length; i++) {
    allItems.push(res.locals.wiki[i].name);
  }

  // collection of related items achievement
  var collectionItems = [
    'Karunashian',
    'Sky Yen',
    'Yoonster',
    'N. Miller'
  ];

  var results = res.locals.items;
  var resultsItems = [];

  for (var i = 0; i < results.length; i++) {
    resultsItems.push(results[i].name);
  }

  for (var i = 0; i < results.length; i++) {
    if (collectionItems.indexOf(results[i].name) === -1 && achievements.collection === true) {
      achievements.collection = false;
    }

    // quantity overload
    if (results[i].greaterThanOne === true && achievements.quantity === false) {
      achievements.quantity = true;
    }

    // needle in a haystack
    if (results[i].name === superRareItem && achievements.needle === false) {
      achievements.needle = true;
    }
  }

  // hoarder
  if (results.length >= 40) {
    achievements.hoarder = true;
  }

  // ultimate collector
  for (var i = 0; i < allItems.length; i++) {
    if (resultsItems.indexOf(allItems[i]) === -1) {
      achievements.ultimate = false;
    }
  }

  res.locals.achievements = achievements;

  next();
};
