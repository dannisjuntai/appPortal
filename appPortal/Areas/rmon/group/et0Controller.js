var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory', 'breadcrumbService'];

var et0Controller = function ($scope, $location, $routeParams, groupFactory, breadcrumbService) {
    $scope.breadcrumbs;
    $scope.showModal = false;
    //Main Tool
    $scope.equipments = [];
    //初始化
    $scope.groupId = ($routeParams.groupId) ? parseInt($routeParams.groupId) : 1;
    $scope.group;

    //顯示Modal 
    $scope.toggleModal = function (o) {
        $scope.groupId = o.groupId;
        //getEvents(o.groupId);
    };
    //設定維護
    $scope.setLink = function (o) {
    //    $scope.maintain = 'on';
    }
    //導覽圖控
    $scope.navDevice = function (o) {
        var oo = o;
        if (o && angular.isObject(o)) {
            redirectToUrl('/rmonDAT/' + o.groupId);
        }
    };
    //設定 Breadcrumb
    $scope.setBreadcrumbs = function (b) {
        if (b && angular.isObject(b)) {
            var url = breadcrumbService.setBreadcrumbs(b);
            if (url != '') {
                //導覽
                redirectToUrl(url);
            }
        }
    };
    //設定維護狀態
    $scope.setMaintain = function (e) {
        if (e && angular.isObject(e)) {
            setMaintain(e.groupId);
        }
    }
    //顯示歷史資料 Modal 
    $scope.showHistory = function () {
        if ($scope.groupId != null) {
            getEvents($scope.groupId);
        }
    };

    //導覽到 圖表資料
    $scope.goChart = function (linkTagSeq) {
        redirectToUrl('/chart/' + linkTagSeq);
    };

    init();
    //初始化
    function init() {
        getGroup();
        getEquipments();
    }
    //設定連結
    function setBreadcrumb(obj) {
        breadcrumbService.setBreadcrumb('home', {
            href: "/et0/" + $scope.group.currentGroup.groupId,
            label: $scope.group.currentGroup.groupName
        });
        $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();
    };
    //取得部門資料
    function getGroup() {
        groupFactory.getGroup($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.group = data;
            setBreadcrumb(data);
        }
        function processError(error) {

        }
    }
    //取得機台集合
    function getEquipments() {
        groupFactory.getEquipments($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.equipments = data;
        }
        function processError(error) {

        }
    };
    //重新導向
    function redirectToUrl(path) {
        $location.replace();
        $location.path(path);
    }
    //取得告警資料
    function getTagAlarm() {
        groupFactory.getTagAlarm($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.tagAlarms = data;
        }
        function processError(error) {

        }
    }
    function setMaintain(groupId) {
        //
        groupFactory.setMaintain(groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.group = data;
            setBreadcrumb(data);
        }
        function processError(error) {

        }
    };
    //取得事件資料
    function getEvents(groupId) {

        groupFactory.getEvents(groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.showModal = !$scope.showModal;
            $scope.events = data;
        }
        function processError(error) {
        }
    };
    //執行資料更新
    var changLinkTag = setInterval(function () {
        //getEquipments();
        //getTagAlarm();
    }, 1000);

    $scope.$on("$routeChangeStart", function (next, current) {

        if (changLinkTag) {
            clearInterval(changLinkTag);
        }
    });
};

et0Controller.$inject = injectParams;

app.register.controller('et0Controller', et0Controller);