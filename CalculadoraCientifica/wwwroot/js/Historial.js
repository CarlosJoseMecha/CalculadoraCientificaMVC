$(document).ready(function () {
   $('.historial-button').on('click', function () {
      var checkedIds = [];

      $('input[type="checkbox"]:checked').each(function () {
         var idElemento = $(this).attr('id');
         checkedIds.push(idElemento);
         console.log(idElemento);
      });

      $.ajax({
         url: '/Historial/DeleteMultiple',
         type: 'POST',
         data: { ids: checkedIds },
         success: function (result) {
            location.reload();
         }
      });
   });
});


$(document).ready(function () {
   $('#checkAll').on('click', function () {
      var isChecked = $(this).prop('checked');
      $('table input[type="checkbox"]').prop('checked', isChecked);
   });
});


$(document).ready(function () {
   $('.historial-button')
      .prop('disabled', true)
      .removeClass('btn-danger')
      .addClass('btn-secondary')

   $('input[type="checkbox"]').on('click', function () {
      var isChecked = $('input[type="checkbox"]:checked').length > 0;
      $('.historial-button')
         .prop('disabled', !isChecked)
         .toggleClass('btn-danger', isChecked)
         .toggleClass('btn-secondary', !isChecked)
   });
});