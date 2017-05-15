
var firebase = require('firebase-admin');
var models = require('../models');
var express = require('express');
var router = express.Router();

var db = require('../models');

var Sequelize = require('sequelize');
var sequelize = new Sequelize('project2', 'root', 'Passw0rd');

// Firebase testing
var uniqueID = [];
var name = [];
var slotID = [];
var description = [];
var moreThanOne = [];

var initialLoadFromGame = false;
var nameLoaded = false;
var slotIDLoaded = false;
var descriptionLoaded = false;
var moreThanOneLoaded = false;

// allitemsafter
var uniqueIDAfterLoad = [];
var nameAfterLoad = [];
var slotIDAfterLoad = [];
var descriptionAfterLoad = [];
var moreThanOneAfterLoad = [];

// var counter = 0;

var serviceAccount = require('../project2-4eb1dfda9ce9.json');

firebase.initializeApp({
  credential: firebase.credential.cert(serviceAccount),
  databaseURL: 'https://project2-e15c9.firebaseio.com'
});

router.get('/', function (req, res) {
  console.log('GET REQUEST IN INDEX.jS (ROUTES)');
  res.render('index');
});

router.get('/firebase', function (req, res) {
  var fireDB = firebase.database();
  var ref = fireDB.ref('Testing');

  var nameRef = fireDB.ref('Testing/name');
  var slotIDRef = fireDB.ref('Testing/slotID');
  var descriptionRef = fireDB.ref('Testing/description');
  var moreThanOneRef = fireDB.ref('Testing/More than One in Inventory');

  // initial load / detects items already in firebase
  ref.on('child_added', function (snapshot) {
    if (!initialLoadFromGame) {
      if (snapshot.key === 'name') {
        for (var key in snapshot.val()) {
          name.push(snapshot.val()[key]);
          uniqueID.push(key);
          nameLoaded = true;
        }
      } else if (snapshot.key === 'description') {
        for (var key in snapshot.val()) {
          description.push(snapshot.val()[key]);
          descriptionLoaded = true;
        }
      } else if (snapshot.key === 'slotID') {
        for (var key in snapshot.val()) {
          slotID.push(snapshot.val()[key]);
          slotIDLoaded = true;
        }
      } else if (snapshot.key === 'More than One in Inventory') {
        for (var key in snapshot.val()) {
          moreThanOne.push(snapshot.val()[key]);
          moreThanOneLoaded = true;
        }
      }

      if (nameLoaded && slotIDLoaded && descriptionLoaded && moreThanOneLoaded) {
        initialLoadFromGame = true;

        for (var i = 0; i < name.length; i++) {
          console.log((i + 1) + '. Item Name: ' + name[i] + ' | Description: ' + description[i] + ' | slotID: ' + slotID[i] + ' | moreThanOne: ' + moreThanOne[i] + ' | uID: ' + uniqueID[i]);

          db.Item.create({
            name: name[i],
            description: description[i],
            slotID: slotID[i],
            moreThanOne: moreThanOne[i],
            uniqueID: uniqueID[i]
          }).catch(function (err) {
            console.log('duplicate: did not insert');
          });
            // .then(function () {
            //   res.send({redirect: '/'});
            // }).catch(function (err) {
            //   res.json(err);
            // });
        }
      }
    }
  });

  nameRef.on('child_added', function (snapshot) {
    if (initialLoadFromGame) {
      nameAfterLoad.push(snapshot.val());
      uniqueIDAfterLoad.push(snapshot.key);
    }
  });

  slotIDRef.on('child_added', function (snapshot) {
    if (initialLoadFromGame) {
      slotIDAfterLoad.push(snapshot.val());
    }
  });

  descriptionRef.on('child_added', function (snapshot) {
    if (initialLoadFromGame) {
      descriptionAfterLoad.push(snapshot.val());
    }
  });

  moreThanOneRef.on('child_added', function (snapshot) {
    if (initialLoadFromGame) {
      moreThanOneAfterLoad.push(snapshot.val());
    }
  });

  ref.on('child_changed', function (snapshot) {
    if (initialLoadFromGame) {
      for (var i = 0; i < nameAfterLoad.length; i++) {
        console.log((i + 1) + '. Item Name: ' + nameAfterLoad[i] + ' | Description: ' + descriptionAfterLoad[i] + ' | slotID: ' + slotIDAfterLoad[i] + ' | moreThanOne: ' + moreThanOneAfterLoad[i] + ' | uID: ' + uniqueIDAfterLoad[i]);

        db.Item.create({
          name: nameAfterLoad[i],
          description: descriptionAfterLoad[i],
          slotID: slotIDAfterLoad[i],
          moreThanOne: moreThanOneAfterLoad[i],
          uniqueID: uniqueIDAfterLoad[i]
        }).catch(function (err) {
          console.log('duplicate: did not insert');
        });
          // .then(function () {
          //   res.send({redirect: '/'});
          // }).catch(function (err) {
          //   res.json(err);
          // });
      }
    }
  });
});

module.exports = router;
