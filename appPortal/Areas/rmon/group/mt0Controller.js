var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory', 'linkFactory', 'breadcrumbService'];

var mt0Controller = function ($scope, $location, $routeParams, groupFactory, linkFactory, breadcrumbService) {
    //歷史資料查詢參數
    $scope.history = {
        modal: false,
        sDateTime: new Date(),
        eDateTime: new Date(),
        optionNo: 0,
        groupId: 0,
        groupType: 1
    };

    //選擇條件
    $scope.eventLevels = [];


    $scope.tagAlarms = [];
    //即時訊息
    $scope.linkTags = [];
    $scope.breadcrumbs;
    //Main Tool
    $scope.mainTools = [];
    //初始化
    $scope.groupId = ($routeParams.groupId) ? parseInt($routeParams.groupId) : 0;

    $scope.group;
    $scope.parentUrl;
    //歷史資料
    $scope.events = [];

    //導覽至Main Tool
    $scope.navEquipment = function (o) {
        if (o && angular.isObject(o)) {
            //
            redirectToUrl('/et0/' + o.groupId);
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
    //導覽到 圖表資料
    $scope.goChart = function (linkTagSeq) {
        redirectToUrl('/chart/' + linkTagSeq);
    };

    //顯示歷史資料 Modal 
    $scope.showHistory = function (index) {
        if (index == 1) {
            $scope.goChart(0);
        } else {
            if ($scope.groupId) {
                $scope.history.groupId = $scope.groupId;
                $scope.history.modal = !$scope.history.modal;
            }
        }
    };
    $scope.exitModal = function () {
        $scope.history.modal = !$scope.history.modal;
    };
    //搜尋資料
    $scope.searchEvents = function () {
        groupFactory.getEvents($scope.history).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.events = data;
        }
        function processError(error) {
        }
    };

    //初始化
    init();
    //初始化
    function init() {
        getGroup();
        getMainTools();
        //getTagAlarm();
        getEventLevels();

        //修正
        getMainToolLinkTags($scope.groupId);
    }
    //取得事件項目
    function getEventLevels() {
        groupFactory.getEventLevels().then(processSuccess, processError);

        function processSuccess(data) {
            $scope.eventLevels = data;
        }
        function processError(error) {

        }
    }
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
    //設定連結
    function setBreadcrumb(obj) {
        breadcrumbService.setBreadcrumb('home', {
            href: "/mt0/" + $scope.group.currentGroup.groupId,
            label: $scope.group.currentGroup.groupName
        });

        $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();
    };
    //取得機台集合
    function getMainTools() {
        groupFactory.getMainTools($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.mainTools = data;
        }
        function processError(error) {

        }
    };
    //取得告警資料
    function getTagAlarm() {
        groupFactory.getTagAlarm($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.tagAlarms = data;
        }
        function processError(error) {

        }
    }
    //重新導向
    function redirectToUrl(path) {
        $location.replace();
        $location.path(path);
    }
    //取得事件資料
    function getEvents(groupId) {

        groupFactory.getEvents(groupId).then(processSuccess, processError);

        function processSuccess(data) {
             $scope.events = data;
        }
        function processError(error) {
        }
    };
    //
    function getMainToolLinkTags(id) {
        linkFactory.getMainToolLinkTags(id).then(processSuccess, processError);
        function processSuccess(data) {
            $scope.linkTags = data.linkTags;
        }
        function processError(error) { }
    }

    //執行資料更新
    var changLinkTag = setInterval(function () {
        getMainTools();
        getMainToolLinkTags($scope.groupId);
    }, 2000);

    $scope.$on("$routeChangeStart", function (next, current) {

        if (changLinkTag) {
            clearInterval(changLinkTag);
        }
    });
};

mt0Controller.$inject = injectParams;

app.register.controller('mt0Controller', mt0Controller);