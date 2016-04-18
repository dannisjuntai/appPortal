var injectParams = ['$scope', '$location', '$routeParams', '$timeout', '$http', '$queueFactory', 'linkFactory', 'groupFactory', '$rootScope', 'breadcrumbService'];

var rmonDATController = function ($scope, $location, $routeParams, $timeout, $http, $queueFactory, linkFactory, groupFactory, $rootScope, breadcrumbService) {
    //$scope.breadcrumbs;
    $scope.tagAlarms = [];
    $scope.linkTags = [];
    $scope.rootGroupId = $rootScope.groupId;
    $scope.groupId = ($routeParams.groupId) ? parseInt($routeParams.groupId) : 0;
    $scope.locationId = 0;

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
    $scope.goChart = function () {
        redirectToUrl('/chart/' + 0);
    };

    canvas = new fabric.Canvas(document.getElementById("playCanvas"));
    $scope.canvas = canvas;
    var image = new Image();

    image.onload = function () {
        var imgbase64 = new fabric.Image(image, {
            scaleX: 1,
            scaleY: 1,
            hasControls: false,
            hasBorders: false,
            evented: false
        })
        //設定背景圖顏色
        canvas.setBackgroundImage(imgbase64, canvas.renderAll.bind(canvas));
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

    function getDeviceLinkTags() {
        linkFactory.getDeviceLinkTags($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.linkTags = data.linkTags;
        }
        function processError(error) {

        }
    }

    function getData() {
        queue.enqueue(function () {
            getGroupLocations();
            //getTagAlarm();
            getDeviceLinkTags();
        });
        queue.enqueue(function (data) {
            // all tasks finished
            var dt = new Date();
            console.log('工作順利完成！' + dt);
        });
    }

    var queue = $queueFactory(0);

    var dataPoint = setInterval(function () { getData() }, 1000);

    $scope.$on("$routeChangeStart", function (next, current) {
        console.log('$routeChangeStart');
        //取消 timer
        if (dataPoint) {
            clearInterval(dataPoint);
        }
    });

    showChart($scope);
    //初始化
    function init() {
        getGroup();
        //取得圖檔
        getGroupImages($scope.groupId);
        //取得圖控資訊
        getGroupLocations();
        getEventLevels();

    };
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
            href: "/rmonDAT/" + $scope.group.currentGroup.groupId,
            label: $scope.group.currentGroup.groupName
        });
        $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();
    };
    //從資料庫 取得Background 圖檔
    function getGroupImages(id) {
        groupFactory.getGroupImages(id).then(processSuccess, processError);

        function processSuccess(data) {
            // binding 資料
            image.src = "data:image/png;base64," + data.base64;
        }
        function processError(error) {

        }
    };
    //取得圖控資訊
    function getGroupLocations() {
        groupFactory.getGroupLocations($scope.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            // binding 資料
            //image.src = "data:image/png;base64," + data.base64;
            $scope.links = data;
            DrawLinks();
        }
        function processError(error) {

        }
    };
    //繪製圖控資訊
    function DrawLinks() {
        //移除繪圖元件
        canvas.forEachObject(function (obj) {
            canvas.remove(obj);
        });
        //新增繪圖元件
        $scope.links.forEach(function (link) {
            //addDeviceLink(link);
            drawDevice(link);
        });
    };

    $scope.update = function () {

        canvas.forEachObject(function (obj) {
            var type = obj.type;
            //群組
            var c = obj._objects[0];
            setActiveStyle("fill", "#C13E3E", c);
        });

        function setActiveStyle(styleName, value, object) {
            object = object || canvas.getActiveObject();
            if (!object) {
                return;
            }
            if (object.setSelectionStyles && object.isEditing) {
                var style = {};
                style[styleName] = value;
                object.setSelectionStyles(style);
                object.setCoords();
            }
            else {
                object[styleName] = value;
            }

            object.setCoords();
            canvas.renderAll();
        };
    };
    //加入圖控元件
    function addDeviceLink(link) {

        var obj = JSON.parse(link.locationValue);

        var rect = new fabric.Rect({
            type: 'element_' + link.locationId, //識別碼
            fill: '#FFFF00',
            width: 150,
            height: 40,
            opacity: 1,
            stroke: '#909ab2',
            strokeWidth: 2,
            borderColor: 'red',
            cornerColor: 'green',
            hasRotatingPoint: false,
            transparentCorners: false,
            cornerSize: 6
        });
        var circle = new fabric.Circle({
            //id: link.locationId,
            type: 'element_' + link.locationId, //識別碼
            //left: obj.x,
            //top: obj.y,
            fill: '#89deae',
            radius: 12,
            opacity: 1,
            stroke: '#909ab2',
            strokeWidth: 2,
            borderColor: 'red',
            cornerColor: 'green',
            hasRotatingPoint: false,
            transparentCorners: false,
            cornerSize: 6
        });


        //新增圖文
        var type = 'prompt_' + link.locationId;
        var text = link.tagName + '\n' + link.curfValue;
        var left = rect.left + 10;//circle.left + (circle.width * circle.scaleX) + 10;
        var top = rect.top; //circle.top;

        var text = new fabric.Text(text, {
            type: type,
            left: left,
            top: top,
            fontSize: 16,
            fill: '#000000',
            originX: 'left',
            hasRotatingPoint: true,
            centerTransform: true,
            selectable: false
        });

        var group = new fabric.Group([rect, text], {
            type: 'group_' + link.locationId,
            left: obj.x,
            top: obj.y,
            //evented: false,
            selectable: false,
            hasControls: false,
            hasBorders: false
        });
        canvas.add(group);
    };
    //繪製圖控
    function drawDevice(link) {
        var obj = JSON.parse(link.locationValue);

        var rect = new fabric.Rect({
            type: 'element_' + link.locationId, //識別碼
            fill: "rgba(0, 0, 0, 0)",
            width: 150,
            height: 20,
            opacity: 1,
            hasRotatingPoint: false,
            transparentCorners: false
        });
        //判斷簡稱
        var name;

        if (link.shortName) {
            //do something
            name = link.shortName;
        } else {
            name = link.tagName;
        }

        //新增圖文
        var type = 'prompt_' + link.locationId;
        var text = name;
        var left = rect.left + 10;
        var top = rect.top + 5;

        var text = new fabric.Text(text, {
            type: type,
            left: left,
            top: top,
            fontSize: 16,
            fill: '#000000',

            originX: 'left',
            textBackgroundColor: '#ffc40d',
            hasRotatingPoint: true,
            centerTransform: true,
            selectable: false,
           
        });

        var value = ' ' + link.curfValue + ' ' + link.unitName;
        var value = new fabric.Text(value, {
            type: type,
            left: text.width + 20,
            top: top,
            fontSize: 16,
            fill: '#f2f2f2',
            originX: 'left',
            textBackgroundColor: link.color,
            hasRotatingPoint: true,
            centerTransform: true,
            selectable: false
        });

        var group = new fabric.Group([rect, text, value], {
            type: 'group_' + link.locationId,
            left: obj.x,
            top: obj.y,

            selectable: false,
            hasControls: false,
            hasBorders: false
        });
        canvas.add(group);
    };

    init();

    $scope.chartdata = {
        labels: [],
        datasets: [
            {
                label: "GroupId",
                fillColor: "transparent",
                strokeColor: "red",
                pointColor: "red",
                pointStrokeColor: "#000",
                pointHighlightFill: "#000",
                pointHighlightStroke: "rgba(220,220,220,1)",
                data: []
            }
        ]
    };

    $scope.showModal = false;
    //$scope.showEventModal = false;

    //歷史資料查詢參數
    $scope.param = {
        modal: false,
        sDateTime: new Date(),
        eDateTime: new Date(),
        optionNo: 0,
        groupId: 0,
        itemsPerPage: 20,
        currentPage: 0
    };
    //控制畫面
    $scope.control = {
        pagedItems: [],
        selectedRow: -1
    };
    $scope.toggleModal = function () {
        $scope.showModal = !$scope.showModal;
    };

    //顯示歷史資料 Modal 
    $scope.showHistory = function (index) {
        if (index == 1) {
            $scope.goChart(0);
        } else {
            if ($scope.groupId) {
                $scope.param.groupId = $scope.groupId;
                $scope.param.modal = !$scope.param.modal;

            };
        }
    };
    $scope.exitModal = function () {
        $scope.param.modal = !$scope.param.modal;
    };
    //搜尋資料
    $scope.searchEvents = function () {
        getEvents($scope.param);
    };

    //$scope.goBack = function () {
    //    redirectToUrl('/equipment/' + $rootScope.groupId);
    //};
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
    //重新導向
    function redirectToUrl(path) {
        $location.replace();
        $location.path(path);
    }
    //取得事件資料
    function getEvents(param) {

        groupFactory.getEvents(param).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.events = data;
        }
        function processError(error) {
        }
    };
    //取得歷史資訊
    $scope.getTagHistories = function (groupId, locationId) {

        groupFactory.getTagHistories(groupId, locationId, 1).then(processSuccess, processError);

        function processSuccess(data) {

            var i = 0;
            //清空
            $scope.dataset.forEach(function (d) {
                d.data = [];
                d.label = '';
            });

            data.data.forEach(function (d) {

                d.list.forEach(function (entry) {
                    $scope.dataset[i].data.push([entry.labels, entry.data]);

                });
                $scope.dataset[i].label = d.label;
                i++;
            });


            $scope.toggleModal();
        }

        function processError(error) {
            return null;
        }
    }

    $scope.myChart = {
        "data": $scope.chartdata, "options": {
            scaleShowGridLines: true,
            scaleShowHorizontalLines: true
        }
    };

    $scope.dataset = [{ data: [] }, { data: [] }, { data: [] }];

    $scope.options = {
        legend: { show: true },
        series: {
            lines: {
                show: true
            },
            points: {
                show: true
            }
        },
        grid: {
            hoverable: true,
            clickable: true
        },
        xaxis: { mode: "time" },
        tooltip: { show: true }
    };
};

rmonDATController.$inject = injectParams;

app.register.controller('rmonDATController', rmonDATController);

function showChart($scope) {

    function updateScope(option) {
        $scope.$$phase || $scope.$digest();
        $scope.canvas.renderAll();
    }

    function mouseUp(options) {
        var p = $scope.canvas.getPointer(options.e);
        if (typeof options.target == 'undefined') {
            return;
        }

        var device = options.target;
        var token = device.type.split("_");
        var lid = token[1];
        $scope.locationId = lid;
        $scope.getTagHistories($scope.groupId, lid);

        console.log('mouse over');
    };

    function mouseOver(options) {
        var p = $scope.canvas.getPointer(options.e);
        console.log('mouse over');
    };
    $scope.canvas
        .on('object:selected', updateScope)
        .on('group:selected', updateScope)
        .on('path:created', updateScope)
        .on('selection:cleared', updateScope)
        .on('mouse:up', mouseUp)
        .on('mouse:over', mouseOver);

};