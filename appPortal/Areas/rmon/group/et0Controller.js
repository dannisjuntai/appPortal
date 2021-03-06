﻿var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory', 'linkFactory', 'breadcrumbService'];

var et0Controller = function ($scope, $location, $routeParams, groupFactory, linkFactory, breadcrumbService) {
    $scope.breadcrumbs;
    $scope.linkTags = [];
    //歷史資料查詢參數
    $scope.param = {
        modal: false,
        sDateTime: new Date(),
        eDateTime: new Date(),
        optionNo: 0,
        groupId: 0,
        groupType: 2,
        itemsPerPage: 20,
        currentPage: 0
    };
    //控制畫面
    $scope.control = {
        pagedItems: [],
        selectedRow: -1,
        loading: false,
        showType: true
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
            //$scope.group = data;
            //setBreadcrumb(data);
            //$scope.maintain.modal = !$scope.maintain.modal;
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
                $scope.param.groupId = $scope.groupId;
                $scope.param.modal = !$scope.param.modal;
            }
        }
    };
    $scope.exitModal = function () {
        $scope.param.modal = !$scope.param.modal;
    };
    //搜尋資料
    $scope.searchEvents = function () {
        getEvents($scope.param);
        //變更顯示欄位
        if ($scope.param.optionNo == 0) {
            $scope.control.showType = true;
        } else {
            $scope.control.showType = false;
        }
    };

    //導覽到 圖表資料
    $scope.goChart = function (linkTagSeq) {
        redirectToUrl('/chart/' + linkTagSeq + '/' + $scope.groupId);
    };
    //上一頁
    $scope.prevPage = function () {
        if ($scope.param.currentPage > 0) {
            $scope.param.currentPage--;
            getEvents($scope.param);
        }
    };
    //下一頁
    $scope.nextPage = function () {
        if ($scope.param.currentPage < $scope.control.pagedItems.length - 1) {
            $scope.param.currentPage++;
            getEvents($scope.param);
        }
    };
    //設定目前頁面
    $scope.setPage = function (n) {
        $scope.param.currentPage = n;
        getEvents($scope.param);
    };
    //匯出excel 
    $scope.export = function () {
        $scope.control.loading = true;
        groupFactory.exportEvents($scope.param).then(processSuccess, processError);

        function processSuccess(data) {
            var d = data;
            var url = $location.protocol() + '://' + $location.host() + ':' + $location.port() + data.fullPath;
            saveToDisk(url, data.fileName);
            $scope.control.loading = false;
        }
        function processError(error) {
            $scope.control.loading = false;
        }
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

    //取得事件資料
    function getEvents(param) {
        $scope.control.loading = true;
        groupFactory.getEvents(param).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.events = data;
            if (param.currentPage > data.pagedItems) {
                param.currentPage = 0
            };
            $scope.control.pagedItems = getPagedItems(param.currentPage, data.pagedItems);
            $scope.control.loading = false;
        }
        function processError(error) {
            $scope.control.loading = false;
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