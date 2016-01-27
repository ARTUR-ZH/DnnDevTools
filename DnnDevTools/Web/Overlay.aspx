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
                <div class="dnnDevTools-sticky">
                    <div class="dnnDevTools-filterList dnnDevTools-bgColorDnnBlue">
                        <a ui-sref="overview({filter: null})" ng-class="{'dnnDevTools-active': !overview.filter}" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite dnnDevTools-removeIcon">Show all</a>
                        <a ui-sref="overview({filter: 'Mail'})" ng-class="{'dnnDevTools-active': overview.filter === 'Mail'}" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite"><span class="dnnDevTools-envelopeClosedIcon dnnDevTools-icon16x16"></span>Mails</a>
                        <a ui-sref="overview({filter: 'DnnEvent'})" ng-class="{'dnnDevTools-active': overview.filter === 'DnnEvent'}" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite"><span class="dnnDevTools-audioIcon dnnDevTools-icon16x16"></span>Events</a>
                        <a ui-sref="overview({filter: 'LogMessage'})" ng-class="{'dnnDevTools-active': overview.filter === 'LogMessage'}" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite"><span class="dnnDevTools-listIcon dnnDevTools-icon16x16"></span>Logs</a>
                    </div>
                    <input type="search" ng-change="overview.onChangeSearchInput()" ng-model="overview.searchInput" class="dnnDevTools-search dnnDevTools-copy" placeholder="type your search">
                </div>

                <div class="dnnDevTools-streamWrapper">
                    <div ng-if="!overview.stream" class="dnnDevTools-spinner"></div>
                    <p ng-if="overview.stream.length === 0" class="dnnDevTools-listEmpty dnnDevTools-copy">No data available.</p>
                    <table class="dnnDevTools-streamList">
                        <tbody>
                            <tr ng-repeat="item in overview.stream" ng-click="overview.showDetail(item)" class="dnnDevTools-streamItem">
                                <td class="dnnDevTools-streamItemTimestamp dnnDevTools-streamItemCell">
                                    <span ng-class="{'dnnDevTools-envelopeClosedIcon-111111': item.Type === 'Mail', 'dnnDevTools-audioIcon-111111': item.Type === 'DnnEvent', 'dnnDevTools-listIcon-111111': item.Type === 'LogMessage'}" class="dnnDevTools-streamItemIcon dnnDevTools-icon16x16"></span>
                                    <p class="dnnDevTools-copy">{{item.TimeStamp | date:'dd.MM.yyyy HH:mm'}}</p>
                                </td>

                                <td ng-if="item.Type === 'DnnEvent'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemEventLogType">{{item.LogType || '&nbsp;'}}</td>
                                <td ng-if="item.Type === 'DnnEvent'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemEventMessage">{{item.Message || '&nbsp;'}}</td>
                                <td ng-if="item.Type === 'DnnEvent'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemEventUsername">{{item.Username || '&nbsp;'}}</td>
                                <td ng-if="item.Type === 'DnnEvent'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemEventPortal">{{item.Portal || '&nbsp;'}}</td>

                                <td ng-if="item.Type === 'LogMessage'" class="dnnDevTools-streamItemCell dnnDevTools-streamItemLogLevel"><p class="dnnDevTools-streamLabel dnnDevTools-copy" ng-class="{'dnnDevTools-bgColorRed': item.Level === 'ERROR', 'dnnDevTools-bgColorOrange': item.Level === 'WARN'}">{{item.Level || '&nbsp;'}}</p></td>
                                <td ng-if="item.Type === 'LogMessage'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemLogMessage">{{item.Message || '&nbsp;'}}</td>
                                <td ng-if="item.Type === 'LogMessage'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemLogLogger">{{item.Logger || '&nbsp;'}}</td>
                                <td ng-if="item.Type === 'LogMessage'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemLogClassNameAndMethodName">{{item.ClassName}}.{{item.MethodName}}</td>

                                <td ng-if="item.Type === 'Mail'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemMailSender">{{item.Sender || '&nbsp;'}}</td>
                                <td ng-if="item.Type === 'Mail'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemMailSubject">{{item.Subject || '&nbsp;'}}</td>
                                <td ng-if="item.Type === 'Mail'" class="dnnDevTools-streamItemCell dnnDevTools-copy dnnDevTools-streamItemMailTo">{{item.To || '&nbsp;'}}</td>
                                <td ng-if="item.Type === 'Mail'" class="dnnDevTools-streamItemCell dnnDevTools-streamItemMailActions">
                                    <button ng-click="overview.removeMail($event, item)" type="button" class="dnnDevTools-iconButton">
                                        <span class="dnnDevTools-trashIcon dnnDevTools-icon16x16"></span>
                                    </button>
                                    <button ng-click="overview.downloadMail($event, item.Id)" type="button" class="dnnDevTools-iconButton">
                                        <span class="dnnDevTools-downloadIcon dnnDevTools-icon16x16"></span>
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    <div ng-if="overview.displayShowMoreButton && overview.stream && overview.stream.length > 0" class="dnnDevTools-showMoreWrapper">
                        <button type="button" ng-click="overview.showMore()" class="dnnDevTools-buttonDnnRed dnnDevTools-copy dnnDevTools-marginTop2">show more</button>
                    </div>
                </div>
            </div>
        </script>

        <script type="text/ng-template" id="dnnDevTools-mail-detail.html">
            <div class="dnnDevTools-filterList dnnDevTools-bgColorDnnBlue">
                <a ui-sref="overview" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite dnnDevTools-backButton">back to overview</a>
            </div>

            <div class="dnnDevTools-panel dnnDevTools-bgColorGrey">
                <div class="dnnDevTools-marginBottom2">
                    <p class="dnnDevTools-copy dnnDevTools-colorGrey3">
                        <span ng-if="mailDetail.mail.Sender !== ''"><span class="dnnDevTools-copyBold dnnDevTools-colorBlack">from</span> {{mailDetail.mail.Sender}} </span>
                        <span ng-if="mailDetail.mail.To !== ''"><span class="dnnDevTools-copyBold dnnDevTools-colorBlack">to</span> {{mailDetail.mail.To}}</span>
                    </p>
                </div>

                <p class="dnnDevTools-copy dnnDevTools-marginBottom1">{{mailDetail.mail.TimeStamp | date:'dd.MM.yyyy HH:mm'}}</p>
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
            <div class="dnnDevTools-panel dnnDevTools-bgColorGrey">
                <div class="dnnDevTools-marginBottom2">
                    <p class="dnnDevTools-copy dnnDevTools-marginBottom1">{{eventDetail.dnnEvent.TimeStamp | date:'dd.MM.yyyy HH:mm'}}</p>
                    <h1 class="dnnDevTools-headline">{{eventDetail.dnnEvent.LogType}}</h1>
                </div>
                <table>
                    <tbody>
                        <tr>
                            <td class="dnnDevTools-tableCell dnnDevTools-bgColorWhite"><p class="dnnDevTools-copyBold">Id</p></td>
                            <td class="dnnDevTools-tableCell"><p class="dnnDevTools-copy">{{eventDetail.dnnEvent.Id}}</p></td>
                        </tr>
                        <tr>
                            <td class="dnnDevTools-tableCell dnnDevTools-bgColorWhite"><p class="dnnDevTools-copyBold">LogType</p></td>
                            <td class="dnnDevTools-tableCell"><p class="dnnDevTools-copy">{{eventDetail.dnnEvent.LogType}}</p></td>
                        </tr>
                        <tr>
                            <td class="dnnDevTools-tableCell dnnDevTools-bgColorWhite"><p class="dnnDevTools-copyBold">Portal</p></td>
                            <td class="dnnDevTools-tableCell"><p class="dnnDevTools-copy">{{eventDetail.dnnEvent.Portal}}</p></td>
                        </tr>
                        <tr>
                            <td class="dnnDevTools-tableCell dnnDevTools-bgColorWhite"><p class="dnnDevTools-copyBold">Username</p></td>
                            <td class="dnnDevTools-tableCell"><p class="dnnDevTools-copy">{{eventDetail.dnnEvent.Username}}</p></td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="dnnDevTools-panel">
                <p class="dnnDevTools-copy">{{eventDetail.dnnEvent.Message}}</p>
            </div>
        </script>

        <script type="text/ng-template" id="dnnDevTools-log-detail.html">
            <div class="dnnDevTools-filterList dnnDevTools-bgColorDnnBlue">
                <a ui-sref="overview" class="dnnDevTools-iconLabelButton dnnDevTools-copy dnnDevTools-colorWhite dnnDevTools-backButton">back to overview</a>
            </div>

            <div class="dnnDevTools-panel">
                <p class="dnnDevTools-copy dnnDevTools-marginBottom1">{{logDetail.logMessage.TimeStamp | date:'dd.MM.yyyy HH:mm'}}</p>
                <p class="dnnDevTools-streamLabel dnnDevTools-displayInlineBlock dnnDevTools-copy" ng-class="{'dnnDevTools-bgColorRed': logDetail.logMessage.Level === 'ERROR', 'dnnDevTools-bgColorOrange': logDetail.logMessage.Level === 'WARN'}">{{logDetail.logMessage.Level || '&nbsp;'}}</p>
            </div>

            <div class="dnnDevTools-panel dnnDevTools-bgColorGrey">
                <table>
                    <tbody>
                        <tr>
                            <td class="dnnDevTools-tableCell dnnDevTools-bgColorWhite"><p class="dnnDevTools-copyBold">Id</p></td>
                            <td class="dnnDevTools-tableCell"><p class="dnnDevTools-copy">{{logDetail.logMessage.Id}}</p></td>
                        </tr>
                        <tr>
                            <td class="dnnDevTools-tableCell dnnDevTools-bgColorWhite"><p class="dnnDevTools-copyBold">Logger</p></td>
                            <td class="dnnDevTools-tableCell"><p class="dnnDevTools-copy">{{logDetail.logMessage.Logger}}</p></td>
                        </tr>
                        <tr>
                            <td class="dnnDevTools-tableCell dnnDevTools-bgColorWhite"><p class="dnnDevTools-copyBold">ClassName</p></td>
                            <td class="dnnDevTools-tableCell"><p class="dnnDevTools-copy">{{logDetail.logMessage.ClassName}}</p></td>
                        </tr>
                        <tr>
                            <td class="dnnDevTools-tableCell dnnDevTools-bgColorWhite"><p class="dnnDevTools-copyBold">MethodName</p></td>
                            <td class="dnnDevTools-tableCell"><p class="dnnDevTools-copy">{{logDetail.logMessage.MethodName}}</p></td>
                        </tr>
                        <tr>
                            <td class="dnnDevTools-tableCell dnnDevTools-bgColorWhite"><p class="dnnDevTools-copyBold">Username</p></td>
                            <td class="dnnDevTools-tableCell"><p class="dnnDevTools-copy">{{logDetail.logMessage.Username}}</p></td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="dnnDevTools-panel">
                <p class="dnnDevTools-copy">{{logDetail.logMessage.Message}}</p>
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

            function OverviewController($scope, $window, $state, $stateParams, $timeout, remoteData) {
                var vm = this,
                    startCount = 100,
                    showMoreCount = 50,
                    searchInputTimeout;

                vm.stream = null;
                vm.filter = $stateParams.filter;
                vm.showDetail = showDetail;
                vm.removeMail = removeMail;
                vm.downloadMail = downloadMail;
                vm.showMore = showMore;
                vm.onChangeSearchInput = onChangeSearchInput;
                vm.displayShowMoreButton = true;

                activate();

                function activate() {
                    remoteData.stream('', 0, startCount, vm.searchInput, vm.filter).then(function (response) {
                        vm.displayShowMoreButton = response.displayShowMoreButton;
                        vm.stream = response.data.all;
                    });
                }

                function showMore() {
                    remoteData.stream(vm.stream[0].Id, vm.stream.length, showMoreCount, vm.searchInput, vm.filter).then(function (response) {
                        vm.displayShowMoreButton = response.displayShowMoreButton;
                        vm.stream = vm.stream.concat(response.data.all);
                    });
                }

                function onChangeSearchInput() {
                    if (searchInputTimeout) {
                        $timeout.cancel(searchInputTimeout);
                    }

                    searchInputTimeout = $timeout(function() {
                        if (vm.searchInput.length > 3 || vm.searchInput === '') {
                            vm.stream = null;
                            remoteData.stream('', 0, startCount, vm.searchInput, vm.filter).then(function (response) {
                                vm.displayShowMoreButton = response.displayShowMoreButton;
                                vm.stream = response.data.all;
                            });
                        }
                    }, 500);
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
                    eventDetail: eventDetail,
                    logDetail: logDetail,
                    mailDetail: mailDetail,
                    removeMail: removeMail
                }

                function stream(start, skip, take, search, type) {
                    var deferred = $q.defer();

                    start = start || '';
                    skip = skip || '';
                    take = take || '';
                    search = search || '';
                    type = type ? type.toLowerCase() : '';

                    // get one more entry to test if there are more entries to come
                    // if there are not - don't show the 'show more' button
                    take += 1;

                    parameters = '?start=' + start + '&skip=' + skip + '&take=' + take + '&search=' + search;

                    if (type.length > 0) {
                        parameters += '&type=' + type;
                    }

                    $http({
                        method: 'GET',
                        url: 'api/stream' + parameters,
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    }).then(success, error);

                    function success(response) {
                        response.displayShowMoreButton = response.data.all.length === take;

                        // remove entry that has been added 'artificially'
                        response.data.all.pop();

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