$(document).ready(function () {
  $('.slot').jSlots({
    spinner: '#playBtn',
    winnerNumber: 7,
    onStart: function () {
      $('.slot').removeClass('winner');
    },
    onWin: function (winCount, winners) {
      $.each(winners, function () {
        this.addClass('winner');
      });

      if (winCount === 1) {

      } else if (winCount === 2) {

      } else if (winCount === 3) {
        // add game logic if you win 3 7's
      }
    }
  });

  $('#playBtn').click(function (event) {
    event.preventDefault();
  });
  // $(document).on('click', '.minigame-item', function () {
  //   console.log('lol');
  //   $(this).hide();
  // });
});
