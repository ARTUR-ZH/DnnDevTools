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
(function (window) {
    var enableCheckbox = document.getElementById('dnnDevTools-enableCheckbox'),
        enableMailCatchCheckbox = document.getElementById('dnnDevTools-enableMailCatchCheckbox'),
        enableDnnEventTraceCheckbox = document.getElementById('dnnDevTools-enableDnnEventTraceCheckbox'),
        enableLoggingCheckbox = document.getElementById('dnnDevTools-enableLoggingCheckbox'),
        logLevelSelect = document.getElementById('dnnDevTools-logLevelSelect'),
        sendMailButton = document.getElementById('dnnDevTools-sendMailButton');

    // set current status
    enableCheckbox.checked = window.weweave.dnnDevTools.enable;
    enableMailCatchCheckbox.checked = window.weweave.dnnDevTools.enableMailCatch;
    enableDnnEventTraceCheckbox.checked = window.weweave.dnnDevTools.enableDnnEventTrace;
    enableLoggingCheckbox.checked = window.weweave.dnnDevTools.logMessageTraceLevel !== 'OFF';
    logLevelSelect.value = window.weweave.dnnDevTools.logMessageTraceLevel;

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
    }, false);

    enableDnnEventTraceCheckbox.addEventListener('click', function () {
        setEventTraceStatus(enableDnnEventTraceCheckbox.checked);
    }, false);

    enableLoggingCheckbox.addEventListener('click', function () {
        setLoggingStatus(enableLoggingCheckbox.checked);
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
}(window));