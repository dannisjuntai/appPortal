var injectParams = ['$scope', '$location', 'groupFactory', 'breadcrumbService'];

var optionController = function ($scope, $location, groupFactory, breadcrumbService) {
    $scope.breadcrumbs;

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

    $scope.optionSets = [];
    //
    $scope.param = {
        fieldName: '',
        select: []
    };
    $scope.option = {
        
        currentView: '',
        //fieldName: '',
        //optionName: '',
        //optionNo: 0
    }


    $scope.chang = function (f) {
        //取得集合
        if (f) {
            getOptionSets($scope.param);
        }
    };

    $scope.setOption = function (optionNo) {
        setView('insert');
        $scope.option.optionId = 0;
    };
    $scope.save = function () {
        setOptionSets($scope.option);
        setView('');
    };
    $scope.update = function (o) {
        $scope.option = o;
        setView('insert');

    }
    $scope.cancel = function () {
        setView('');
        //$scope.option =[];
    };
    init();
    function init() {
        breadcrumbService.setBreadcrumb('home', {
            href: '/menu/option',
            label: '參數設定'
        });

        $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();

        getOptionFieldName();
    };

    function getOptionFieldName() {
        groupFactory.getOptionFieldName().then(processSuccess, processError);

        function processSuccess(data) {
            $scope.param.select = data;
        };
        function processError(error) {

        };
    };

    function getOptionSets(param) {
        groupFactory.getOptionSets(param).then(processSuccess, processError);
        function processSuccess(data) {
            $scope.optionSets = data;
        };
        function processError(error) {

        };
    }
    function setOptionSets(option) {
        groupFactory.setOptionSets(option).then(processSuccess, processError);
        function processSuccess(data) {
            $scope.optionSets = data;
        };
        function processError(error) {

        };
    }
    //設定顯示區塊
    function setView(view) {
        $scope.option.currentView = view;
    }
};

optionController.$inject = injectParams;


app.register.controller('optionController', optionController);