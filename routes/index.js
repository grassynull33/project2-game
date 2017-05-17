
var firebase = require('firebase-admin');
var express = require('express');
var router = express.Router();

var db = require('../models');

var serviceAccount = require('../project2-4eb1dfda9ce9.json');

firebase.initializeApp({
  credential: firebase.credential.cert(serviceAccount),
  databaseURL: 'https://project2-e15c9.firebaseio.com'
});

router.get('/firebase', function (req, res) {
  var fireDB = firebase.database();
  var ref = fireDB.ref('Inventory');

  ref.on('child_added', function (snapshot) {
    console.log(snapshot.key, snapshot.val().ItemName);

    db.Item.create({
      name: snapshot.val().ItemName,
      description: snapshot.val().Description,
      slotID: snapshot.val().SlotID,
      greaterThanOne: snapshot.val().GreaterThanOne,
      uniqueID: snapshot.key,
      hasDurability: snapshot.val().HasDurability,
      isBlueprint: snapshot.val().IsBlueprint,
      isCraftable: snapshot.val().IsCraftable
    }).catch(function (err) {
      console.log(err);
      console.log('duplicate: did not insert');
    });
      // .then(function () {
      //   res.send({redirect: '/'});
      // }).catch(function (err) {
      //   res.json(err);
      // });
  });
});

router.get('/', function (req, res) {
  console.log('GET REQUEST IN INDEX.jS (ROUTES)');

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
});

module.exports = router;
