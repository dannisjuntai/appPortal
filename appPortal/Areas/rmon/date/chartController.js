var injectParams = ['$scope', '$location', '$routeParams', 'groupFactory', 'breadcrumbService'];

var chartController = function ($scope, $location, $routeParams, groupFactory, breadcrumbService) {
    $scope.breadcrumbs;

    //圖控設定資料
    $scope.link = { linkSubSeq: 0, linkTagSeq: 0 };
    
    $scope.link.linkTagSeq = ($routeParams.linkTagSeq) ? parseInt($routeParams.linkTagSeq) : 1;

    Date.prototype.yyyymmdd = function () {

        var yyyy = this.getFullYear().toString();
        var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based         
        var dd = this.getDate().toString();

        return yyyy + '-' + (mm[1] ? mm : "0" + mm[0]) + '-' + (dd[1] ? dd : "0" + dd[0]);
    };
    $scope.now = new Date().yyyymmdd();

    var d = new Date();
    var endTime = d.getTime() + 600000;

    
    var now = new Date();
    var copy = now.getHours();
  

    $scope.tag = { linkTagSeq: 0, startDate: new Date(), startTime: new Date(), endDate: new Date(), endTime: new Date() };
    $scope.getTagHistories = function () {
        getTagHistories(1038, 1067);
    }
    $scope.tags = [];

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

    $scope.dataset = [{ label: "Tag", data: [] }, { label: "Tag", data: [], yaxis: 2 }, { label: "Tag", data: [], yaxis: 3 }];
    $scope.options = {
        legend: { show: true },
        series: {
            lines: {
                show: true,
            },
            points: {
               fillColor: "#0062FF", show: true
            }
        },
        xaxis: {
            mode: "time",
            //tickFormatter: function (val, axis) {
            //    return val;
            //},
            //tickSize: [1, "minute"],
            rotateTicks: 135
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
    function getTagHistories(groupId, locationId) {

        $scope.tag.linkTagSeq = $scope.link.linkTagSeq;
        $scope.tag.linkTags = $scope.linkTags;

        groupFactory.getHistoryTags($scope.tag).then(processSuccess, processError);
        //groupFactory.getTagHistories(groupId, locationId, 1).then(processSuccess, processError);
        //http://www.pikemere.co.uk/blog/flot-how-to-create-line-graphs/
        function processSuccess(data) {
            $scope.tags = data
            //$scope.dataset[0].data = [];
            
            $scope.dataset.forEach(function (d) {
                //var d = data;
                d.data = [];
            });
            var i = 0;
            var ticks = [];
            var content = [];
            //ticks
            data[0].list.forEach(function (entry) {
                var h = new Date(entry.labels).getHours();
                var m = new Date(entry.labels).getMinutes();
                var s = new Date(entry.labels).getSeconds();

                ticks.push([entry.labels, h + ":" + m + ":" + s]);
            });
            //$scope.options.xaxis.min = ticks[0];
            //$scope.options.xaxis.map = ticks[ticks.lastIndexOf];
            $scope.options.xaxis.ticks = [];
            $scope.options.xaxis.ticks = ticks;
            data.forEach(function (d) {
                var l = d.list;
                d.list.forEach(function (entry) {
                    $scope.dataset[i].data.push([entry.labels, entry.data]);
                });
                i++;
            });
            var d = $scope.dataset;
            //data[0].list.forEach(function (entry) {
            //    var h = new Date(entry.labels).getHours();
            //    var m = new Date(entry.labels).getMinutes();
            //    var s = new Date(entry.labels).getSeconds();

            //    $scope.dataset[0].data.push([entry.labels, entry.data]);
            //    ticks.push([entry.labels, h + ":" + m + ":" + s]);
            //});
            
            //data[1].list.forEach(function (entry) {
            //    $scope.dataset[1].data.push([entry.labels, entry.data]);
            //});

            //data.forEach(function (entry) {
                
            //    var h = new Date(entry.labels).getHours();
            //    var m = new Date(entry.labels).getMinutes();
            //    var s = new Date(entry.labels).getSeconds();
                
            //    $scope.dataset[0].data.push([entry.labels, entry.data]);

            //    $scope.dataset[1].data.push([entry.labels-100000, entry.data/10]);

            //    ticks.push([entry.labels, h + ":" + m + ":" + s]);
            //    i++;
            //});

        }
        $scope.item = 0;

        function processError(error) {
            return null;
        }
    }

    $scope.changLink = function (link) {
        getLinkTags(link.linkSubSeq);
    };

    //設定連結
    function setBreadcrumb(obj) {
        breadcrumbService.setBreadcrumb('home', {
            href: "/chart/" + $scope.link.linkTagSeq,
            label: "歷史資料"
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
    //初始化
    function init() {
        //找到 Device Id
        getLinkDevices();
        //取得linkSub 資料 
        getLinkTag($scope.link.linkTagSeq);
       //設定 Banner
        setBreadcrumb(null);
    };

    //重新導向
    function redirectToUrl(path) {
        $location.replace();
        $location.path(path);
    }
};

chartController.$inject = injectParams;

app.register.controller('chartController', chartController);