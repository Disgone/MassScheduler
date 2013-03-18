Notifications = (selector) ->
    $el = $ selector
    id = 0

    $el.on 'click', 'a.close', (e) =>
        e.preventDefault()
        $msg = $(e.currentTarget).parent()
        $msg.fadeOut 'slow', () ->
            $(@).remove()

    createMessage = (message, type) ->
        msg = $ "<p>#{message}</p>"
        close = $ "<a>Dismiss</a>"
        close.attr
            'href': '#dismiss'
            'class': 'close'
        msg.prepend close
        msg.prop 'id', "msg-#{++id}"
        msg.addClass type
        msg

    addMessage: (message, type) =>
        msg = createMessage message, type
        msg.hide()
        $el.prepend msg
        msg.fadeIn(150)
        msg


Schedule = (selector) ->
    $el = $ selector

    getEventId = (event) ->
        $ev = $ event
        $ev.parents('.event').attr('data-eventid')

    togglePreview = (e) ->
        e.preventDefault()
        $ev = $ @
        $ev_id = getEventId $ev

        $el.find("#e_" + $ev_id).toggleClass 'hidden'
        return

    rsvp = (e) ->
        e.preventDefault()
        $ev = $ @
        $evr = $ev.parents '.event'
        action = $ev.prop 'href'

        $.ajax
            type: 'POST'
            url: action
            success: (d, s) ->
                refreshEventLine $evr, true
                refreshAttendees $evr.data 'eventid'
                window.HKS.Alerts.addMessage "You have been RSVP'd to this event!", "confirm"
                return

    cancelRsvp = (e) ->
        e.preventDefault()
        $ev = $ @
        $evr = $ev.parents '.event'
        action = $ev.prop 'href'
        
        $.ajax
            type: 'POST'
            url: action
            success: (d, s) ->
                refreshEventLine $evr, false
                refreshAttendees $evr.data 'eventid'
                window.HKS.Alerts.addMessage "Sorry you can't make it, your RSVP has been canceled.", "confirm"
                return

    refreshEventLine = ($event, attending) ->
        if attending
            $event.find('a.rsvp-button').parent().hide()
            $event.find('a.cancel-rsvp-button').parent().show()
            $event.addClass 'attending'
        else
            $event.find('a.cancel-rsvp-button').parent().hide()
            $event.find('a.rsvp-button').parent().show()
            $event.removeClass 'attending'

    refreshAttendees = (id) ->
        if typeof baseUrl isnt 'undefined'
            $.ajax
                type: 'GET'
                url:  baseUrl + "/#{id}" 
                cache: false
                success: (d) ->
                    $("#attendees").html(d)
                    return


    $events = $el.find '.event'

    $events.each () ->
        $e = $ @
        refreshEventLine $e, $e.hasClass('attending')
            
    $events.on 'click', 'a.rsvp-button', rsvp

    $events.on 'click', 
                'a.cancel-rsvp-button', 
                cancelRsvp

    $events.on 'click', 'a.title-link', togglePreview

    return

window.HKS =
    Schedule: new Schedule('.events')
    Alerts: new Notifications('.notifications')