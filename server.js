var express = require('express');
var bodyParser = require('body-parser');
var exphbs = require('express-handlebars');
var path = require('path');
var routes = require('./routes/index.js');
var passport = require('passport');

var app = express();
var PORT = process.env.PORT || 3000;

var db = require('./models');

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.text());
app.use(bodyParser.json({ type: 'application/vnd.api+json' }));

app.use(passport.initialize());
app.use(passport.session());

passport.use(db.User.createStrategy());

passport.serializeUser(db.User.serializeUser());
passport.deserializeUser(db.User.deserializeUser());

app.engine('handlebars', exphbs({ defaultLayout: 'main' }));
app.set('view engine', 'handlebars');

app.use(express.static(path.join(__dirname, 'public')));

app.use('/', routes);

db.sequelize.sync({ force: true }).then(function () {
  app.listen(PORT, function () {
    console.log('Listening on PORT ' + PORT);
  });
});
