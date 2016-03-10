'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */
angular.module('app')
	.directive('headerNotification',function(){
		return {
        templateUrl:'Scripts/directives/header/header-notification.html',
        restrict: 'E',
        replace: true,
    	}
	});


