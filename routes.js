module.exports = function (app) {
    // Our model controllers (rather than routes)
  var index = require('./routes/index');
  var userController = require('./controllers/userController');
  var itemController = require('./controllers/itemController');

  app.use('/', index);
  app.use('/users', userController);
  app.use('/firebase', itemController);

    // other routes..
};
