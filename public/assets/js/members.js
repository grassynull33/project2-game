$(document).ready(function () {
  // This file just does a GET request to figure out which user is logged in
  // and updates the HTML on the page
  $.get('/api/user_data').then(function (data) {
    $('#member').text(data.username);
    $('#creditAmount').text(data.credits);

    $('#profile-pic').attr('src', data.gravatar);

    if (data.username) {
      $('#login-logout-link').html('<span class="icon fa-sign-out">Logout</span>');
      $('#login-logout-link').attr('href', '/users/signout');
      $('#login-logout-link').attr('data-target', '#');
    } else {
      $('#login-logout-link').html('<span class="icon fa-sign-in">Login / Register</span>');
      $('#login-logout-link').attr('href', 'page-scroll');
      $('#login-logout-link').attr('data-target', '#login-modal');
    }

    $(document).on('click', '.buyBtn', function () {
      $.get('/api/user_data').then(function (data) {
        if (!data.username) {
          $('#cannot-buy').text('You must be logged in to purchase items!');
          $('#cannot-buy').show();
        } else {

        }
      });
    });
  });

  // $.get('/users/signout').then(function () {
  //   $('#login-logout-link').text('Logout');
  // });
});
