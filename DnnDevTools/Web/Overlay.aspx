<%@ Page Language="C#" %>
<html>
<head>
    <title>DotNetNuke Developer Tools Mail Window</title>
    <style>
        body {
            margin: 0;
        }        

        [ng-cloak] {
            display: none !important;
        }

        /* FONTS */
        .dnn-mdt-copy {
            font-family: Arial, sans-serif;
            font-size: 16px;
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
            border-bottom: 1px solid #cdcdcd;
        }
    </style>
</head>
<body>
    
    <% Response.Write(System.Web.Helpers.AntiForgery.GetHtml()); %>

    <div ng-app="app">
        <div ng-controller="MailController as mail">
            <p ng-if="mail.list.length === 0" class="dnn-mdt-copy">[res:Loading]</p>
            <table ng-cloak ng-if="mail.list.length > 0 && !mail.currentMail" class="dnn-mdt-table">
                <thead>
                    <tr>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy">[res:Mails.Column.Id.Title]</th>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy">[res:Mails.Column.Sender.Title]</th>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy">[res:Mails.Column.SentOn.Title]</th>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy">Subject</th>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy">To</th>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy"></th>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy"></th>
                    </tr>
                </thead>
                <tbody>
                    <tr ng-repeat="item in mail.list">
                        <td class="dnn-mdt-tableCell dnn-mdt-copy">{{item.Id}}</td>
                        <td class="dnn-mdt-tableCell dnn-mdt-copy">{{item.Sender}}</td>
                        <td class="dnn-mdt-tableCell dnn-mdt-copy">{{item.SentOn}}</td>
                        <td class="dnn-mdt-tableCell dnn-mdt-copy">{{item.Subject}}</td>
                        <td class="dnn-mdt-tableCell dnn-mdt-copy">{{item.To}}</td>
                        <td class="dnn-mdt-tableCell dnn-mdt-copy"><button ng-click="mail.detail(item)" type="button">Detail</button></td>
                        <td class="dnn-mdt-tableCell dnn-mdt-copy"><button ng-click="mail.remove(item)" type="button">Delete</button></td>
                    </tr>
                </tbody>
            </table>
            <div ng-if="mail.currentMail" ng-cloak>
                <button type="button" ng-click="mail.currentMail = undefined">back</button>
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

                function detail(mail) {
                    var deferred = $q.defer();

                    $http({
                        method: 'GET',
                        url: 'api/mail/detail',
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

                vm.list = [];
                vm.detail = detail;
                vm.remove = remove;
                vm.currentMail = undefined;

                activate();

                function activate() {
                    remoteData.list().then(success, error);

                    function success(response) {
                        vm.list = response;
                    }

                    function error(response) {
                        // TODO handle error
                        console.log(response);
                    }
                }

                function detail(mail) {
                    remoteData.detail(mail).then(success, error);

                    function success(response) {
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
            }
        }());
    </script>
</body>
</html>