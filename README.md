# DNN Dev Tools
DNN Dev Tools - The Swiss army knife for DNN developers and host admins

DNN Dev Tools is developed by weweave (https://weweave.net/) and licensed under the The MIT License (MIT).

## Features
Screenshots and videos can be found in the wiki (https://github.com/weweave/DNNDevTools/wiki/Screenshots).

### Log message tracing
DNN Dev Tools watches for log messages that are created in the DNN environment and offers a web interface to analyze them. 

### DNN event tracing
DNN Dev Tools watches for events that are created in the DNN environment and offers a web interface to analyze them. 

### E-mail catch
DNN Dev Tools catches e-mails that are created in the DNN environment instead of sending them to the actual recipient and offers a web interface to analyze them. This feature is especially useful in non-production environments where you want to make sure no e-mails are sent to real users. This feature is not enabled by default, it blocks e-mails from being sent to the users which is in most cases not desired in production environments. 
**Please note: Enabling this feature blocks e-mails from bwing sent to the actual recipient**. 

### Real-time notification
DNN Dev Tools uses real-time notification: If “something happens” (like a DNN event happened or a mail was caught), DNN Dev Tools shows an overlay on the website in real-time.

### Seamless UI integration
DNN Dev Tools takes care to not break the existing user interface by prefixing all CSS selectors and not using any 3rd party JavaScript or CSS libraries when adding elements to user interface like the DnnDevTool icon in the bottom right corner. The main (overlay) screen of DNN Dev Tools that makes use of libraries like AngularJS runs in an iframe to not influence the DNN website.

### Misc
* Security: By default, DNN Dev Tools functionalities are only enabled/visible for host admins
* Internationalization: Localized in en-US and de-DE
* Quality: Passed EVS (see http://evs.dotnetnuke.com/)

## Requirements

* DNN 08.00.00 CTP 6 (tested up to DNN 08.00.00 Beta 1)
* A-Grade browser (Firefox, Chrome, IE 11, Edge)

## Installation / Setup
**Please note: Whenever possible use DNN Dev Tools only in development, staging or QA-environments but not on production website.** Even though DNN Dev Tools only adds a minimal performance overhead, the purpose of the module is to help developers (and host admins) to analyze problems in a DNN environment and that should be done in a separate non-production environment.

DNN Dev Tools is installed just as any other DNN module:
 1. Download the most recent release DNN Dev Tools from here https://github.com/weweave/DnnDevTools/releases
 1. Log in as host admin and install the module on the page System > Extensions
 
**Please note:** After installing DNN Dev Tools, the module is (automatically) enabled and the DNN Dev Tools icon should be visible in the bottom right corner of every page. For security reasons DNN Dev Tools is only enabled for host admin users, for all other users nothing changes. To configure DNN Dev Tools settings, log in as host admin and browser to the page System > DNN Dev Tools.
