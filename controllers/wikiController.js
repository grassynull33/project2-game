var db = require('../models');

exports.checkWiki = function (req, res) {
  var data = {
    wiki: []
  };

  db.Wiki.findAll().then(function (results) {
    console.log('wiki controller passing thru');

    for (var i = 0; i < results.length; i++) {
      data.wiki.push(results[i].dataValues);
      // console.log(results[i].dataValues);
    }

    // console.log(data.wiki);

    res.locals.wiki = data.wiki;
    res.locals.wikiCount = data.wiki.length;

    res.render('index', res.locals);
    // next();
  }).catch(function (error) {
    console.log(error);

    console.log('wiki controller error');

    // next();
  });
};
