$(document).ready(function () {
  $('.slot').jSlots({
    spinner: '#playBtn',
    winnerNumber: 7
  });

  $('#playBtn').click(function (event) {
    event.preventDefault();
  });
  // $(document).on('click', '.minigame-item', function () {
  //   console.log('lol');
  //   $(this).hide();
  // });
});
