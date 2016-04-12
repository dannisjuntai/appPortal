var injectParams = ['$scope', '$location', '$routeParams', '$window', 'groupFactory', 'breadcrumbService'];

var menuController = function ($scope, $location, $routeParams, $window, groupFactory, breadcrumbService) {
    $scope.breadcrumbs;

    $scope.menus = [];
    //導覽報表
    $scope.navMenu = function (o) {
        var oo = o;
        if (o && angular.isObject(o)) {
            if (o.url == '/reprot') {
                $window.open('http://10.92.198.66:8075/WebReport/ReportServer?op=fs_load&cmd=fs_signin&_=1458118273592', 'FABTool Report');
            } else {
                redirectToUrl($location, o.url);
            }
            
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
    //初始化
    function init() {
        breadcrumbService.clearBreadcrumbs('home');
        breadcrumbService.setBreadcrumb('home', {
            href: '/menu',
            label: '功能選單'
        });

        $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();

        $scope.menus = [
            { name: '設備監控', code: 'Monitor', url: '/dt0', icon: 'fa fa-desktop', color: 'quick-button metro blue' },
            { name: '管理報表', code: 'Report', url: '/reprot', icon: 'fa fa-line-chart', color: 'quick-button metro yellow' },
            { name: '系統設定', code: 'Setting', url: '/menu/set', icon: 'fa fa-wrench', color: 'quick-button metro pink' }
        ];
    }
    //設定連結
    function setBreadcrumb(obj) {
        breadcrumbService.setBreadcrumb('home', {
            href: "/et0/" + $scope.group.currentGroup.groupId,
            label: $scope.group.currentGroup.groupName
        });
        $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();
    };

};

menuController.$inject = injectParams;

app.register.controller('menuController', menuController);