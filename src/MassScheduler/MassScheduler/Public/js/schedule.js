(function() {
  var Notifications, Schedule;

  Notifications = function(selector) {
    var $el, createMessage, id,
      _this = this;
    $el = $(selector);
    id = 0;
    $el.on('click', 'a.close', function(e) {
      var $msg;
      e.preventDefault();
      $msg = $(e.currentTarget).parent();
      return $msg.fadeOut('slow', function() {
        return $(this).remove();
      });
    });
    createMessage = function(message, type) {
      var close, msg;
      msg = $("<p>" + message + "</p>");
      close = $("<a>Dismiss</a>");
      close.attr({
        'href': '#dismiss',
        'class': 'close'
      });
      msg.prepend(close);
      msg.prop('id', "msg-" + (++id));
      msg.addClass(type);
      return msg;
    };
    return {
      addMessage: function(message, type) {
        var msg;
        msg = createMessage(message, type);
        msg.hide();
        $el.prepend(msg);
        msg.fadeIn(150);
        return msg;
      }
    };
  };

  Schedule = function(selector) {
    var $el, $events, cancelRsvp, getEventId, refreshAttendees, refreshEventLine, rsvp, togglePreview;
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
      var $ev, $evr, action;
      e.preventDefault();
      $ev = $(this);
      $evr = $ev.parents('.event');
      action = $ev.prop('href');
      return $.ajax({
        type: 'POST',
        url: action,
        success: function(d, s) {
          refreshEventLine($evr, true);
          refreshAttendees($evr.data('eventid'));
          window.HKS.Alerts.addMessage("You have been RSVP'd to this event!", "confirm");
        }
      });
    };
    cancelRsvp = function(e) {
      var $ev, $evr, action;
      e.preventDefault();
      $ev = $(this);
      $evr = $ev.parents('.event');
      action = $ev.prop('href');
      return $.ajax({
        type: 'POST',
        url: action,
        success: function(d, s) {
          refreshEventLine($evr, false);
          refreshAttendees($evr.data('eventid'));
          window.HKS.Alerts.addMessage("Sorry you can't make it, your RSVP has been canceled.", "confirm");
        }
      });
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
    refreshAttendees = function(id) {
      if (typeof baseUrl !== 'undefined') {
        return $.get(baseUrl + ("/" + id), function(d) {
          $("#attendees").html(d);
        });
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
    $events.on('click', 'a.title-link', togglePreview);
  };

  window.HKS = {
    Schedule: new Schedule('.events'),
    Alerts: new Notifications('.notifications')
  };

}).call(this);
