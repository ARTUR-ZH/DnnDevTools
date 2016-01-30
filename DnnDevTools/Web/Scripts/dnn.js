/**
 * General functionality
 */
(function (window) {
    window.weweave.dnnDevTools.ajax = function (method, url, success, error) {
        var req = new XMLHttpRequest();

        req.open(method, url);

        req.setRequestHeader('requestVerificationToken', $.ServicesFramework().getAntiForgeryValue());

        req.onload = function () {
            var response = (req.responseText && req.responseText != '') ? JSON.parse(req.responseText) : '';

            if (req.status == 200) {
                success(response);
            }
            else {
                error(Error(req.statusText));
            }
        };

        req.onerror = function () {
            error(Error("Please check your internet connection."));
        };

        req.send();
    }
}(window));

/**
 * Functionality for HostSettings.aspx
 */
(function (document, window) {

    // Test if Host setting elements exist
    if (!document.getElementById('dnnDevTools-hostSettings')) return;
    
    var enableCheckbox = document.getElementById('dnnDevTools-enableCheckbox'),
        enableMailCatchCheckbox = document.getElementById('dnnDevTools-enableMailCatchCheckbox'),
        enableDnnEventTraceCheckbox = document.getElementById('dnnDevTools-enableDnnEventTraceCheckbox'),
        enableLoggingCheckbox = document.getElementById('dnnDevTools-enableLoggingCheckbox'),
        logLevelSelect = document.getElementById('dnnDevTools-logLevelSelect'),
        sendMailButton = document.getElementById('dnnDevTools-sendMailButton'),
        devToolsSettingsWrapper = document.getElementById('dnnDevTools-devToolsSettingsWrapper'),
        mailSettingsWrapper = document.getElementById('dnnDevTools-mailSettingsWrapper'),
        logMessagesSettingsWrapper = document.getElementById('dnnDevTools-logMessagesSettingsWrapper');

    // set current status
    enableCheckbox.checked = window.weweave.dnnDevTools.enable;
    enableMailCatchCheckbox.checked = window.weweave.dnnDevTools.enableMailCatch;
    enableDnnEventTraceCheckbox.checked = window.weweave.dnnDevTools.enableDnnEventTrace;
    enableLoggingCheckbox.checked = window.weweave.dnnDevTools.logMessageTraceLevel !== 'OFF';
    logLevelSelect.value = window.weweave.dnnDevTools.logMessageTraceLevel;

    // only show settings when dnn dev tools are activated
    setElementVisibility(devToolsSettingsWrapper, window.weweave.dnnDevTools.enable);

    // only show mail settings when mail catch is enabled
    setElementVisibility(mailSettingsWrapper, window.weweave.dnnDevTools.enableMailCatch);

    // only show log message settings when log message catch is enabled
    setElementVisibility(logMessagesSettingsWrapper, enableLoggingCheckbox.checked);

    // enable send test mail functionality if smtp is configured properly
    // otherwise disable send test mail button
    if (window.weweave.dnnDevTools.hostSmtpConfigured) {
        sendMailButton.addEventListener('click', function () {
            sendMail();
        }, false);
    } else {
        sendMailButton.setAttribute('disabled', 'disabled');
    }

    enableCheckbox.addEventListener('change', function (event) {
        setStatus(enableCheckbox.checked);
    }, false);

    enableMailCatchCheckbox.addEventListener('change', function (event) {
        setMailCatchStatus(enableMailCatchCheckbox.checked);
        setElementVisibility(mailSettingsWrapper, enableMailCatchCheckbox.checked);
    }, false);

    enableDnnEventTraceCheckbox.addEventListener('click', function () {
        setEventTraceStatus(enableDnnEventTraceCheckbox.checked);
    }, false);

    enableLoggingCheckbox.addEventListener('click', function () {
        setLoggingStatus(enableLoggingCheckbox.checked);
        setElementVisibility(logMessagesSettingsWrapper, enableLoggingCheckbox.checked);
        logLevelSelect.value = 'ALL';
    }, false);

    logLevelSelect.addEventListener('change', function () {
        setLogLevel(logLevelSelect.value);
    });

    function setStatus(isEnabled) {
        var url = window.weweave.dnnDevTools.baseUrl + 'api/config/enable?status=' + isEnabled;

        window.weweave.dnnDevTools.ajax('PUT', url, success, error);

        function success(response) {
            window.location.reload(false);
        }

        function error(error) {
            // TODO handle error
        }
    }

    function setMailCatchStatus(isEnabled) {
        var url = window.weweave.dnnDevTools.baseUrl + 'api/config/enableMailCatch?status=' + isEnabled;

        window.weweave.dnnDevTools.ajax('PUT', url, success, error);

        function success(response) {
            // TODO handle success
        }

        function error(error) {
            // TODO handle error
        }
    }

    function setEventTraceStatus(isEnabled) {
        var url = window.weweave.dnnDevTools.baseUrl + 'api/config/enableDnnEventTrace?status=' + isEnabled;

        window.weweave.dnnDevTools.ajax('PUT', url, success, error);

        function success(response) {
            // TODO handle success
        }

        function error(error) {
            // TODO handle error
        }
    }

    function setLoggingStatus(isEnabled) {
        if (isEnabled) {
            setLogLevel(logLevelSelect.value);
        } else {
            setLogLevel('OFF');
        }
    }

    function sendMail() {
        var url = window.weweave.dnnDevTools.baseUrl + 'api/config/sendTestMail';

        window.weweave.dnnDevTools.ajax('POST', url, success, error);

        function success(response) {
            // TODO handle success
        }

        function error(error) {
            // TODO handle error
        }
    }

    function setLogLevel(level) {
        var url = window.weweave.dnnDevTools.baseUrl + 'api/config/setLogMessageTraceLevel?level=' + level;

        window.weweave.dnnDevTools.ajax('PUT', url, success, error);

        function success(response) {
            // TODO handle success
        }

        function error(error) {
            // TODO handle error
        }
    }

    function setElementVisibility(element, isVisible) {
        if (isVisible) {
            element.classList.remove('dnnDevTools-hidden');
        } else {
            element.classList.add('dnnDevTools-hidden');
        }
    }
}(document, window));

/**
 * Functionality for Toolbar
 */
(function (document, window, settings) {
    var hub = $.connection.dnnDevToolsNotificationHub,
        currentNoteData = {},
        noteTimeoutId,
        noteElement = document.getElementById('dnnDevTools-note'),
        overviewButton = document.getElementById('dnnDevTools-overviewButton'),
        overlay = document.getElementById('dnnDevTools-overlay'),
        overlayPanel = document.getElementById('dnnDevTools-overlayPanel'),
        overviewIframe = document.getElementById('dnnDevTools-overviewIframe'),
        settingsButton = document.getElementById('dnnDevTools-settingsButton');

    // merge settings with default settings
    settings.noteVisibleTime = settings.noteVisibleTime || 3000;

    // initialization
    initNote();
    initOverlay();
    initConnection();

    // openOverlay({
    //     Type: 'LogMessage',
    //     Id: '52-62-58-76-79-55-F9-29-48-80-95-90-88-F3-6C-89'
    // });

    function initNote() {
        noteElement.addEventListener('click', function () {
            hideNote();
            openOverlay(currentNoteData);
        }, false);
    }

    function initOverlay() {
        // toggle overlay visibility on overview button click
        overviewButton.addEventListener('click', function () {
            toggleOverlay();
        }, false);

        // open settings page on settings button click
        settingsButton.addEventListener('click', function () {
            window.location.href = window.weweave.dnnDevTools.hostSettingsUrl;
        }, false);

        // close overview window when clicking outside the window panel
        overlay.addEventListener('click', function () {
            closeOverlay();
        });

        // prevent closing the overview window when clicking on the window panel
        overlayPanel.addEventListener('click', function (event) {
            event.stopPropagation();
        });
    }

    function initConnection() {
        // listen to signalR events
        hub.client.OnEvent = function (data) {
            currentNoteData = data;

            // remove icon classes
            noteElement.classList.remove('dnnDevTools-envelopeClosedIcon', 'dnnDevTools-audioIcon', 'dnnDevTools-listIcon');

            switch (data.Type) {
                case 'Mail':
                    noteElement.classList.add('dnnDevTools-envelopeClosedIcon');
                    noteElement.textContent = data.Subject;
                    break;
                case 'DnnEvent':
                    noteElement.classList.add('dnnDevTools-audioIcon');
                    noteElement.textContent = data.Message;
                    break;
                case 'LogMessage':
                    noteElement.classList.add('dnnDevTools-listIcon');
                    noteElement.textContent = data.Message;
                    break;
            }

            showNote();
        };

        // open connection
        $.connection.hub.start();
    }

    function toggleOverlay() {
        if (overlay.classList.contains('dnnDevTools-hidden')) {
            openOverlay();
        } else {
            closeOverlay();
        }
    }

    /**
     * opens the mail overlay
     * @param {string} id The mail with this id will be initially highlighted in overlay
     */
    function openOverlay(noteData) {
        var route = (noteData !== undefined) ? ('#/' + noteData.Type.toLowerCase() + 'detail/' + noteData.Id) : '';

        overviewIframe.src = window.weweave.dnnDevTools.baseUrl + 'Overlay.aspx' + route;
        overlay.classList.remove('dnnDevTools-hidden');

        overviewButton.classList.add('dnnDevTools-active');
    }

    function closeOverlay() {
        overviewIframe.src = '';
        overlay.classList.add('dnnDevTools-hidden');

        overviewButton.classList.remove('dnnDevTools-active');
    }

    function showNote() {
        // TODO check internet explorer support (http://caniuse.com/#search=classList)
        noteElement.classList.remove('dnnDevTools-hidden');

        // reset timeout if another mail comes in while the last mail is still visible
        if (noteTimeoutId) {
            clearTimeout(noteTimeoutId);
            noteTimeoutId = null;
        }

        // hide last mail after some short delay
        noteTimeoutId = window.setTimeout(hideNote, settings.noteVisibleTime);
    }

    function hideNote() {
        // TODO check internet explorer support (http://caniuse.com/#search=classList)
        noteElement.classList.add('dnnDevTools-hidden');
    }
}(document, window, {}));