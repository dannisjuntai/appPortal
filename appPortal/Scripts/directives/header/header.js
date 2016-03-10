'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */
angular.module('app')
	.directive('header', function () {
	    return {
	        templateUrl: 'Scripts/directives/header/header.html',
	        restrict: 'E',
	        replace: true,
	        controller: function ($scope) {
	            $scope.user;
	        }
	    }
	});


