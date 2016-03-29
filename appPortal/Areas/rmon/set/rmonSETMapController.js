var injectParams = ['$scope', '$rootScope', '$q', '$location', '$http', 'groupFactory', 'breadcrumbService'];

var rmonSETMapController = function ($scope, $rootScope, $q, $location, $http, groupFactory, breadcrumbService) {
    $scope.breadcrumbs;

    $scope.currentView = '';
    $scope.vm = [];
    $scope.newVm = []
    $scope.newVm.parentGroup = [];
    $scope.newVm.currentGroup = [];
    $scope.groupTypes = '';
    $scope.showLinkMap = false;

    var uploadedCount = 0;

    $scope.files = [];
    $scope.file = {};

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

    //切換 View
    function setVisble(node) {
        $scope.currentView = node;
    };

    //controller 初始化
    function init() {
        breadcrumbService.setBreadcrumb('home', {
            href: '/rmonSETMap/',
            label: '群組階層'
        });

        $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();

        getGroups();
    }

    //取得群組
    function getGroups() {
        groupFactory.getGroups().then(processSuccess, processError);

        function processSuccess(data) {
            $scope.treedata = data;
        }

        function processError(error) {

        }
    }

    //取得群組資料
    function getGroup(id) {
        groupFactory.getGroup(id).then(processSuccess, processError);

        function processSuccess(data) {
            // binding 資料
            $scope.vm = data;
            //取得群組類別清單
            getGroupTypes($scope.vm.currentGroup.groupTypeKey);

            if ($scope.vm.currentGroup.groupTypeKey >= 2) {
                $scope.showLinkMap = true;
            }
            else {
                $scope.showLinkMap = false;
            }
        }

        function processError(error) {
            //顯示錯誤訊息
        }
    }

    //取得群組類型
    function getGroupTypes(groupTypeValue) {
        groupFactory.getGroupTypes(groupTypeValue).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.groupTypes = data;
        }

        function processError(error) {

        }
    }

    //重新導向


    //新增群組
    $scope.InsertGroup = function () {
        //清空 CurrentGroup Data
        groupFactory.getGroup(0).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.newVm = data;

            $scope.newVm.parentGroup.parentName = $scope.vm.currentGroup.groupName;

            $scope.newVm.currentGroup.parentId = $scope.vm.currentGroup.groupId;
            $scope.newVm.currentGroup.groupId = 0;
            $scope.newVm.currentGroup.groupCode = '';
            $scope.newVm.currentGroup.groupName = '';

            $scope.newVm.currentGroup.createUser = 8888;
            $scope.newVm.currentGroup.systemUser = 8888;
            $scope.newVm.currentGroup.groupTypeKey = $scope.vm.currentGroup.groupTypeKey;

            //取得群組類別清單
            $scope.newVm.currentGroup.groupTypeKeys = getGroupTypes($scope.vm.currentGroup.groupTypeKey);

            setVisble('Insert');

        }

        function processError(error) {

        }
    };

    //刪除群組
    $scope.DeleteGroup = function () {
        groupFactory.deleteGroup($scope.vm.currentGroup.groupId).then(processSuccess, processError);

        function processSuccess(data) {
            //取得最新
            getGroups();
        }
        function processError(error) {
            var e = error;
        }
    }

    //存檔群組資料
    $scope.UpdateGroup = function () {
        //if ($scope.editForm.$valid) {
        if (!$scope.vm.currentGroup.groupId) {

            groupFactory.insertGroup($scope.vm).then(processSuccess, processError);
        }
        else {
            groupFactory.updateGroup($scope.vm).then(processSuccess, processError);
        }

        //}

        function processSuccess(data) {
            $scope.editForm.$dirty = false;
            $scope.message = "更新成功";
            //取得最新
            getGroups();
        }

        function processError(error) {
            var e = error;
        }
    };

    //存檔
    $scope.Save = function () {
        var o = $scope.newVm;
        groupFactory.insertGroup($scope.newVm).then(processSuccess, processError);

        function processSuccess(data) {

            setVisble('default');
            getGroups();
        }
        function processError(error) {

        }

    };

    //取消
    $scope.Cancel = function () {
        setVisble('default');
    };

    //圖控設定
    $scope.Set = function () {
        if ($scope.vm.currentGroup.groupId) {
            redirectToUrl($location, '/rmonSET/' + $scope.vm.currentGroup.groupId);
        }
        else {
            alert("請選擇");
        }

    };



    //選擇階層
    $scope.$watch('group.currentNode', function (newObj, oldObj) {
        if ($scope.group && angular.isObject($scope.group.currentNode)) {

            setVisble('default');

            getGroup($scope.group.currentNode.id);

        }
    }, false);

    //執行初始化
    init();
};

rmonSETMapController.$inject = injectParams;

app.register.controller('rmonSETMapController', rmonSETMapController);
