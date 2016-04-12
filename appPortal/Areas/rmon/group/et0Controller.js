var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory', 'linkFactory', 'breadcrumbService'];

var et0Controller = function ($scope, $location, $routeParams, groupFactory, linkFactory, breadcrumbService) {
    $scope.breadcrumbs;
    $scope.linkTags = [];
    //歷史資料查詢參數
    $scope.history = {
        modal: false,
        sDateTime: new Date(),
        eDateTime: new Date(),
        optionNo: 0,
        groupId: 0,
        groupType: 2
    };
    //維護
    $scope.maintain = { modal: false, groupId: 0, items: [], optionNo: 0, message: "" };


    //Main Tool
    $scope.equipments = [];
    //初始化
    $scope.groupId = ($routeParams.groupId) ? parseInt($routeParams.groupId) : 0;
    $scope.group;

    //顯示Modal 
    $scope.toggleModal = function (o) {
        $scope.groupId = o.groupId;
        getEvents(o.groupId);
    };
    //設定維護
    $scope.setLink = function (o) {
        //    $scope.maintain = 'on';
    }
    //導覽圖控
    $scope.navDevice = function (o) {
        var oo = o;
        if (o && angular.isObject(o)) {
            redirectToUrl($location, '/rmonDAT/' + o.groupId);
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
    //顯示維護 modal
    $scope.setMaintainModal = function (e) {
        if (e && angular.isObject(e)) {
            if (e.groupId) {
                $scope.maintain.groupId = e.groupId;
            }

            if (e.maintainCount > 0) {
                $scope.setMaintain();
                alert("取消保養設定");
            } else {
                $scope.maintain.modal = !$scope.maintain.modal;
            }

        }
    }
    //設定維護資料
    $scope.setMaintain = function () {
        //
        $scope.maintain.message = "";
        groupFactory.setMaintain($scope.maintain).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.maintain.message = "設定保養項目完成!";
        }
        function processError(error) {
            $scope.maintain.message = "設定保養項目失敗!";
        }
    };
    $scope.exitMaintainModal = function () {
        $scope.maintain.modal = !$scope.maintain.modal;
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

    //導覽到 圖表資料
    $scope.goChart = function (linkTagSeq) {
        redirectToUrl($location, '/chart/' + linkTagSeq);
    };

    init();
    //初始化
    function init() {
        getGroup();
        getEquipments();
        getMaintainItems();
        getEventLevels();
        //即時訊息
        getEquipmentLinkTags($scope.groupId);
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
    //取得事件項目
    function getMaintainItems() {
        groupFactory.getMaintainItems().then(processSuccess, processError);

        function processSuccess(data) {
            $scope.maintain.items = data;
        }
        function processError(error) {

        }
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

    //取得告警資料
    function getTagAlarm() {
        groupFactory.getTagAlarm($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.tagAlarms = data;
        }
        function processError(error) {

        }
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
    function getEquipmentLinkTags(id) {
        linkFactory.getEquipmentLinkTags(id).then(processSuccess, processError);
        function processSuccess(data) {
            $scope.linkTags = data.linkTags;
        }
        function processError(error) { }
    }
    //執行資料更新
    var changLinkTag = setInterval(function () {
        getEquipments();
        //getTagAlarm();
        getEquipmentLinkTags($scope.groupId);
    }, 2000);

    $scope.$on("$routeChangeStart", function (next, current) {

        if (changLinkTag) {
            clearInterval(changLinkTag);
        }
    });
};

et0Controller.$inject = injectParams;

app.register.controller('et0Controller', et0Controller);