var injectParams = ['$scope'];

var setMenuController = function ($scope) {

    $scope.user;

};

setMenuController.$inject = injectParams;


app.register.controller('setMenuController', setMenuController);