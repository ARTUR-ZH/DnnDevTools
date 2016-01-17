<%@ Page Language="C#" %>
<html>
<head>
    <title>DNN Dev Tools</title>

    <link rel="stylesheet" type="text/css" href="Styles/dnn.css">

    <style>
        /* LIST */
        .dnnDevTools-listEmpty {
            text-align: center;
        }

        /* DETAIL */
        .dnnDevTools-detail {
            padding: 20px;
        }
    </style>
</head>
<body class="dnnDevTools-overview dnnDevTools-bgColorWhite">
    
    <% Response.Write(System.Web.Helpers.AntiForgery.GetHtml()); %>

    <div ng-app="app">
        <div ui-view role="main"></div>

        <script type="text/ng-template" id="dnnDevTools-overview.html">
            <div class="dnnDevTools-stream">
                <div class="dnnDevTools-filterList dnnDevTools-is-sticky dnnDevTools-bgColorDnnBlue">
                    <a ui-sref="overview({filter: null})" ng-class="{'dnnDevTools-active': !overview.filter}" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite dnnDevTools-removeIcon">Show all</a>
                    <a ui-sref="overview({filter: 'Mail'})" ng-class="{'dnnDevTools-active': overview.filter === 'Mail'}" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite"><span class="dnnDevTools-envelopeClosedIcon dnnDevTools-icon16x16"></span>Mails</a>
                    <a ui-sref="overview({filter: 'DnnEvent'})" ng-class="{'dnnDevTools-active': overview.filter === 'DnnEvent'}" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite"><span class="dnnDevTools-audioIcon dnnDevTools-icon16x16"></span>Events</a>
                    <a ui-sref="overview({filter: 'LogMessage'})" ng-class="{'dnnDevTools-active': overview.filter === 'LogMessage'}" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite"><span class="dnnDevTools-listIcon dnnDevTools-icon16x16"></span>Logs</a>
                </div>

                <div class="dnnDevTools-streamWrapper">
                    <div ng-if="!overview.stream" class="dnnDevTools-spinner"></div>
                    <p ng-if="overview.stream.length === 0" class="dnnDevTools-listEmpty dnnDevTools-copy">Your inbox is currently empty.</p>
                    <ul class="dnnDevTools-streamList">
                        <li ng-repeat="item in overview.stream | orderBy:'-TimeStamp'" ng-click="overview.showDetail(item)" class="dnnDevTools-streamItem">
                            <div class="dnnDevTools-streamItemTimestamp dnnDevTools-streamItemCell">
                                <span ng-class="{'dnnDevTools-envelopeClosedIcon-111111': item.Type === 'Mail', 'dnnDevTools-audioIcon-111111': item.Type === 'DnnEvent', 'dnnDevTools-listIcon-111111': item.Type === 'LogMessage'}" class="dnnDevTools-streamItemIcon dnnDevTools-icon16x16"></span>
                                <p class="dnnDevTools-copy">{{item.TimeStamp | date:'dd.MM.yyyy HH:mm'}}</p>
                            </div>

                            <p ng-if="item.Type === 'DnnEvent'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemEventLogType">{{item.LogType || '&nbsp;'}}</p>
                            <p ng-if="item.Type === 'DnnEvent'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemEventMessage">{{item.Message || '&nbsp;'}}</p>
                            <p ng-if="item.Type === 'DnnEvent'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemEventUsername">{{item.Username || '&nbsp;'}}</p>
                            <p ng-if="item.Type === 'DnnEvent'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemEventPortal">{{item.Portal || '&nbsp;'}}</p>

                            <div ng-if="item.Type === 'LogMessage'" class="dnnDevTools-streamItemCell dnnDevTools-streamItemLogLevel"><p class="dnnDevTools-streamLabel dnnDevTools-copy" ng-class="{'dnnDevTools-bgColorRed': item.Level === 'ERROR', 'dnnDevTools-bgColorOrange': item.Level === 'WARN'}">{{item.Level}}</p></div>
                            <p ng-if="item.Type === 'LogMessage'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemLogMessage">{{item.Message}}</p>
                            <p ng-if="item.Type === 'LogMessage'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemLogLogger">{{item.Logger}}</p>
                            <p ng-if="item.Type === 'LogMessage'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemLogClassNameAndMethodName">{{item.ClassName}}.{{item.MethodName}}</p>

                            <p ng-if="item.Type === 'Mail'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemMailSender">{{item.Sender}}</p>
                            <p ng-if="item.Type === 'Mail'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemMailSubject">{{item.Subject}}</p>
                            <p ng-if="item.Type === 'Mail'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemMailTo">{{item.To}}</p>
                            <div ng-if="item.Type === 'Mail'" class="dnnDevTools-streamItemCell dnnDevTools-streamItemMailActions">
                                <button ng-click="overview.removeMail($event, item)" type="button" class="dnnDevTools-iconButton">
                                    <span class="dnnDevTools-trashIcon dnnDevTools-icon16x16"></span>
                                </button>
                                <button ng-click="overview.downloadMail($event, item.Id)" type="button" class="dnnDevTools-iconButton">
                                    <span class="dnnDevTools-downloadIcon dnnDevTools-icon16x16"></span>
                                </button>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </script>

        <script type="text/ng-template" id="dnnDevTools-mail-detail.html">
            <div class="dnnDevTools-filterList dnnDevTools-bgColorDnnBlue">
                <a ui-sref="overview" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite dnnDevTools-backButton">back to overview</a>
            </div>

            <div class="dnnDevTools-panel dnnDevTools-bgColorGrey">
                <div class="dnnDevTools-marginBottom2">
                    <p class="dnnDevTools-copy dnnDevTools-colorGrey3"><span class="dnnDevTools-copyBold dnnDevTools-colorBlack">from</span> {{mailDetail.mail.Sender}} <span class="dnnDevTools-copyBold dnnDevTools-colorBlack">to</span> {{mailDetail.mail.To}}</p>
                </div>

                <p class="dnnDevTools-copy">{{mailDetail.mail.TimeStamp | date:'dd.MM.yyyy HH:mm'}}</p>
                <h1 class="dnnDevTools-headline">{{mailDetail.mail.Subject}}</h1>
            </div>

            <div class="dnnDevTools-panel">
                <div ng-if="mailDetail.mail.BodyIsHtml" ng-bind-html="mailDetail.mail.Body" class="dnnDevTools-copy"></div>
                <pre ng-if="!mailDetail.mail.BodyIsHtml" ng-bind-html="mailDetail.mail.Body" class="dnnDevTools-pre"></pre>
                <p ng-if="mailDetail.mail && mailDetail.mail.Body === ''" class="dnnDevTools-copy">Body is empty.</p>
            </div>
        </script>

        <script type="text/ng-template" id="dnnDevTools-event-detail.html">
            <div class="dnnDevTools-filterList dnnDevTools-bgColorDnnBlue">
                <a ui-sref="overview" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite dnnDevTools-backButton">back to overview</a>
            </div>
            <div class="dnnDevTools-panel">
                <table>
                    <tbody>
                        <tr>
                            <td><p class="dnnDevTools-copy">Id</p></td>
                            <td><p class="dnnDevTools-copy">{{eventDetail.dnnEvent.Id}}</p></td>
                        </tr>
                        <tr>
                            <td><p class="dnnDevTools-copy">LogType</p></td>
                            <td><p class="dnnDevTools-copy">{{eventDetail.dnnEvent.LogType}}</p></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </script>

        <script type="text/ng-template" id="dnnDevTools-log-detail.html">
            <div class="dnnDevTools-filterList dnnDevTools-bgColorDnnBlue">
                <a ui-sref="overview" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite dnnDevTools-backButton">back to overview</a>
            </div>
            <div class="dnnDevTools-panel">
                <table>
                    <tbody>
                        <tr>
                            <td><p class="dnnDevTools-copy">TimeStamp</p></td>
                            <td><p class="dnnDevTools-copy">{{logDetail.logMessage.TimeStamp}}</p></td>
                        </tr>
                        <tr>
                            <td><p class="dnnDevTools-copy">Level</p></td>
                            <td><p class="dnnDevTools-copy">{{logDetail.logMessage.Level}}</p></td>
                        </tr>
                        <tr>
                            <td><p class="dnnDevTools-copy">Type</p></td>
                            <td><p class="dnnDevTools-copy">{{logDetail.logMessage.Type}}</p></td>
                        </tr>
                        <tr>
                            <td><p class="dnnDevTools-copy">Id</p></td>
                            <td><p class="dnnDevTools-copy">{{logDetail.logMessage.Id}}</p></td>
                        </tr>
                        <tr>
                            <td><p class="dnnDevTools-copy">Logger</p></td>
                            <td><p class="dnnDevTools-copy">{{logDetail.logMessage.Logger}}</p></td>
                        </tr>
                        <tr>
                            <td><p class="dnnDevTools-copy">ClassName</p></td>
                            <td><p class="dnnDevTools-copy">{{logDetail.logMessage.ClassName}}</p></td>
                        </tr>
                        <tr>
                            <td><p class="dnnDevTools-copy">MethodName</p></td>
                            <td><p class="dnnDevTools-copy">{{logDetail.logMessage.MethodName}}</p></td>
                        </tr>
                        <tr>
                            <td><p class="dnnDevTools-copy">Message</p></td>
                            <td><p class="dnnDevTools-copy">{{logDetail.logMessage.Message}}</p></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </script>
    </div>
    
    <script src="Scripts/angular.js"></script>
    <script src="Scripts/angular-ui-router.js"></script>
    <script src="Scripts/angular-sanitize.js"></script>

    <script>
        (function () {
            angular.module('app', ['ui.router', 'ngSanitize'])
                .config(config)
                .factory('remoteData', remoteData)
                .controller('OverviewController', OverviewController)
                .controller('MailDetailController', MailDetailController)
                .controller('EventDetailController', EventDetailController)
                .controller('LogDetailController', LogDetailController);


            function config($stateProvider, $urlRouterProvider) {
                $urlRouterProvider.otherwise('/');
                $stateProvider
                    .state('overview', {
                        url: '/{filter}',
                        templateUrl: 'dnnDevTools-overview.html',
                        controller: 'OverviewController as overview'
                    })
                    .state('mailDetail', {
                        url: '/maildetail/{id}',
                        templateUrl: 'dnnDevTools-mail-detail.html',
                        controller: 'MailDetailController as mailDetail',
                        resolve: {
                            mail: function ($stateParams, remoteData) {
                                return remoteData.mailDetail($stateParams.id);
                            }
                        }
                    })
                    .state('eventDetail', {
                        url: '/dnneventdetail/{id}',
                        templateUrl: 'dnnDevTools-event-detail.html',
                        controller: 'EventDetailController as eventDetail',
                        resolve: {
                            dnnEvent: function ($stateParams, remoteData) {
                                return remoteData.eventDetail($stateParams.id);
                            }
                        }
                    })
                    .state('logDetail', {
                        url: '/logmessagedetail/{id}',
                        templateUrl: 'dnnDevTools-log-detail.html',
                        controller: 'LogDetailController as logDetail',
                        resolve: {
                            logMessage: function ($stateParams, $filter, remoteData) {
                                return remoteData.logDetail($stateParams.id);
                            }
                        }
                    });
            }

            function OverviewController($scope, $window, $state, $stateParams, remoteData) {
                var vm = this;

                vm.stream = null;
                vm.filter = $stateParams.filter;
                vm.showDetail = showDetail;
                vm.removeMail = removeMail;
                vm.downloadMail = downloadMail;

                activate();

                function activate() {
                    // load stream
                    switch (vm.filter) {
                        case 'Mail':
                            remoteData.mailList().then(function (response) {
                                vm.stream = response.data;
                            });
                            break;
                        case 'DnnEvent':
                            remoteData.eventList().then(function (response) {
                                vm.stream = response.data;
                            });
                            break;
                        case 'LogMessage':
                            remoteData.logList().then(function (response) {
                                vm.stream = response.data;
                            });
                            break;
                        default:
                            remoteData.stream().then(function (response) {
                                vm.stream = response.data.all;
                            });
                    }
                }

                function showDetail(item) {
                    switch (item.Type) {
                        case 'Mail':
                            $state.go('mailDetail', {id:item.Id});
                            break;
                        case 'DnnEvent':
                            $state.go('eventDetail', {id:item.Id});
                            break;
                        case 'LogMessage':
                            $state.go('logDetail', {id:item.Id});
                            break;
                    }
                }

                function removeMail($event, mail) {
                    $event.stopPropagation();

                    remoteData.removeMail(mail).then(success, error);

                    function success(response) {
                        var index = vm.stream.indexOf(mail);
                        vm.stream.splice(index, 1);
                    }

                    function error(response) {
                        // TODO handle error
                    }
                }

                function downloadMail($event, id) {
                    $event.stopPropagation();
                    $window.location.href = 'api/mail/download?id=' + id;
                }

                function getUrlParameterByName(name) {
                    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                        results = regex.exec(location.search);
                    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
                }
            }

            function MailDetailController(mail) {
                var vm = this;

                vm.mail = mail.data;
            }

            function EventDetailController(dnnEvent) {
                var vm = this;

                vm.dnnEvent = dnnEvent.data;
            }

            function LogDetailController(logMessage) {
                var vm = this;

                vm.logMessage = logMessage.data;
            }

            function remoteData($http, $q) {
                return {
                    stream: stream,
                    mailList: mailList,
                    logList: logList,
                    eventList: eventList,
                    eventDetail: eventDetail,
                    logDetail: logDetail,
                    mailDetail: mailDetail,
                    removeMail: removeMail
                }

                function stream() {
                    var deferred = $q.defer();

                    $http({
                        method: 'GET',
                        url: 'api/stream?skip=0&take=100&search=',
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    }).then(success, error);

                    function success(response) {
                        deferred.resolve(response);
                    }

                    function error(response) {
                        deferred.reject(response);
                    }

                    return deferred.promise;
                }

                function mailList() {
                    var deferred = $q.defer();

                    $http({
                        method: 'GET',
                        url: 'api/mail/list?skip=0&take=10&search=',
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    }).then(success, error);

                    function success(response) {
                        deferred.resolve(response);
                    }

                    function error(response) {
                        deferred.reject(response);
                    }

                    return deferred.promise;
                }

                function eventList() {
                    var deferred = $q.defer();

                    $http({
                        method: 'GET',
                        url: 'api/dnnEvent/list?skip=0&take=10&search=',
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    }).then(success, error);

                    function success(response) {
                        deferred.resolve(response);
                    }

                    function error(response) {
                        deferred.reject(response);
                    }

                    return deferred.promise;
                }

                function logList() {
                    var deferred = $q.defer();

                    $http({
                        method: 'GET',
                        url: 'api/log/list?skip=0&take=10&search=',
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    }).then(success, error);

                    function success(response) {
                        deferred.resolve(response);
                    }

                    function error(response) {
                        deferred.reject(response);
                    }

                    return deferred.promise;
                }

                function eventDetail(id) {
                    return $http({
                        method: 'GET',
                        url: 'api/dnnEvent/detail',
                        params: {
                            id: id
                        },
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    });
                }

                function logDetail(id) {
                    return $http({
                        method: 'GET',
                        url: 'api/log/detail',
                        params: {
                            id: id
                        },
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    });
                }

                function mailDetail(id) {
                    return $http({
                        method: 'GET',
                        url: 'api/mail/detail',
                        params: {
                            id: id
                        },
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    });
                }

                function removeMail(mail) {
                    return $http({
                        method: 'delete',
                        url: 'api/mail/delete',
                        params: {
                            id: mail.Id
                        },
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    });
                }
            }
        }());
    </script>
</body>
</html>