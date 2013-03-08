(function() {
  var Schedule;

  Schedule = function(selector) {
    var $el, togglePreview;
    $el = $(selector);
    togglePreview = function(e) {
      var $ev, $ev_id;
      e.preventDefault();
      $ev = $(this);
      $ev_id = $ev.parents('.event-bar').attr('data-eventid');
      $el.find("#e_" + $ev_id).toggleClass('hidden');
    };
    $el.on('click', '.title-link a', togglePreview);
  };

  window.Schedule = Schedule;

  window.Schedule('.schedule');

}).call(this);
