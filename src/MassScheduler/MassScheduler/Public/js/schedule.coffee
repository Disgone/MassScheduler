Schedule = (selector) ->
    $el = $ selector

    togglePreview = (e) ->
        e.preventDefault()
        $ev = $ @
        $ev_id = $ev.parents('.event-bar').attr('data-eventid')

        $el.find("#e_" + $ev_id).toggleClass 'hidden'
        return


    $el.on 'click', '.title-link a', togglePreview

    return
    
window.Schedule = Schedule
window.Schedule('.schedule')