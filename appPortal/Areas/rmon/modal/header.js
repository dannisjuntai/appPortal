'use strict';

angular.module('app')
	.directive('header', function () {
	    return {
	        templateUrl: 'Areas/rmon/modal/header.html',
	        restrict: 'E',
	        replace: true,
	        link: function (scope) {
	            //scope.$watch("user", function () {

	            //});
	            var u = scope.user;

	        },
	        controller: 'headerController'
	    }
	});

app.controller('headerController', function ($scope) {
    $scope.user = $scope.user;
    //var user = authFactory.user;

    //µn¥X
    //$scope.logout = function () {
    //  //  authFactory.logout();
    //  //  authFactory.redirectToLogin();
    //}

    //$scope.$on('user', function (user) {
    //    var u = user;
    //});
});


