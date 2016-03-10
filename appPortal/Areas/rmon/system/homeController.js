var injectParams = ['$scope'];

var homeController = function ($scope) {

    // $rootScope.title = "登入資料設定";
    $scope.user;

};

homeController.$inject = injectParams;


app.register.controller('mainController', homeController);