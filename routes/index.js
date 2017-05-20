var firebase = require('firebase-admin');
var express = require('express');
var router = express.Router();
var minigameController = require('../controllers/minigameController');
var achievementController = require('../controllers/achievementController');
var wikiController = require('../controllers/wikiController');
var userController = require('../controllers/userController');
var itemController = require('../controllers/itemController');
var storeItemController = require('../controllers/storeItemController');
var guestbookController = require('../controllers/guestbookController');
var gravatar = require('gravatar');

var passport = require('../config/passport');
var isAuthenticated = require('../config/middleware/isAuthenticated');

var db = require('../models');

var serviceAccount = {
  'type': 'service_account',
  'project_id': process.env.PROJECT_ID,
  'private_key_id': process.env.PRIVATE_KEY_ID,
  'private_key': process.env.PRIVATE_KEY,
  'client_email': process.env.CLIENT_EMAIL,
  'client_id': process.env.CLIENT_ID,
  'auth_uri': 'https://accounts.google.com/o/oauth2/auth',
  'token_uri': 'https://accounts.google.com/o/oauth2/token',
  'auth_provider_x509_cert_url': 'https://www.googleapis.com/oauth2/v1/certs',
  'client_x509_cert_url': process.env.CLIENT_CERT_URL
};

firebase.initializeApp({
  credential: firebase.credential.cert(serviceAccount),
  databaseURL: process.env.DATABASE_URL
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
  itemController.checkItemsList,
  minigameController.checkMinigame,
  wikiController.checkWiki,
  achievementController.checkAchievements,
  storeItemController.checkStoreItems
  // ,
  // guestbookController.checkGuestBook
);

router.get('/users/signout', function (req, res) {
  req.logout();
  res.redirect('/');
});

router.get('/users/signup', function (req, res) {
  res.render('registration', {
    layout: 'main-registration'
  });
});

// register a user
router.post('/users/signup', function (req, res) {
  db.User.findAll({
    where: {username: req.body.username}
  }).then(function (users) {
    if (users.length > 0) {
      res.json({
        duplicateUser: true
      });
    // At some point, make sure that only one user can be associated with an email.
    } else {
      db.User.create({
        username: req.body.username,
        email: req.body.email,
        password: req.body.password,
        credits: 200
      }).then(function () {
        // res.send({redirect: '/'});
        res.redirect(307, '/users/login');
        // res.redirect('/');
      }).catch(function (err) {
        res.json(err);
      });
    }
  });
});

router.post('/api/feedback/', function (req, res) {
  console.log(req.body);
  db.Guestbook.create({
    name: req.body.name,
    feedback: req.body.feedback,
    username: req.body.username
  }).then(function () {
    res.redirect('/');
  }).catch(function (err) {
    res.json(err);
  });
});

router.get('api/feedback/', function (req, res) {
  res.json({
    name: req.body.name,
    feedback: req.body.feedback,
    username: req.body.username
  });
});

// login
router.post('/users/login', passport.authenticate('local'), function (req, res) {
    // Since we're doing a POST with javascript, we can't actually redirect that post into a GET request
    // So we're sending the user back the route to the members page because the redirect will happen on the front end
    // They won't get this or even be able to access this page if they aren't authed
  // res.json('/');
  res.send({redirect: '/'});
  // res.redirect('/');
});

// Route for getting some data about our user to be used client side
router.get('/api/user_data', function (req, res) {
  if (!req.user) {
    // The user is not logged in, send back an empty object
    res.json({});
  } else {
    // Otherwise send back the user's email and id
    // Sending back a password, even a hashed password, isn't a good idea
    var url = gravatar.url(req.user.email, {s: '200', r: 'r', d: 'mm'});

    res.json({
      email: req.user.email,
      id: req.user.id,
      username: req.user.username,
      gravatar: url,
      credits: req.user.credits
    });
  }
});

module.exports = router;
