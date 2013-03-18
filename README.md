Mass Scheduler
=============

Customized [NerdDinner][nd] example written using MVC 4 and Windows Authentication for a week long, intra-company 
workshop.

## Requirements
1. Use windows auth to power access and provide the needed information for RSVP's.
2. Employees can RSVP and cancel their RSVP for any event that had not ended.
3. Create speaker bios so employees could learn more about the presenters.

We removed the social media elements and location information (at least in the geographic sense).  We also changed
the flow so only a specified group could perform "admin" tasks such as creating speakers or meetings.  Our plan was to
have a specific group maintain the event data and allow all employees to pick and choose which events they wanted to
attend.

## Features
* Uses windows authentication
* We exchanged "hosts" for speakers. Each speaker will have a bio with a little information about who they are.
* Upload handler for bio images.  We added a *simple* upload handler that will automatically resize images to a max height/width.

Layout and UI was inspired by the terrific schedule/speaker lists on both [UX London][uxlondon] and [SXSW][sxsw] event sites.

[uxlondon]: http://2013.uxlondon.com/
[sxsw]: http://schedule.sxsw.com/
[nd]: http://www.nerddinner.com/
