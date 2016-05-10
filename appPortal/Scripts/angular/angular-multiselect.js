var mod = angular.module('ngMultiselect', []);

mod.directive('dropdownMultiselect', function () {
    return {
        restrict: 'E',
        scope: {
            model: '=',
            options: '=',
        },
        //				
        template:
                "<div class='btn-group' data-ng-class='{open: open}'>" +
                    "<button class='btn btn-small'>Select...</button>" +
                    "<button class='btn btn-small dropdown-toggle' data-ng-click='openDropdown()'><span class='caret'></span></button>" +
                    "<ul class='dropdown-menu pre-scrollable' aria-labelledby='dropdownMenu'>" +
                       // "<li><a data-ng-click='selectAll()'><span class='glyphicon glyphicon-ok green' aria-hidden='true'></span> Check All</a></li>" +
                        "<li><input type='checkbox' data-ng-click='selectAll($event)' style='margin-left:20px'/>Check All</li>" +
                        //"<li><a data-ng-click='deselectAll();'><span class='glyphicon glyphicon-remove red' aria-hidden='true'></span> Uncheck All</a></li>" +
                        "<li class='divider'></li>" +
                        "<li data-ng-repeat='option in options'>" +
                        "<a data-ng-click='toggleSelectItem(option)'><span data-ng-class='getClassName(option)' aria-hidden='true'></span> {{option.tagName}}</a></li>" +
                    "</ul>" +
                "</div>",

        controller: function ($scope) {
            $scope.openDropdown = function () {
                $scope.open = !$scope.open;
            };

            $scope.selectAll = function (e) {
                 if (e.target.checked == true) {
                    $scope.model = [];
                    angular.forEach($scope.options, function (item, index) {
                        $scope.model.push(item.linkTagSeq);
                    });
                } else {
                    $scope.model = [];
      
                }
                    

            };

            $scope.deselectAll = function () {
                $scope.model = [];
            };

            $scope.toggleSelectItem = function (option) {
                var intIndex = -1;

                angular.forEach($scope.model, function (item, index) {
                    if (item == option.linkTagSeq) {
                        intIndex = index;
                    }
                });

                if (intIndex >= 0) {
                    $scope.model.splice(intIndex, 1);
                }
                else {
                    $scope.model.push(option.linkTagSeq);

                }

            };

            $scope.getClassName = function (option) {

                var varClassName = 'glyphicon glyphicon-remove red';

                angular.forEach($scope.model, function (item, index) {

                    if (item == option.linkTagSeq) {

                        varClassName = 'glyphicon glyphicon-ok green';

                    }

                });

                return (varClassName);

            };

        }
    }

});