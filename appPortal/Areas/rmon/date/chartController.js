var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory', 'linkFactory', 'breadcrumbService'];

var chartController = function ($scope, $location, $routeParams, groupFactory, linkFactory, breadcrumbService) {
    $scope.breadcrumbs;

    //圖控設定資料
    $scope.link = { linkSubSeq: 0, linkTagSeq: 0 };

    $scope.link.linkTagSeq = ($routeParams.linkTagSeq) ? parseInt($routeParams.linkTagSeq) : 1;

    $scope.tag = {
        linkSubSeq: 0,
        linkTagSeq: 0,
        startDate: new Date(),
        startTime: new Date(),
        endDate: new Date(),
        endTime: new Date()
    };
    //
    $scope.getTagHistories = function () {
        getTagHistories();
    }
    //$scope.tags = [];

    $scope.dataset = [{ data: [] }];

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

    // selected fruits
    //$scope.selection = [];

    // helper method to get selected fruits
    //$scope.selectedFruits = function selectedFruits() {
    //    return filterFilter($scope.linkTags, { selected: true });
    //};

    // watch fruits for changes
    $scope.$watch('linkTags|filter:{selected:true}', function (nv) {
        $scope.selection = nv.map(function (linkTag) {
            //linkTag.linkTagSeq
            var linkTags = $scope.linkTags;
            return linkTag.tagName;
        });
    }, true);


    init();
    //初始化
    function init() {
        //找到 Device Id
        getLinkDevices();
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
            mode: "time", tickSize: [1, "minute"],
        },
        grid: {
            hoverable: true,
            clickable: true
        },
        tooltip: {
            show: true
        }
    };
    //取得歷史資訊
    function getTagHistories() {

        $scope.tag.linkTagSeq = $scope.link.linkTagSeq;
        $scope.tag.linkTags = $scope.linkTags;

        linkFactory.getHistoryTags($scope.tag).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.dataset = data;
            var i = 0;
            data.forEach(function (d) {
                var l = d.data;
                d.data.forEach(function (entry) {
                    $scope.dataset[i].data.push([entry.x, entry.y]);
                });

                i++;
            });
        }
        function processError(error) {
        }
    }
    //
    $scope.changLink = function (link) {
        getLinkTags(link.linkSubSeq);
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
    function getLinkDevices() {
        groupFactory.getLinkDevices().then(processSuccess, processError);
        function processSuccess(data) {
            //binding select
            $scope.linkDevices = data;
        }
        function processError(error) {
        }
    }
    //取得LinkTag 集合
    function getLinkTags(linkSubSeq) {
        groupFactory.getLinkTags(linkSubSeq).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.linkTags = '';
            $scope.linkTags = data;
        }
        function processError(error) {
        }
    };
    //取得LinkTag
    function getLinkTag(linkTagSeq) {
        groupFactory.getLinkTag(linkTagSeq).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.link.linkSubSeq = data.linkSubSeq;
            //$scope.linkTags = data;
            if ($scope.link.linkSubSeq > 0) {
                getLinkTags($scope.link.linkSubSeq);
            }

        }
        function processError(error) {
        }
    };
    //重新導向
    function redirectToUrl(path) {
        $location.replace();
        $location.path(path);
    }
};

chartController.$inject = injectParams;

app.register.controller('chartController', chartController);