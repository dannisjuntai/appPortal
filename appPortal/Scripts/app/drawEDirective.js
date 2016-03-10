var injectParams = [];
//繪圖元件
var drawEDirective = function () {
    return {
        link: function ($scope, element, iAttrs, controller) {

            element.bind('click', function () {
                //console.log(iAttrs);
                //var obj = JSON.parse(iAttrs.shape);
                //var width = iAttrs.width;
                //var height = iAttrs.height;
                //var drawAttr = {
                //    attrib: iAttrs.shape,
                //    width: parseInt(iAttrs.width),
                //    height: parseInt(iAttrs.height)
                //}
                $scope.addDrawElement(iAttrs);
                //switch (obj.type) {
                //    case "svg":
                //        $scope.addDrawElement(obj.name);
                //        break;
                //    case "element":
                //        $scope.addDataElement(obj.name, 100, 100);
                //        break;
                //    default:
                //        break;
                //}

            })

        }
    };
};

drawEDirective.$inject = injectParams;

app.directive('drawE', drawEDirective);

/*
app.directive('fileUpload', function () {
    return {
        scope: true,        //create a new scope
        link: function (scope, el, attrs) {
            el.bind('change', function (event) {
                var files = event.target.files;
                //iterate files since 'multiple' may be specified on the element
                for (var i = 0; i < files.length; i++) {
                    //emit event upward
                    scope.$emit("fileSelected", { file: files[i] });
                }
            });
        }
    };
});

//listen for the file selected event 監聽 fileSelected 事件
$scope.$on("fileSelected", function (event, args) {
    $scope.$apply(function () {
        //add the file object to the scope's files collection
        $scope.files.push(args.file);
    });
});
*/