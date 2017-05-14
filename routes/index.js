
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

  // detects items already in firebase
  ref.on('child_added', function (snapshot) {
    // console.log(snapshot.key, snapshot.val());
    // counter++;
    // console.log(snapshot.key);
    // console.log(snapshot.val());
    // add to array for each type of data
    if (snapshot.key === 'name') {
      for (var key in snapshot.val()) {
        name.push(snapshot.val()[key]);
        uniqueID.push(key);
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
        console.log((i + 1) + '. Item Name: ' + name[i] + ' | Description: ' + description[i] + ' | slotID: ' + slotID[i] + ' | moreThanOne: ' + moreThanOne[i] + ' | uID: ' + uniqueID[i]);

        db.Item.create({
          name: name[i],
          description: description[i],
          slotID: slotID[i],
          moreThanOne: moreThanOne[i],
          uniqueID: uniqueID[i]
        });
          // .then(function () {
          //   res.send({redirect: '/'});
          // }).catch(function (err) {
          //   res.json(err);
          // });

        // db.Item.findAll({
        //   where: {uniqueID: uniqueID[i]}
        // }).then(function (items) {
        //   if (items.length > 0) {
        //     console.log('duplicate found');
        //   } else {

        //   }
        // });
      }
    }
  });

  // update w newly added items
  ref.on('child_changed', function (snapshot) {
    console.log(snapshot.key);
    console.log(snapshot.val());

    // TODO need to figure out how to get last item in returned child obj
    // if (snapshot.key === 'name') {
    //   for (var key in snapshot.val()) {
    //     name.push(snapshot.val()[key]);
    //     uniqueID.push(key);
    //   }
    // } else if (snapshot.key === 'description') {
    //   for (var key in snapshot.val()) {
    //     description.push(snapshot.val()[key]);
    //   }
    // } else if (snapshot.key === 'slotID') {
    //   for (var key in snapshot.val()) {
    //     slotID.push(snapshot.val()[key]);
    //   }
    // } else if (snapshot.key === 'More than One in Inventory') {
    //   for (var key in snapshot.val()) {
    //     moreThanOne.push(snapshot.val()[key]);
    //   }
    // }
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
