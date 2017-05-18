var firebase = require('firebase-admin');
var express = require('express');
var router = express.Router();
var minigameController = require('../controllers/minigameController');
var achievementController = require('../controllers/achievementController');
var userController = require('../controllers/userController');

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
    // console.log(snapshot.key, snapshot.val().ItemName);

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

router.get('/',
  minigameController.checkMinigame,
  achievementController.checkAchievements
);

// Route for getting some data about our user to be used client side
router.get('/api/user_data', function (req, res) {
  if (!req.user) {
    // The user is not logged in, send back an empty object
    res.json({});
  } else {
    // Otherwise send back the user's email and id
    // Sending back a password, even a hashed password, isn't a good idea
    res.json({
      email: req.user.email,
      id: req.user.id,
      username: req.user.username
    });
  }
});

module.exports = router;
