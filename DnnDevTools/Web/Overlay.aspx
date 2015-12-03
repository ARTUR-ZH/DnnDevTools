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
        <div ng-controller="MailController as mail">
            <div ng-if="mail.state === 'loading'" class="dnn-mdt-spinner"></div>
            <div ng-if="mail.state === 'list'" class="dnn-mdt-list" ng-cloak>
                <table class="dnn-mdt-table">
                    <tbody>
                        <tr ng-repeat="item in mail.list | orderBy:'-SentOn'">
                            <td class="dnn-mdt-tableCell dnn-mdt-copy dnn-mdt-tableCellSender">{{item.Sender}}</td>
                            <td class="dnn-mdt-tableCell dnn-mdt-copy dnn-mdt-tableCellSubject">{{item.Subject}}</td>
                            <td class="dnn-mdt-tableCell dnn-mdt-copy dnn-mdt-tableCellSentOn">{{item.SentOn | date:'dd.MM.yyyy HH:mm'}}</td>
                            <td class="dnn-mdt-tableCell dnn-mdt-copy dnn-mdt-tableCellTo">{{item.To}}</td>
                            <td class="dnn-mdt-tableCell dnn-mdt-tableCellActions">
                                <button ng-click="mail.showDetail(item.Id)" type="button">+</button>
                                <button ng-click="mail.remove(item)" type="button">x</button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div ng-if="mail.state === 'detail'" class="dnn-mdt-detail" ng-cloak>
                <button type="button" ng-click="mail.showList()">back</button>
                <p ng-if="mail.currentMail.BodyHtml" ng-bind-html="mail.currentMail.BodyHtml" class="dnn-mdt-copy"></p>
                <p ng-if="mail.currentMail && mail.currentMail.BodyHtml === ''" class="dnn-mdt-copy">Body is empty.</p>
            </div>
        </div>
    </div>
    
    <script src="Scripts/angular.js"></script>
    <script src="Scripts/angular-sanitize.js"></script>

    <script>
        (function () {
            angular.module('app', ['ngSanitize'])
                .factory('remoteData', remoteData)
                .controller('MailController', MailController);

            function remoteData($http, $q) {
                return {
                    list: list,
                    detail: detail,
                    remove: remove
                }

                function list() {
                    var deferred = $q.defer();

                    $http({
                        method: 'GET',
                        url: 'api/mail/list',
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .success(success)
                        .error(error);

                    return deferred.promise;

                    function success(response) {
                        deferred.resolve(response);
                    }

                    function error(response) {
                        deferred.reject(response);
                    }
                }

                function detail(id) {
                    var deferred = $q.defer();

                    $http({
                        method: 'GET',
                        url: 'api/mail/detail',
                        params: {
                            id: id
                        },
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .success(success)
                        .error(error);

                    return deferred.promise;

                    function success(response) {
                        deferred.resolve(response);
                    }

                    function error(response) {
                        deferred.reject(response);
                    }
                }

                function remove(mail) {
                    var deferred = $q.defer();

                    $http({
                        method: 'delete',
                        url: 'api/mail/delete',
                        params: {
                            id: mail.Id
                        },
                        headers: {
                            'requestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
                        }
                    })
                        .success(success)
                        .error(error);

                    return deferred.promise;

                    function success(response) {
                        deferred.resolve(response);
                    }

                    function error(response) {
                        deferred.reject(response);
                    }
                }
            }

            function MailController($scope, remoteData) {
                var vm = this;

                vm.list = undefined;
                vm.currentMail = undefined;
                vm.state = 'loading';
                
                vm.showList = showList;
                vm.showDetail = showDetail;
                vm.remove = remove;

                activate();

                function activate() {
                    // get id of mail which should be initially highlighted
                    var currentMailId = getUrlParameterByName('id');

                    if (currentMailId) {
                        showDetail(currentMailId);
                    } else {
                        showList();
                    }
                }

                function showList() {
                    vm.currentMail = undefined;

                    // get list if its not already available
                    if (!vm.list) {
                        vm.state = 'loading';
                        remoteData.list().then(success, error);
                    } else {
                        vm.state = 'list';
                    }

                    function success(response) {
                        vm.state = 'list';
                        vm.list = response;
                    }

                    function error(response) {
                        // TODO handle error
                        console.log(response);
                    }
                }

                function showDetail(id) {
                    remoteData.detail(id).then(success, error);
                    vm.state = 'loading';

                    function success(response) {
                        vm.state = 'detail';
                        vm.currentMail = response;
                    }

                    function error(response) {
                        // TODO handle error
                        console.log(response);
                    }
                }

                function remove(mail) {
                    remoteData.remove(mail).then(success, error);

                    function success(response) {
                        var index = vm.list.indexOf(mail);
                        vm.list.splice(index, 1);
                    }

                    function error(response) {
                        // TODO handle error
                        console.log(response);
                    }
                }

                function getUrlParameterByName(name) {
                    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                        results = regex.exec(location.search);
                    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
                }
            }
        }());
    </script>
</body>
</html>