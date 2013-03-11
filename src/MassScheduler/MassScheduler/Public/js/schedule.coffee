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
        $evp = $ev.parents '.event'
        $ev_id = $evp.attr 'data-eventid'

        $.post $ev.attr('href')

        refreshEventLine $evp, true

    cancelRsvp = (e) ->
        e.preventDefault()
        $ev = $ @
        $evp = $ev.parents '.event'
        $ev_id = $evp.attr 'data-eventid'

        $.post $ev.attr('href')

        refreshEventLine $evp, false

    refreshEventLine = ($event, attending) ->
        if attending
            $event.find('a.rsvp-button').parent().hide()
            $event.find('a.cancel-rsvp-button').parent().show()
            $event.addClass 'attending'
        else
            $event.find('a.cancel-rsvp-button').parent().hide()
            $event.find('a.rsvp-button').parent().show()
            $event.removeClass 'attending'


    $events = $el.find '.event'

    $events.each () ->
        $e = $ @
        refreshEventLine $e, $e.hasClass('attending')
            
    $events.on 'click', 'a.rsvp-button', rsvp

    $events.on 'click', 
                'a.cancel-rsvp-button', 
                cancelRsvp

    $events.on 'click', '.title-link a', togglePreview

    return
    
window.Schedule = Schedule
window.Schedule('.schedule')