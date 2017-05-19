$(document).ready(function () {
  // This file just does a GET request to figure out which user is logged in
  // and updates the HTML on the page
  $.get('/api/user_data').then(function (data) {
    $('#member').text(data.username);
    $('#email').text(data.email);

    $('#profile-pic').attr('src', data.gravatar);

    if (data.username) {
      $('#login-logout-link').html('<span class="icon fa-sign-in">Logout</span>');
      $('#login-logout-link').attr('href', '/users/signout');
      $('#login-logout-link').attr('data-target', '#');
    } else {
      $('#login-logout-link').html('<span class="icon fa-sign-in">Login</span>');
      $('#login-logout-link').attr('href', 'page-scroll');
      $('#login-logout-link').attr('data-target', '#login-modal');
    }
  });

  // $.get('/users/signout').then(function () {
  //   $('#login-logout-link').text('Logout');
  // });
});
