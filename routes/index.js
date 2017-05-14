var firebase = require('firebase-admin');
var models = require('../models');
var express = require('express');
var router = express.Router();

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
  ref.on('child_added', function (snapshot) {
    console.log(snapshot.key, snapshot.val());

    // add to array for each type of data
    // parse arr
    // sequelize
    // check if unique id exists so no duplicate rows are added?
    // add rows to table
  });
});

module.exports = router;
