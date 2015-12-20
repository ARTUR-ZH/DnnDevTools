<%@ Page Language="C#" %>
<html>
<head>
    <title>DotNetNuke Developer Tools Mail Window</title>
    <style>
        body {
            margin: 0;
        } 

        /* style scrollbar in webkit */
        ::-webkit-scrollbar {
            width: 10px;
        }

        ::-webkit-scrollbar-track {
            background-color: #272822;
        }

        ::-webkit-scrollbar-thumb {
            background-color: #12120f;
        }

        [ng-cloak] {
            display: none !important;
        }

        /* SPINNER */
        @keyframes spinner {
            to {transform: rotate(360deg);}
        }
         
        @-webkit-keyframes spinner {
            to {-webkit-transform: rotate(360deg);}
        }
         
        .dnn-mdt-spinner {
            min-width: 24px;
            min-height: 24px;
        }

        .dnn-mdt-spinner:before {
            content: '';
            position: absolute;
            top: 50%;
            left: 50%;
            width: 16px;
            height: 16px;
            margin-top: -10px;
            margin-left: -10px;
            border-radius: 50%;
            border: 2px solid rgba(255, 255, 255, .3);
            border-top-color: rgba(255, 255, 255, .6);
            animation: spinner .6s linear infinite;
            -webkit-animation: spinner .6s linear infinite;
        }

        /* FONTS */
        .dnn-mdt-copy {
            font-family: Arial, sans-serif;
            font-size: 13px;
            margin-top: 0;
            margin-bottom: 0;
        }

        .dnn-mdt-pre {
            margin-top: 0;
            margin-bottom: 0;
        }

        /* TABLE */
        .dnn-mdt-table {
            width: 100%;
            table-layout: fixed;
            border-spacing: 0;
            border-collapse: separate;
        }

        .dnn-mdt-tableCell {
            padding: 7px 14px;
            text-align: left;
            border-bottom: 1px solid rgba(255, 255, 255, 0.1);

            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .dnn-mdt-tableCellSender {
            width: 25%;
            color: #ffffff;
        }
        .dnn-mdt-tableCellSubject {
            width: 20%;
            color: #ffffff;
        }
        .dnn-mdt-tableCellSentOn {
            width: 20%;
            color: rgba(255, 255, 255, 0.3);
        }
        .dnn-mdt-tableCellTo {
            width: 25%;
            color: rgba(255, 255, 255, 0.3);
        }
        .dnn-mdt-tableCellActions {
            width: 10%;
            text-align: right;
        }

        /* LIST */
        .dnn-mdt-list {
            padding: 20px;
            background-color: #272822;
            color: #ffffff;
        }
        .dnn-mdt-listEmpty {
            text-align: center;
        }

        /* DETAIL */
        .dnn-mdt-detail {
            padding: 20px;
            background-color: #ffffff;
            color: #111111;
        }
    </style>
</head>
<body>
    
    <% Response.Write(System.Web.Helpers.AntiForgery.GetHtml()); %>

    <div ng-app="app">
        <div ui-view role="main"></div>

        <script type="text/ng-template" id="dnn-mdt-overview.html">
            <div class="dnn-mdt-list">
                <p ng-if="overview.mailList.length === 0" class="dnn-mdt-listEmpty dnn-mdt-copy">Your inbox is currently empty.</p>
                <table class="dnn-mdt-table">
                    <tbody>
                        <tr ng-repeat="mail in overview.mailList | orderBy:'-SentOn'">
                            <td class="dnn-mdt-tableCell dnn-mdt-copy dnn-mdt-tableCellSender">{{mail.Sender}}</td>
                            <td class="dnn-mdt-tableCell dnn-mdt-copy dnn-mdt-tableCellSubject">{{mail.Subject}}</td>
                            <td class="dnn-mdt-tableCell dnn-mdt-copy dnn-mdt-tableCellSentOn">{{mail.SentOn | date:'dd.MM.yyyy HH:mm'}}</td>
                            <td class="dnn-mdt-tableCell dnn-mdt-copy dnn-mdt-tableCellTo">{{mail.To}}</td>
                            <td class="dnn-mdt-tableCell dnn-mdt-tableCellActions">
                                <button ui-sref="mailDetail({id:mail.Id})" type="button">+</button>
                                <button ng-click="overview.removeMail(mail)" type="button">x</button>
                                <button ng-click="overview.downloadMail(mail.Id)" type="button">down</button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </script>

        <script type="text/ng-template" id="dnn-mdt-mail-detail.html">
            <div class="dnn-mdt-detail">
                <a ui-sref="overview">back to overview</a>
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
                        url: '/',
                        templateUrl: 'dnn-mdt-overview.html',
                        controller: 'OverviewController as overview'
                    })
                    .state('mailDetail', {
                        url: '/mail/{id}',
                        templateUrl: 'dnn-mdt-mail-detail.html',
                        controller: 'MailDetailController as mailDetail',
                        resolve: {
                            mail: function ($stateParams, remoteData) {
                                return remoteData.mailDetail($stateParams.id);
                            }
                        }
                    });
            }

            function OverviewController($scope, $window, remoteData) {
                var vm = this;

                vm.mailList = [];
                vm.removeMail = removeMail;
                vm.downloadMail = downloadMail;

                activate();

                function activate() {
                    remoteData.mailList().then(function (response) {
                        vm.mailList = response.data;
                    });
                    remoteData.logList().then(function () {
                        console.log('logList');
                    });
                    remoteData.eventList().then(function () {
                        console.log('eventList');
                    });
                }

                function removeMail(mail) {
                    remoteData.removeMail(mail).then(success, error);

                    function success(response) {
                        var index = vm.mailList.indexOf(mail);
                        vm.mailList.splice(index, 1);
                    }

                    function error(response) {
                        // TODO handle error
                        console.log(response);
                    }
                }

                function downloadMail(id) {
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

                vm.mail = mail;
            }

            function remoteData($http, $q) {
                return {
                    mailList: mailList,
                    logList: logList,
                    eventList: eventList,
                    mailDetail: mailDetail,
                    removeMail: removeMail
                }

                function mailList() {
                    return $http({
                        method: 'GET',
                        url: 'api/mail/list',
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    });
                }

                function logList() {
                    return $http({
                        method: 'GET',
                        url: 'api/log/list',
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    });
                }

                function eventList() {
                    return $http({
                        method: 'GET',
                        url: 'api/dnnEvent/list',
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