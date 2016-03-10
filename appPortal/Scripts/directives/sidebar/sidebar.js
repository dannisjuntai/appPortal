'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */

angular.module('app')
  .directive('sidebar', ['$location', function () {
      return {
          templateUrl: 'Scripts/directives/sidebar/sidebar.html',
          restrict: 'E',
          replace: true,
          controller: function ($scope) {
              //$scope.selectedMenu = 'dashboard';
              $scope.collapseVar = 0;
              $scope.menus = getSideBarMenu();

              $scope.check = function (index) {
                  
                  if (index == $scope.collapseVar)
                      $scope.collapseVar = 0;
                  else
                      $scope.collapseVar = index;
                  console.log($scope.collapseVar);
              };


          }
      }
  }]);
