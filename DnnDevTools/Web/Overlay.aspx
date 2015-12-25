<%@ Page Language="C#" %>
<html>
<head>
    <title>DotNetNuke Developer Tools Mail Window</title>

    <link rel="stylesheet" type="text/css" href="Styles/dnn.css">

    <style>
        /* LIST */
        .dnn-mdt-listEmpty {
            text-align: center;
        }

        /* DETAIL */
        .dnn-mdt-detail {
            padding: 20px;
        }
    </style>
</head>
<body class="dnn-mdt-overview dnn-mdt-bgColorWhite">
    
    <% Response.Write(System.Web.Helpers.AntiForgery.GetHtml()); %>

    <div ng-app="app">
        <div ui-view role="main"></div>

        <script type="text/ng-template" id="dnn-mdt-overview.html">
            <div class="dnn-mdt-stream">
                <div class="dnn-mdt-filterList dnn-mdt-bgColorDnnBlue">
                    <a ui-sref="overview({filter: null})" ng-class="{'dnn-mdt-active': !overview.filter}" class="dnn-mdt-iconLabelButton dnn-mdt-copy dnn-mdt-colorWhite dnn-mdt-removeIcon">Show all</a>
                    <a ui-sref="overview({filter: 'Mail'})" ng-class="{'dnn-mdt-active': overview.filter === 'Mail'}" class="dnn-mdt-iconLabelButton dnn-mdt-copy dnn-mdt-colorWhite"><span class="dnn-mdt-envelopeClosedIcon dnn-mdt-icon16x16"></span>Mails</a>
                    <a ui-sref="overview({filter: 'DnnEvent'})" ng-class="{'dnn-mdt-active': overview.filter === 'DnnEvent'}" class="dnn-mdt-iconLabelButton dnn-mdt-copy dnn-mdt-colorWhite"><span class="dnn-mdt-audioIcon dnn-mdt-icon16x16"></span>Events</a>
                    <a ui-sref="overview({filter: 'LogMessage'})" ng-class="{'dnn-mdt-active': overview.filter === 'LogMessage'}" class="dnn-mdt-iconLabelButton dnn-mdt-copy dnn-mdt-colorWhite"><span class="dnn-mdt-listIcon dnn-mdt-icon16x16"></span>Logs</a>
                </div>

                <div class="dnn-mdt-streamWrapper">
                    <div ng-if="!overview.stream" class="dnn-mdt-spinner"></div>
                    <p ng-if="overview.stream.length === 0" class="dnn-mdt-listEmpty dnn-mdt-copy">Your inbox is currently empty.</p>
                    <ul class="dnn-mdt-streamList">
                        <li ng-repeat="item in overview.stream | orderBy:'-TimeStamp'" ng-click="overview.showDetail(item)" class="dnn-mdt-streamItem">
                            <div class="dnn-mdt-streamItemTimestamp dnn-mdt-streamItemCell">
                                <span ng-class="{'dnn-mdt-envelopeClosedIcon-111111': item.Type === 'Mail', 'dnn-mdt-audioIcon-111111': item.Type === 'DnnEvent', 'dnn-mdt-listIcon-111111': item.Type === 'LogMessage'}" class="dnn-mdt-streamItemIcon dnn-mdt-icon16x16"></span>
                                <p class="dnn-mdt-copy">{{item.TimeStamp | date:'dd.MM.yyyy HH:mm'}}</p>
                            </div>

                            <p ng-if="item.Type === 'DnnEvent'" class="dnn-mdt-streamItemCell dnn-mdt-copy dnn-mdt-streamItemEventLogType">{{item.LogType || '&nbsp;'}}</p>
                            <p ng-if="item.Type === 'DnnEvent'" class="dnn-mdt-streamItemCell dnn-mdt-copy dnn-mdt-streamItemEventMessage">{{item.Message || '&nbsp;'}}</p>
                            <p ng-if="item.Type === 'DnnEvent'" class="dnn-mdt-streamItemCell dnn-mdt-copy dnn-mdt-streamItemEventUsername">{{item.Username || '&nbsp;'}}</p>
                            <p ng-if="item.Type === 'DnnEvent'" class="dnn-mdt-streamItemCell dnn-mdt-copy dnn-mdt-streamItemEventPortal">{{item.Portal || '&nbsp;'}}</p>

                            <div ng-if="item.Type === 'LogMessage'" class="dnn-mdt-streamItemCell dnn-mdt-streamItemLogLevel"><p class="dnn-mdt-streamLabel dnn-mdt-copy" ng-class="{'dnn-mdt-bgColorRed': item.Level === 'ERROR', 'dnn-mdt-bgColorOrange': item.Level === 'WARN'}">{{item.Level}}</p></div>
                            <p ng-if="item.Type === 'LogMessage'" class="dnn-mdt-streamItemCell dnn-mdt-copy dnn-mdt-streamItemLogMessage">{{item.Message}}</p>
                            <p ng-if="item.Type === 'LogMessage'" class="dnn-mdt-streamItemCell dnn-mdt-copy dnn-mdt-streamItemLogClassName">{{item.ClassName}}</p>
                            <p ng-if="item.Type === 'LogMessage'" class="dnn-mdt-streamItemCell dnn-mdt-copy dnn-mdt-streamItemLogMethodName">{{item.MethodName}}</p>

                            <p ng-if="item.Type === 'Mail'" class="dnn-mdt-streamItemCell dnn-mdt-copy dnn-mdt-streamItemMailSender">{{item.Sender}}</p>
                            <p ng-if="item.Type === 'Mail'" class="dnn-mdt-streamItemCell dnn-mdt-copy dnn-mdt-streamItemMailSubject">{{item.Subject}}</p>
                            <p ng-if="item.Type === 'Mail'" class="dnn-mdt-streamItemCell dnn-mdt-copy dnn-mdt-streamItemMailTo">{{item.To}}</p>
                            <div ng-if="item.Type === 'Mail'" class="dnn-mdt-streamItemCell dnn-mdt-streamItemMailActions">
                                <button ng-click="overview.removeMail($event, item)" type="button" class="dnn-mdt-iconButton">
                                    <span class="dnn-mdt-trashIcon dnn-mdt-icon16x16"></span>
                                </button>
                                <button ng-click="overview.downloadMail($event, item.Id)" type="button" class="dnn-mdt-iconButton">
                                    <span class="dnn-mdt-downloadIcon dnn-mdt-icon16x16"></span>
                                </button>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </script>

        <script type="text/ng-template" id="dnn-mdt-mail-detail.html">
            <div class="dnn-mdt-detail">
                <a ui-sref="overview" class="dnn-mdt-copy">back to overview</a>
                <div ng-if="mailDetail.mail.BodyIsHtml" ng-bind-html="mailDetail.mail.Body" class="dnn-mdt-copy"></div>
                <pre ng-if="!mailDetail.mail.BodyIsHtml" ng-bind-html="mailDetail.mail.Body" class="dnn-mdt-pre"></pre>
                <p ng-if="mailDetail.mail && mailDetail.mail.Body === ''" class="dnn-mdt-copy">Body is empty.</p>
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
                .controller('MailDetailController', MailDetailController);

            function config($stateProvider, $urlRouterProvider) {
                $urlRouterProvider.otherwise('/');
                $stateProvider
                    .state('overview', {
                        url: '/{filter}',
                        templateUrl: 'dnn-mdt-overview.html',
                        controller: 'OverviewController as overview'
                    })
                    .state('mailDetail', {
                        url: '/maildetail/{id}',
                        templateUrl: 'dnn-mdt-mail-detail.html',
                        controller: 'MailDetailController as mailDetail',
                        resolve: {
                            mail: function ($stateParams, remoteData) {
                                return remoteData.mailDetail($stateParams.id);
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
                        console.log(response);
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

            function MailDetailController($stateParams, mail) {
                var vm = this;

                vm.mail = mail.data;
            }

            function remoteData($http, $q) {
                return {
                    stream: stream,
                    mailList: mailList,
                    logList: logList,
                    eventList: eventList,
                    mailDetail: mailDetail,
                    removeMail: removeMail
                }

                function stream() {
                    return $http({
                        method: 'GET',
                        url: 'api/stream?skip=0&take=100&search=',
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    });
                }

                function mailList() {
                    return $http({
                        method: 'GET',
                        url: 'api/mail/list?skip=0&take=10&search=',
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    });
                }

                function eventList() {
                    return $http({
                        method: 'GET',
                        url: 'api/dnnEvent/list?skip=0&take=10&search=',
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    });
                }

                function logList() {
                    return $http({
                        method: 'GET',
                        url: 'api/log/list?skip=0&take=10&search=',
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