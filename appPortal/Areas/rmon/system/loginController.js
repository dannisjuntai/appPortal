var injectParams = ['$scope', '$window', '$location', 'appStoreFatory'];

var loginController = function ($scope, $window, $location, appStoreFatory) {

   // $rootScope.title = "登入資料設定";

    $scope.user;
    
    $scope.login = function () {
        appStoreFatory.login($scope.user).then(processSuccess, processError);

        function processSuccess(data) {
            var login = data;

            if (login == true) {
                //$window.location.replace("#/dt0");
                redirectToUrl("/dt0");
            }
        }

        function processError(error) {

        }
    };

    function redirectToUrl(path) {
        $location.replace();
        $location.path(path);
    }
};

loginController.$inject = injectParams;


app.register.controller('loginController', loginController);
