var firebase = require('firebase-admin');
var models = require('../models');
var express = require('express');
var router = express.Router();

// Firebase testing
var name = [];
var slotID = [];
var description = [];
var moreThanOne = [];

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
  var db = firebase.database();
  var ref = db.ref('Testing');

  var nameRef = db.ref('Testing/name');
  var slotIDRef = db.ref('Testing/slotID');
  var descriptionRef = db.ref('Testing/description');
  var moreThanOneRef = db.ref('Testing/More than One in Inventory');

  ref.on('child_added', function (snapshot) {
    // console.log(snapshot.key, snapshot.val());
    // counter++;
    // console.log(snapshot.key);
    // console.log(snapshot.val());
    // add to array for each type of data
    if (snapshot.key === 'name') {
      for (var key in snapshot.val()) {
        name.push(snapshot.val()[key]);
      }
    } else if (snapshot.key === 'description') {
      for (var key in snapshot.val()) {
        description.push(snapshot.val()[key]);
      }
    } else if (snapshot.key === 'slotID') {
      for (var key in snapshot.val()) {
        slotID.push(snapshot.val()[key]);
      }
    } else if (snapshot.key === 'More than One in Inventory') {
      for (var key in snapshot.val()) {
        moreThanOne.push(snapshot.val()[key]);
      }
    }
    // parse arr
    // sequelize
    // check if unique id exists so no duplicate rows are added?
    // add rows to table
    // console.log(counter);
    // console.log(name);
    // console.log(description);
    // console.log(slotID);
    // console.log(moreThanOne);
    if (name.length > 0 && description.length > 0 && slotID.length > 0 && moreThanOne.length > 0) {
      for (var i = 0; i < name.length; i++) {
        console.log('Item Name: ' + name[i] + ' | Description: ' + description[i] + ' | slotID: ' + slotID[i] + ' | moreThanOne: ' + moreThanOne[i]);
      }
    }
  });

  // nameRef.on('child_added', function (snapshot) {
  //   console.log(snapshot.key, snapshot.val());

  //   // add to array for each type of data
  //   name.push(snapshot.val());
  //   // parse arr
  //   // sequelize
  //   // check if unique id exists so no duplicate rows are added?
  //   // add rows to table
  //   console.log(name);
  // });

  // slotIDRef.on('child_added', function (snapshot) {
  //   console.log(snapshot.key, snapshot.val());

  //   // add to array for each type of data
  //   slotID.push(snapshot.val());
  //   // parse arr
  //   // sequelize
  //   // check if unique id exists so no duplicate rows are added?
  //   // add rows to table
  //   console.log(slotID);
  // });

  // descriptionRef.on('child_added', function (snapshot) {
  //   console.log(snapshot.key, snapshot.val());

  //   // add to array for each type of data
  //   description.push(snapshot.val());
  //   // parse arr
  //   // sequelize
  //   // check if unique id exists so no duplicate rows are added?
  //   // add rows to table
  //   console.log(description);
  // });

  // moreThanOneRef.on('child_added', function (snapshot) {
  //   console.log(snapshot.key, snapshot.val());

  //   // add to array for each type of data
  //   moreThanOne.push(snapshot.val());
  //   // parse arr
  //   // sequelize
  //   // check if unique id exists so no duplicate rows are added?
  //   // add rows to table
  //   console.log(moreThanOne);
  // });
});

module.exports = router;
