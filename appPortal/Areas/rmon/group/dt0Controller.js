var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory', '$rootScope', 'breadcrumbService'];

var dt0Controller = function ($scope, $location, $routeParams, groupFactory, $rootScope, breadcrumbService) {

    $scope.breadcrumbs;
    //部門集合
    $scope.departments = [];
    //初始化
    $scope.groupId = ($routeParams.groupId) ? parseInt($routeParams.groupId) : 1;

    //導覽至部門資料
    $scope.navDepartment = function (o) {
        if (o && angular.isObject(o)) {
            redirectToUrl($location, '/dt1/' + o.groupId);
        }
    };
    //設定 Breadcrumb
    $scope.setBreadcrumbs = function (b) {
        if (b && angular.isObject(b)) {
            //移除 Breadcrumb
            var url = breadcrumbService.setBreadcrumbs(b);
            if (url == '') {
                //nav
                console.log("last");
            } else {
                //導覽
                redirectToUrl($location, url);
            }
        }
    };
    init();

    //初始化
    function init() {
        // breadcrumbService.setBreadcrumb("首頁", "#/dt0/");
        //breadcrumbService.clearBreadcrumbs('home');
        breadcrumbService.setBreadcrumb('home', {
            href: '/dt0/',
            label: '首頁'
        });

        $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();
        getDepartment();
    }

    //取得部門資料
    function getDepartment() {
        groupFactory.getDepartment($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.departments = data;
        }
        function processError(error) {

        }
    };

};

dt0Controller.$inject = injectParams;

app.register.controller('dt0Controller', dt0Controller);