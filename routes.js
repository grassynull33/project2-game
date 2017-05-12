module.exports = function(app){

		// Our model controllers (rather than routes)
		var index = require('./routes/index');
		var userController = require('./controllers/userController');

		app.use('/', index);
		app.use('/users', userController);

    //other routes..
}