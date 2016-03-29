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

    //$scope.options = [];
    $scope.optionSets = [];

    $scope.options = { select: [], fieldName: '' }
    

    $scope.chang = function (f) {

        //取得集合
        if (f) {
            getOptionSets($scope.options);
        }
    };

    $scope.setOption = function (optionNo) {

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
            $scope.options.select = data;
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
};

optionController.$inject = injectParams;


app.register.controller('optionController', optionController);