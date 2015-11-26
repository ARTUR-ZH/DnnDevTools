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
    <div ng-app="app">
        <div ng-controller="MailController as mail">
            <p ng-if="mail.list.length === 0" class="dnn-mdt-copy">Loading&hellip;</p>
            <table ng-cloak ng-if="mail.list.length > 0" class="dnn-mdt-table">
                <thead>
                    <tr>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy">Id</th>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy">Sender</th>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy">SentOn</th>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy">Subject</th>
                        <th class="dnn-mdt-tableCell dnn-mdt-copy">To</th>
                    </tr>
                </thead>
                <tbody>
                    <tr ng-repeat="item in mail.list">
                        <td class="dnn-mdt-tableCell dnn-mdt-copy">{{item.Id}}</td>
                        <td class="dnn-mdt-tableCell dnn-mdt-copy">{{item.Sender}}</td>
                        <td class="dnn-mdt-tableCell dnn-mdt-copy">{{item.SentOn}}</td>
                        <td class="dnn-mdt-tableCell dnn-mdt-copy">{{item.Subject}}</td>
                        <td class="dnn-mdt-tableCell dnn-mdt-copy">{{item.To}}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    
    <script src="Scripts/angular.js"></script>

    <script>
        (function () {
            angular.module('app', [])
                .factory('remoteData', remoteData)
                .controller('MailController', MailController);

            function remoteData($http, $q) {
                return {
                    getMailList: getMailList
                }

                function getMailList() {
                    var deferred = $q.defer();

                    $http({
                        method: 'GET',
                        url: 'API/Mail/List'
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

                activate();

                function activate() {
                    remoteData.getMailList().then(success, error);
                }

                function success(response) {
                    vm.list = response;
                }

                function error(response) {
                    // TODO handle error
                    console.log(response);
                }
            }
        }());
    </script>
</body>
</html>