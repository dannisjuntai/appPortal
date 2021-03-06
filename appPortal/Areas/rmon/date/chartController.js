﻿var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory', 'linkFactory', 'breadcrumbService'];

var chartController = function ($scope, $location, $routeParams, groupFactory, linkFactory, breadcrumbService) {
    $scope.breadcrumbs;

    //圖控設定資料
    $scope.link = { linkSubSeq: 0, linkTagSeq: 0, parentId: 0 };

    $scope.link.linkTagSeq = ($routeParams.linkTagSeq) ? parseInt($routeParams.linkTagSeq) : 0;
    $scope.link.parentId = ($routeParams.parentId) ? parseInt($routeParams.parentId) : 0;
    //$scope.selection =[];
    var dt = new Date();

    var sDt = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), dt.getHours(), dt.getMinutes() - 60, 00);
    var eDt = new Date();
    $scope.param = {
        linkSubSeq: 0,
        linkTagSeq: 0,
        startDate: sDt,
        startTime: sDt,
        endDate: new Date(),
        endTime: eDt,
        itemsPerPage: 86400,
        currentPage: 0,
        selection: []
    };
    $scope.control = {
        autoSearch: 0,
        loading: false,
        deviceSelect: true
    };
    //取得歷史資訊
    $scope.getTagHistories = function () {
        getHistoryTags();
    }

    $scope.setTime = function (type) {
        //前
        if (type == -1) {
            var dt = $scope.param.startDate;
            $scope.param.startDate = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), dt.getHours(), dt.getMinutes() - 60, 00);
        }
        else {
            var dt = $scope.param.endDate;
            $scope.param.endDate = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), dt.getHours(), dt.getMinutes() + 60, 00);
        }
        getHistoryTags();
    }
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
        //找到 Device Id
        getLinkDevices($scope.link.parentId);
        //取得linkSub 資料 
        getLinkTag($scope.link.linkTagSeq);
        //設定 Banner
        setBreadcrumb(null);
    };

    $scope.dataset = [];

    $scope.options = {
        legend: { show: true, position: "nw", margin: [0, -30], noColumns: 0, },
        series: {
            lines: { show: true },
            points: { fillColor: "#0062FF", show: true }
        },
        xaxis: {
            mode: "time", tickSize: ["10", "minute"],
        },
        //"second", "minute", "hour", "day", "month" and "year"
        yaxes: [{
            //[第一個軸]
            //如果沒有要設定, 就留空白
        }, {
            //[第二個軸]
            position: "right"  //設定標籤是顯示在圖表的右方或是左方
        }
        ],
        grid: {
            hoverable: true,
            clickable: true
        },
        tooltip: {
            show: true
        }
    };


    //選擇Link Device 
    $scope.changLink = function (link) {
        //清除
        $scope.param.selection = [];

        getLinkTags(link.linkSubSeq, $scope.control.autoSearch);
    };

    //設定連結
    function setBreadcrumb(obj) {
        breadcrumbService.setBreadcrumb('home', {
            href: "/chart/" + $scope.link.linkTagSeq,
            label: "歷史紀錄"
        });

        $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();
    };
    //取得LinkDevice 資料
    function getLinkDevices(parentId) {
        groupFactory.getLinkDevices(parentId).then(processSuccess, processError);
        function processSuccess(data) {
            if (data.length <= 0) {
                $scope.control.deviceSelect = false;
            } else {
                //binding select
                $scope.linkDevices = data;
                $scope.control.deviceSelect = true;
            }
            

        }
        function processError(error) {
        }
    }
    //取得LinkTag 集合
    function getLinkTags(linkSubSeq, autoSearch) {
        groupFactory.getLinkTags(linkSubSeq).then(onSuccess, onError);
        function onSuccess(d) {
            $scope.linkTags = '';
            $scope.linkTags = d;
            //自動勾選
            $scope.linkTags.forEach(function (link) {
                if ($scope.link.linkTagSeq == link.linkTagSeq) {
                    link.selected = true;
                    $scope.param.selection.push(link.linkTagSeq);
                };
            });
            //取得歷史紀錄
            if (autoSearch > 0) {
                getHistoryTags();
            }
            $scope.control.autoSearch = 0;

        }
        function onError(e) {
        }
    };

    //取得LinkTag
    function getLinkTag(linkTagSeq) {
        groupFactory.getLinkTag(linkTagSeq).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.link.linkSubSeq = data.linkSubSeq;
            if ($scope.link.linkSubSeq > 0) {
                $scope.control.autoSearch = 1;
                getLinkTags($scope.link.linkSubSeq, $scope.control.autoSearch);
            }

        }
        function processError(error) {
        }
    };
    function getHistoryTags() {
    
        $scope.control.loading = true;
        $scope.param.linkTagSeq = $scope.link.linkTagSeq;
        $scope.param.linkTags = $scope.linkTags;

        linkFactory.getHistoryTags($scope.param).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.dataset = data.datasets;
            $scope.options.yaxes = data.yaxes;
            $scope.options.xaxis.tickSize = [data.xaxis.tickSize.key, data.xaxis.tickSize.value];
            var i = 0;
            data.datasets.forEach(function (d) {
                var l = d.data;
                d.data.forEach(function (entry) {
                    $scope.dataset[i].data.push([entry.x, entry.y]);
                });
                i++;
            });
            var y = $scope.options.yaxes;
            $scope.control.loading = false;
        }
        function processError(error) {
            $scope.control.loading = false;
        }
    };
};

chartController.$inject = injectParams;

app.register.controller('chartController', chartController);