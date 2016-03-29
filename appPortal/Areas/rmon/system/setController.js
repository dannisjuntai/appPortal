var injectParams = ['$scope', '$location', 'breadcrumbService'];

var setController = function ($scope, $location, breadcrumbService) {
    $scope.breadcrumbs;

    $scope.menus = [];
    //導覽報表
    $scope.navMenu = function (o) {
        var oo = o;
        if (o && angular.isObject(o)) {
            redirectToUrl($location, o.url);
        }
    };
    //設定 Breadcrumb
    $scope.setBreadcrumbs = function (b) {
        if (b && angular.isObject(b)) {
            var url = breadcrumbService.setBreadcrumbs(b);
            if (url != '') {
                //導覽
                redirectToUrl($location, url);
            }
        }
    };

    init();

    function init() {
        breadcrumbService.setBreadcrumb('home', {
            href: '/menu/set',
            label: '系統設定'
        });

        $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();

        $scope.menus = [
            { name: '參數設定', code: 'Option', url: '/menu/option', icon: 'fa fa-code', color: 'quick-button metro blue' },
            { name: '底圖設定', code: 'Setting', url: '/rmonSETMap', icon: 'fa fa-hand-paper-o', color: 'quick-button metro pink' }
        ];
    };
};

setController.$inject = injectParams;


app.register.controller('setController', setController);