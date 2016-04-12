var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory', '$rootScope', 'breadcrumbService'];

var dt1Controller = function ($scope, $location, $routeParams, groupFactory, $rootScope, breadcrumbService) {
    $scope.breadcrumbs;
    //部門
    $scope.departments = [];
    //初始化
    $scope.groupId = ($routeParams.groupId) ? parseInt($routeParams.groupId) : 1;
    //$scope.dt = $rootScope.dt0[0];
    $scope.group;
    //導覽至Main Tool
    $scope.navMainTool = function (o) {
        if (o && angular.isObject(o)) {
            redirectToUrl($location, '/mt0/' + o.groupId);
        }
    };
    //設定 Breadcrumb
    $scope.setBreadcrumbs = function (b) {
        if (b && angular.isObject(b)) {
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
        getGroup();
        getDepartment();
    }
    //取得部門資料
    function getGroup() {
        groupFactory.getGroup($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.group = data;

            breadcrumbService.setBreadcrumb('home', {
                href: "/dt1/" + $scope.group.currentGroup.groupId,
                label: $scope.group.currentGroup.groupName
            });
            //breadcrumbService.setBreadcrumb($scope.group.currentGroup.groupName, "/dt1/" + $scope.group.currentGroup.groupId);
            $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();
        }
        function processError(error) {

        }
    }
    //取得部門集合
    function getDepartment() {
        groupFactory.getDepartment($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.departments = data;
        }
        function processError(error) {

        }
    };

};

dt1Controller.$inject = injectParams;

app.register.controller('dt1Controller', dt1Controller);