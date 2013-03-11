(function() {
  var Schedule;

  Schedule = function(selector) {
    var $el, $events, cancelRsvp, getEventId, refreshEventLine, rsvp, togglePreview;
    $el = $(selector);
    getEventId = function(event) {
      var $ev;
      $ev = $(event);
      return $ev.parents('.event').attr('data-eventid');
    };
    togglePreview = function(e) {
      var $ev, $ev_id;
      e.preventDefault();
      $ev = $(this);
      $ev_id = getEventId($ev);
      $el.find("#e_" + $ev_id).toggleClass('hidden');
    };
    rsvp = function(e) {
      var $ev, $ev_id, $evp;
      e.preventDefault();
      $ev = $(this);
      $evp = $ev.parents('.event');
      $ev_id = $evp.attr('data-eventid');
      $.post($ev.attr('href'));
      return refreshEventLine($evp, true);
    };
    cancelRsvp = function(e) {
      var $ev, $ev_id, $evp;
      e.preventDefault();
      $ev = $(this);
      $evp = $ev.parents('.event');
      $ev_id = $evp.attr('data-eventid');
      $.post($ev.attr('href'));
      return refreshEventLine($evp, false);
    };
    refreshEventLine = function($event, attending) {
      if (attending) {
        $event.find('a.rsvp-button').parent().hide();
        $event.find('a.cancel-rsvp-button').parent().show();
        return $event.addClass('attending');
      } else {
        $event.find('a.cancel-rsvp-button').parent().hide();
        $event.find('a.rsvp-button').parent().show();
        return $event.removeClass('attending');
      }
    };
    $events = $el.find('.event');
    $events.each(function() {
      var $e;
      $e = $(this);
      return refreshEventLine($e, $e.hasClass('attending'));
    });
    $events.on('click', 'a.rsvp-button', rsvp);
    $events.on('click', 'a.cancel-rsvp-button', cancelRsvp);
    $events.on('click', '.title-link a', togglePreview);
  };

  window.Schedule = Schedule;

  window.Schedule('.schedule');

}).call(this);
