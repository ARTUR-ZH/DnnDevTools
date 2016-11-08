/* ------------------------- */
/* PersonaBar initialization */
/* ------------------------- */

'use strict';
define(['jquery',
    '../scripts/config'
],
    function ($, cf) {
        var utility;
        var config = cf.init();
        
        return {
            init: function (wrapper, util, params, callback) {
                utility = util;


                if (typeof callback === "function") {
                    callback();
                }

                initDnnDevTools(config);
            },

            initMobile: function (wrapper, util, params, callback) {
                this.init(wrapper, util, params, callback);
            },

            load: function (params, callback) {
                var fb = window.dnn.formBuilder;
                if (fb && fb.load) {
                    fb.load();
                }
                var optin = window.dnn.optIn;
                if (optin && optin.load) {
                    var mode = getOptInMode();
                    optin.load(mode);
                }
            },

            loadMobile: function (params, callback) {
                this.load(params, callback);
            }
        };
    });


/* ------------------------- */
/* DNN Dev Tools             */
/* ------------------------- */

function initDnnDevTools(config) {

    window.weweave = window.weweave || {};
    window.weweave.dnnDevTools = window.weweave.dnnDevTools || {};

    console.log("Config:", config);

    // Set dnnDevTools' config
    window.weweave.dnnDevTools.baseUrl = config.siteRoot + '/DesktopModules/DnnDevTools/';

    /**
     * General functionality
     */
    (function(window) {

        window.weweave.dnnDevTools.ajax = function(method, url, success, error) {
            var req = new XMLHttpRequest();

            req.open(method, url);

            req.setRequestHeader('requestVerificationToken', config.antiForgeryToken);

            req.onload = function() {
                var response = (req.responseText && req.responseText != '') ? JSON.parse(req.responseText) : '';

                if (req.status == 200) {
                    success(response);
                } else {
                    error(Error(req.statusText));
                }
            };

            req.onerror = function() {
                error(Error("Please check your internet connection."));
            };

            req.send();
        }
    }(window));

    /**
     * Functionality for HostSettings.aspx
     */
    (function(document, window) {

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
            sendMailButton.addEventListener('click',
                function() {
                    sendMail();
                },
                false);
        } else {
            sendMailButton.setAttribute('disabled', 'disabled');
        }

        enableCheckbox.addEventListener('change',
            function(event) {
                setStatus(enableCheckbox.checked);
            },
            false);

        enableMailCatchCheckbox.addEventListener('change',
            function(event) {
                setMailCatchStatus(enableMailCatchCheckbox.checked);
                setElementVisibility(mailSettingsWrapper, enableMailCatchCheckbox.checked);
            },
            false);

        enableDnnEventTraceCheckbox.addEventListener('click',
            function() {
                setEventTraceStatus(enableDnnEventTraceCheckbox.checked);
            },
            false);

        enableLoggingCheckbox.addEventListener('click',
            function() {
                setLoggingStatus(enableLoggingCheckbox.checked);
                setElementVisibility(logMessagesSettingsWrapper, enableLoggingCheckbox.checked);
                logLevelSelect.value = 'ALL';
            },
            false);

        logLevelSelect.addEventListener('change',
            function() {
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

}