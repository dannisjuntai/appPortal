var app = angular.module("app", [
    'ngRoute',
    'routeResolverServices',
    'ngResource',
    'wc.directives',
    'angularTreeview',
    'naif.base64',
    'ngQueue',
    'angular-flot',
    'ngModal',
    'ng-timepicker',
    'mgcrea.ngStrap']);
var canvas;

app.config(['$routeProvider',
    'routeResolverProvider',
    '$controllerProvider',
    '$compileProvider',
    '$filterProvider',
    '$provide',
    '$httpProvider',
    'ngModalDefaultsProvider',
    '$datepickerProvider',
    '$timepickerProvider',
function ($routeProvider,
    routeResolverProvider,
    $controllerProvider,
    $compileProvider,
    $filterProvider,
    $provide,
    $httpProvider,
    ngModalDefaultsProvider,
    $datepickerProvider,
    $timepickerProvider) {

    //日期元件設定
    angular.extend($datepickerProvider.defaults, {
        dateFormat: 'yyyy/MM/dd',
        startWeek: 1
    });

    angular.extend($timepickerProvider.defaults, {
        timeFormat: 'HH:mm',
        length: 7
    });


    //Change default views and controllers directory using the following:
    routeResolverProvider.routeConfig.setBaseDirectories('/Areas/rmon/', '/Areas/rmon/');
    ngModalDefaultsProvider.set('closeButtonHtml', "<i class='fa fa-times'></i>");
    
    app.register =
    {
        controller: $controllerProvider.register,
        directive: $compileProvider.directive,
        filter: $filterProvider.register,
        factory: $provide.factory,
        service: $provide.service
    };

    //Define routes - controllers will be loaded dynamically
    var route = routeResolverProvider.route;


    var routes = [
        //{ url: '/chart', baseName: 'chart', path: 'date/' },
        { url: '/login', baseName: 'login', path: 'system/' },
        { url: '/menu', baseName: 'menu', path: 'group/' },
        { url: '/menu/set', baseName: 'set', path: 'system/' },
        { url: '/menu/option', baseName: 'option', path: 'system/' },
        { url: '/dt0', baseName: 'dt0', path: 'group/' },
        { url: '/dt1/:groupId', baseName: 'dt1', path: 'group/' },
        { url: '/mt0/:groupId', baseName: 'mt0', path: 'group/' },
        { url: '/et0/:groupId', baseName: 'et0', path: 'group/' },
        { url: '/chart/:linkTagSeq', baseName: 'chart', path: 'date/' },
        
        { url: '/department', baseName: 'department', path: 'group/' },
        { url: '/mainTool', baseName: 'mainTool', path: 'group/' },
        { url: '/equipment/:groupId', baseName: 'equipment', path: 'group/' },
        { url: '/rmonDAT/:groupId', baseName: 'rmonDAT', path: 'date/' },
        { url: '/rmonDATMap', baseName: 'rmonDATMap', path: 'date/' },
        { url: '/rmonSET/:groupId', baseName: 'rmonSET', path: 'set/' },
        { url: '/rmonSETMap', baseName: 'rmonSETMap', path: 'set/' },
       
        { url: '/home', baseName: 'home', path: 'system/' },
    ];

    angular.forEach(routes, function (templet) {
        $routeProvider.when(templet.url, route.resolve(templet.baseName, templet.path, 'vm'));
    });
    $routeProvider.otherwise({ redirectTo: '/' + routes[0].baseName });
}]);

app.controller("mainController", ['$scope', '$rootScope', 'appStoreFatory', mainController]);

/* 
main Controller
*/
function mainController($scope, $rootScope, appStoreFatory) {
    var vm = this,
        appTitle = '監控';
  
    $scope.getTitle = function () {
        return $rootScope.title;
    };
    $scope.menus = {};

    function init() {
        getMenu();
    }
    $scope.collapseVar = 0;
    $scope.check = function (index) {

        if (index == $scope.collapseVar){
            //$scope.collapseVar = 0;
        }
        else {
            $scope.collapseVar = index;
        }
            
        console.log($scope.collapseVar);
    };

    function getMenu() {
        $scope.menus = appStoreFatory.getCustomer(1);



    }

    $scope.target = '';

    //點選側邊 實作點選
    $scope.sidebarMenu = function (event) {
        $scope.target = event.target;
        event.stopPropagation();
    };

    $scope.menuOpened = function (attr) {
        var current = attr.m;
        var target = $scope.target;
        if (typeof target == 'undefined' && target == "") {
            return false;
        }

        if (current.parentApp.parentAppNo == 0) {
            var parrentAppNo = Math.floor(target.id / 1000) * 1000;

            if (target.id == current.parentApp.appNo || parrentAppNo == current.parentApp.appNo) {
                return true;
            }
            else {
                return false;
            }
        }
    };
    //選單縮放
    //$scope.toggleMenu = function (event) {

    //    $("#wrapper").toggleClass("toggled");
    //    event.preventDefault();
    //}

    $scope.state = false;
    //http://plnkr.co/edit/xzcjStdvmkI2rpfMzLjI?p=preview
    $scope.toggleState = function (id) {

        $scope.state = !$scope.state;
    };

    init();
}
//

function appRun($rootScope, $location) {

    //Client-side security. Server-side framework MUST add it's 
    //own security as well since client-based security is easily hacked
    $rootScope.$on("$routeChangeStart", function (event, next, current) {
        if (next && next.$$route && next.$$route.secure) {
            //if (!authService.user.isAuthenticated) {
            //    $rootScope.$evalAsync(function () {
            //        authService.redirectToLogin();
            //    });
            //}
        }
    });
    
    function redirectToUrl(path) {
        $location.replace();
        $location.path(path);
    }
    


};

app.run(['$rootScope', '$location', appRun]);

app.directive('sidebarDirective', function () {
    return {
        link: function (scope, element, attr) {
            scope.$watch(attr.sidebarDirective, function (newVal) {
                if (newVal) {
                    var e = element[0];
                    element.addClass('show');
                    return;
                }
                element.removeClass('show');
            });
        }
    }
});

app.directive('modal', function () {
    return {
        template: '<div class="modal fade">' +
            '<div class="modal-dialog" style="width:800px">' +
              '<div class="modal-content">' +
                '<div class="modal-header">' +
                  //'<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
                  '<h4 class="modal-title">{{ title }}</h4>' +
                '</div>' +
                '<div class="modal-body" ng-transclude></div>' +
              '</div>' +
            '</div>' +
          '</div>',
        restrict: 'E',
        transclude: true,
        replace: true,
        scope: true,
        link: function postLink(scope, element, attrs) {
            scope.title = attrs.title;
            attrs.value = '';
            scope.$watch(attrs.visible, function (value) {
                if (value == true)
                    $(element).modal('show');
                else
                    $(element).modal('hide');
            });

            $(element).on('shown.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = true;
                });
            });

            $(element).on('hidden.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = false;
                });
            });
        }
    };
});
//
app.directive('datepicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {
                element.datepicker({
                    //dateFormat: 'yy/mm/dd',
                    format: "yyyy/mm/dd",
                    autoclose: true,
                    todayBtn: "linked",
                    onSelect: function (date) {
                        ngModelCtrl.$setViewValue(date);
                        scope.$apply();
                    }
                });
            });
        }
    }
});

//共用函式
function redirectToUrl($location, path) {
    $location.replace();
    $location.path(path);
}