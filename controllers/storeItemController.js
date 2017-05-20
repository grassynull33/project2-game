var db = require('../models');

exports.checkStoreItems = function (req, res, next) {
  console.log('STORE ITEMS MIDDLEWARE');

  res.locals.storeItems = [
    {
      name: 'Equilibrium',
      description: 'Pre-order now. Year of release 2020 (anticipated).',
      price: 200,
      imageRef: '/assets/images/store-01.png'
    },
    {
      name: 'Equilibrium Deluxe',
      description: 'Pre-order now. Year of release 2021 (anticipated).',
      price: 300,
      imageRef: '/assets/images/store-02.png'
    },
    {
      name: 'Equilibrium Shirt',
      description: 'Only available in XS or 4XL. Must pick up locally.',
      price: 250,
      imageRef: '/assets/images/store-03.png'
    }
  ];

  res.render('index', res.locals);

  // next();
};
