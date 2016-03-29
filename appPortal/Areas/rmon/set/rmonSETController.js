var injectParams = ['$scope', '$routeParams', '$location', 'groupFactory', 'breadcrumbService'];

var rmonSETController = function ($scope, $routeParams,$location, groupFactory, breadcrumbService) {
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

    var status = { Added: 0, Unchanged: 1, Modified: 2, Deleted: 3 };
    var statusEnum = new enums(status);
    //
    $scope.groupId = ($routeParams.groupId) ? parseInt($routeParams.groupId) : 0;
    $scope.imageId = 0;
    $scope.showModal = false;
    //圖控 Select 元件資料 
    $scope.linkDevices = '';
    $scope.linkTag = '';
    $scope.links;
    //圖控設定資料
    $scope.link = [];

    $scope.selectedRow = null;
    //顯示Modal 
    $scope.toggleModal = function (o) {
        var oo = o;
        getLinkTag(o.linkTagSeq);
    };
    //設定linkTag
    $scope.setLinkTag = function(o){
        setLinkTag(o);
    }
    //$scope.deawData = getdrawData();
    //$scope.deawElement = getdeawElement();

    canvas = new fabric.Canvas(document.getElementById("drawCanvas"));
    
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
        //設定背景圖
        canvas.setBackgroundImage(imgbase64, canvas.renderAll.bind(canvas));
        
    };

    function enums(eo) {
        var enums = function () { };
        for (var key in eo) {
            var enumObject = new enums();
            enums[key] = eo[key];
        }
        return enums;
    }
    //初始化
    function init() {
        breadcrumbService.setBreadcrumb('home', {
            href: '/rmonSET/' + $scope.groupId,
            label: '底圖設定'
        });

        $scope.breadcrumbs = breadcrumbService.getBreadcrumbs();

        //取得圖檔
        getGroupImages($scope.groupId);
        //取得圖控資訊
        getGroupLocations($scope.groupId);

        getLinkDevices();

    }

    function DrawLinks() {
        //移除
        canvas.forEachObject(function (obj) {
            canvas.remove(obj);
        });
        //重畫
        $scope.links.forEach(function (link) {
            //$scope.addDeviceLink(link);
            $scope.drawDevice(link);
        });
    }
    //從資料庫 取得Background 圖檔
    function getGroupImages(id) {
        groupFactory.getGroupImages(id).then(processSuccess, processError);

        function processSuccess(data) {
            // binding 資料
            image.src = "data:image/png;base64," + data.base64;
            $scope.imageId = data.imageId;
        }
        function processError(error) {

        }
    }
    //取得圖控資訊
    function getGroupLocations(id) {
        groupFactory.getGroupLocations(id).then(processSuccess, processError);

        function processSuccess(data) {
            // binding 資料
            //image.src = "data:image/png;base64," + data.base64;
            $scope.links = data;
            DrawLinks();
        }
        function processError(error) {

        }
    }

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
    //取得LinkTag
    function getLinkTags(linkSubSeq) {
        groupFactory.getLinkTags(linkSubSeq).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.linkTags = '';
            $scope.linkTags = data;
        }
        function processError(error) {

        }
    }

    //取得LinkTag資料
    function getLinkTag(linkTagSeq) {
       
        groupFactory.getLinkTag(linkTagSeq).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.linkTag = data;
            $scope.showModal = !$scope.showModal;
            
        }
        function processError(error) {

        }
    };
    //設定LinkTag
    function setLinkTag(linkTag) {
        groupFactory.setLinkTag(linkTag).then(processSuccess, processError);

        function processSuccess(data) {
            var i = data;
            //$scope.linkTag = data;
            //$scope.showModal = !$scope.showModal;
        }
        function processError(error) {

        }
    }
    $scope.getActiveStyle = getActiveStyle;

    addAccessors($scope);
    watchCanvas($scope);

    var uploadedCount = 0;

    $scope.files = [];
    $scope.file = {};

    //上傳圖檔
    $scope.uploadFiles = function () {
        var files = angular.copy($scope.files);

        if ($scope.file) {
            $scope.file.groupId = $scope.groupId;
            $scope.file.imageId = $scope.imageId;
            $scope.file.data = '';
            files.push($scope.file);
        }

        if (files.length === 0) {
            $window.alert('Please select files!');
            return false;
        }

        for (var i = files.length - 1; i >= 0; i--) {
            var file = files[i];
            groupFactory.saveBackground(file).then(processSuccess, processError);

            function processSuccess(data) {
                //載入
                image.src = 'data:' + data.FileType + ';base64,' + data.base64;
            };

            function processError(errpr) {

            };
        }
    };

    //新增 insert Location
    $scope.insertLocation = function (link) {

        setGroupLocations();

        function setGroupLocations() {

            var ld = $scope.links.length + 1;
            var dev = updateLinkDevName(link);
            var tag = updateTagName(link);
            //加入資料
            var newLink = ({
                locationId: 0,
                groupId: $scope.groupId,
                linkSubSeq: link.linkSubSeq,
                linkDevName: dev.linkDevName,
                linkTagSeq: link.linkTagSeq,
                tagName: tag.tagName,
                mTagSeq: tag.mTagSeq,
                tObjSeq: tag.tObjSeq,
                locationValue: JSON.stringify({x:150, y:150}),
                prompt: link.prompt,
                modifyFlag: status.Added
            });

            $scope.links.push(newLink);

            groupFactory.setGroupLocations($scope.links).then(processSuccess, processError);

            function processSuccess(data) {
                var d = data;
                $scope.links = data;
                //清空
                $scope.selectedRow = null;
                $scope.link = null;
                //繪製圖
                DrawLinks();
            }
            function processError(error) {

            }
        }
        //
        function updateLinkDevName(l) {
            var arr = $scope.linkDevices;
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].linkSubSeq == l.linkSubSeq) {
                    return arr[i];
                }
            }
        }
        //
        function updateTagName(link) {
            var arr = $scope.linkTags;
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].linkTagSeq == link.linkTagSeq) {
                    return arr[i];
                }
            }
        }
    };
    //更新
    $scope.updateLink = function (link) {
        groupFactory.setGroupLocations($scope.links).then(processSuccess, processError);

        function processSuccess(data) {
            var d = data;
            $scope.links = data;
            //清空
            $scope.selectedRow = null;
            $scope.link = null;
            alert("存檔成功");
        }
        function processError(error) {

        }
    };

    Array.prototype.remove = function () {
        var what, a = arguments, L = a.length, ax;
        while (L && this.length) {
            what = a[--L];
            while ((ax = this.indexOf(what)) !== -1) {
                this.splice(ax, 1);
            }
        }
        return this;
    };

    $scope.removeLink = function (link) {

        groupFactory.deleteGroupLocation(link).then(processSuccess, processError);

        function processSuccess(data) {
            $scope.links = data;
            $scope.selectedRow = null;
            $scope.link = null;

            $scope.RemoveDeviceLink(link);
        }
        
        function processError(error) {

        }

    };
    //combox 
    $scope.changLink = function (link) {
        getLinkTags(link.linkSubSeq);
    };
    //選擇
    $scope.Selected = function (link, index) {

        if ($scope.selectedRow != index) {
            $scope.selectedRow = index;
            //點位不做更新
            //$scope.link = link;
        }
        else {
            $scope.selectedRow = null;
            $scope.link = null;
        }
    }


    init();

    //$scope.$watchCollection
    $scope.$watchCollection('links', function (newVal, oldVal) {
        if (newVal != 'undefined') {
            //判斷狀態
            console.log(newVal);
        }
    });
};

rmonSETController.$inject = injectParams;

app.register.controller('rmonSETController', rmonSETController);

function watchCanvas($scope) {

    function updateScope(option) {
        $scope.$$phase || $scope.$digest();
        canvas.renderAll();
    }

    function afterRender(option) {
        var p = canvas.getPointer(option.e);

        var activeObj = canvas.getActiveObject();

        if (typeof activeObj == 'undefined') {
            return;
        }

        
        if (activeObj != null) {
            $scope.links.forEach(function (link) {
                var token = activeObj.type.split("_");
                var lid = token[1];
                if (link.locationId == lid) {
                    console.log(p);
                    //binding Data
                    link.locationValue = JSON.stringify({ x: activeObj.left, y: activeObj.top });
                }
            });
        }
    }

    function beforeSelection(option) {
        var p = canvas.getPointer(option.e);
        var activeObj = canvas.getActiveObject();
        //binding Data
        if (typeof activeObj != 'undefined') {
            //$scope.draw = activeObj;
            //$scope.links.forEach()
        }
    }

    function mouseMove(options) {
        var p = canvas.getPointer(options.e);


        var activeObj = canvas.getActiveObject();
        if (!activeObj) {
            return;
        }

        var elementType;
        var promptType;
        var token;
        if (typeof activeObj.type == 'undefined') {
            return;
        }

        token = activeObj.type.split("_");
        promptType = "prompt_" + token[1];
        //refashPrompt(promptType);

        function refashPrompt(promptType) {

            //console.log(promptType);
            //移除提示文字
            canvas.forEachObject(function (obj) {
                if (obj.type.indexOf(promptType) > -1) {
                    var type = obj.type;
                    canvas.remove(obj);

                    //加入
                    var prompt = new fabric.Text('', {
                        id: obj.id,
                        type: type,
                        fontSize: 16,
                        left: activeObj.left + (activeObj.width) + 10,
                        top: activeObj.top,
                        text: obj.text,
                        selectable: 0
                    });
                    canvas.add(prompt);
                }
            });

            canvas.renderAll();
        }
    }

    function mouseOver(options) {
        var p = canvas.getPointer(options.e);

        var activeObj = canvas.getActiveObject();
        if (activeObj) {
            var t = $scope.getText();
        }
    }
    canvas
        .on('object:selected', updateScope)
        .on('group:selected', updateScope)
        .on('path:created', updateScope)
        .on('selection:cleared', updateScope)

        .on('mouse:move', mouseMove)
        .on('mouse:over', mouseOver)
        .on('after:render', afterRender)
        .on('before:selection:cleared', beforeSelection);
}

function addAccessors($scope) {
    //加入圖控元件
    $scope.addDeviceLink = function (link) {
       // var l = link.locationValue;
        var obj = JSON.parse(link.locationValue);
        var circle = new fabric.Circle({
            //id: link.locationId,
            type: 'element_' + link.locationId, //識別碼
            //left: obj.x,
            //top: obj.y,
            fill: '#89deae',//'#fcee21', //+ getRandomColor(),
            radius: 12,
            opacity: 1,
            stroke: '#909ab2',
            strokeWidth: 2,
            borderColor: 'red',
            cornerColor: 'green',
            //hasRotatingPoint: false,
            //transparentCorners: false,
            hasControls: false,
            cornerSize: 6
        });

        //canvas.add(circle);

        //新增圖文
        var type = 'prompt_' + link.locationId;
        var t = link.tagName + '\n'; //link.locationId.toString()+ '\n'
        var left = circle.left;//circle.left + (circle.width * circle.scaleX) + 10;
        var top = circle.top + circle.height + 10;

        var text = new fabric.Text(t, {
            type: type,
            left: left,
            top: top,
            fontSize: 16,
            fill: 'red',
            originX: 'left',
            hasRotatingPoint: true,
            centerTransform: true,
            selectable: false
        });

        var group = new fabric.Group([circle, text], {
            type: 'group_' + link.locationId,
            left: obj.x,
            top: obj.y,
            hasControls: false,
            hasBorders: false
        });

        canvas.add(group);

    };
    //繪製圖控
    $scope.drawDevice = function (link) {
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

        //新增圖文
        var type = 'prompt_' + link.locationId;
        var text = link.tagName;// + '  ' + link.curfValue;
        var left = rect.left + 10;
        var top = rect.top + 5;

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

        var value = '' + link.curfValue;
        var value = new fabric.Text(value, {
            type: type,
            left: text.width + 20,
            top: top,
            fontSize: 16,
            fill: '#000000',
            originX: 'left',
            textBackgroundColor: '#ffc40d',
            hasRotatingPoint: true,
            centerTransform: true,
            selectable: false
        });

        var group = new fabric.Group([rect, text, value], {
            type: 'group_' + link.locationId,
            left: obj.x,
            top: obj.y,
            //evented: false,
            hasControls: false,
            hasBorders: false
        });
        canvas.add(group);
    };
    //繪製點位

    //移除 Link Device
    $scope.RemoveDeviceLink = function (link) {

        objType = 'group_' + link.locationId;

        canvas.forEachObject(function (obj) {
            if (obj.type.indexOf(objType) > -1) {
                var type = obj.type;
                canvas.remove(obj);
            }
        });

        canvas.renderAll();
    };

    $scope.removeText = function (activeObj) {
        if (typeof activeObj.index == 'undefined')
            return;
        var token = activeObj.index.split("_");
        var promptType = "prompt_" + token[1];
        //console.log(promptType);
        //移除提示文字
        canvas.forEachObject(function (obj) {
            if (obj.type.indexOf(promptType) > -1) {
                var type = obj.type;
                canvas.remove(obj);
            }
        });
    };

    $scope.getOriginX = function () {
        return getActiveStyle('originX');
    };

    $scope.getOpacity = function () {
        return getActiveStyle('opacity') * 100;
    };
    $scope.setOpacity = function (value) {
        setActiveStyle('opacity', parseInt(value, 10) / 100);
    };

    $scope.getFill = function () {
        return getActiveStyle('fill');
    };
    $scope.setFill = function (value) {
        setActiveStyle('fill', value);
    };

    $scope.isBold = function () {
        return getActiveStyle('fontWeight') === 'bold';
    };
    $scope.toggleBold = function () {
        setActiveStyle('fontWeight',
          getActiveStyle('fontWeight') === 'bold' ? '' : 'bold');
    };
    $scope.isItalic = function () {
        return getActiveStyle('fontStyle') === 'italic';
    };
    $scope.toggleItalic = function () {
        setActiveStyle('fontStyle',
          getActiveStyle('fontStyle') === 'italic' ? '' : 'italic');
    };

    $scope.isUnderline = function () {
        return getActiveStyle('textDecoration').indexOf('underline') > -1;
    };
    $scope.toggleUnderline = function () {
        var value = $scope.isUnderline()
          ? getActiveStyle('textDecoration').replace('underline', '')
          : (getActiveStyle('textDecoration') + ' underline');

        setActiveStyle('textDecoration', value);
    };

    $scope.isLinethrough = function () {
        return getActiveStyle('textDecoration').indexOf('line-through') > -1;
    };
    $scope.toggleLinethrough = function () {
        var value = $scope.isLinethrough()
          ? getActiveStyle('textDecoration').replace('line-through', '')
          : (getActiveStyle('textDecoration') + ' line-through');

        setActiveStyle('textDecoration', value);
    };
    $scope.isOverline = function () {
        return getActiveStyle('textDecoration').indexOf('overline') > -1;
    };
    $scope.toggleOverline = function () {
        var value = $scope.isOverline()
          ? getActiveStyle('textDecoration').replace('overline', '')
          : (getActiveStyle('textDecoration') + ' overline');

        setActiveStyle('textDecoration', value);
    };

    $scope.getText = function () {
        return getActiveProp('text');
    };
    $scope.setText = function (value) {
        setActiveProp('text', value);
    };

    $scope.getTextAlign = function () {
        return capitalize(getActiveProp('textAlign'));
    };
    $scope.setTextAlign = function (value) {
        setActiveProp('textAlign', value.toLowerCase());
    };

    $scope.getFontFamily = function () {
        return getActiveProp('fontFamily').toLowerCase();
    };
    $scope.setFontFamily = function (value) {
        setActiveProp('fontFamily', value.toLowerCase());
    };

    $scope.getBgColor = function () {
        return getActiveProp('backgroundColor');
    };
    $scope.setBgColor = function (value) {
        setActiveProp('backgroundColor', value);
    };

    $scope.getTextBgColor = function () {
        return getActiveProp('textBackgroundColor');
    };
    $scope.setTextBgColor = function (value) {
        setActiveProp('textBackgroundColor', value);
    };

    $scope.getStrokeColor = function () {
        return getActiveStyle('stroke');
    };
    $scope.setStrokeColor = function (value) {
        setActiveStyle('stroke', value);
    };

    $scope.getStrokeWidth = function () {
        return getActiveStyle('strokeWidth');
    };
    $scope.setStrokeWidth = function (value) {
        setActiveStyle('strokeWidth', parseInt(value, 10));
    };

    $scope.getFontSize = function () {
        return getActiveStyle('fontSize');
    };
    $scope.setFontSize = function (value) {
        setActiveStyle('fontSize', parseInt(value, 10));
    };

    $scope.getLineHeight = function () {
        return getActiveStyle('lineHeight');
    };
    $scope.setLineHeight = function (value) {
        setActiveStyle('lineHeight', parseFloat(value, 10));
    };

    $scope.getBold = function () {
        return getActiveStyle('fontWeight');
    };
    $scope.setBold = function (value) {
        setActiveStyle('fontWeight', value ? 'bold' : '');
    };

    $scope.getCanvasBgColor = function () {
        return canvas.backgroundColor;
    };
    $scope.setCanvasBgColor = function (value) {
        canvas.backgroundColor = value;
        canvas.renderAll();
    };




    $scope.addSample = function () {

        // addShape(3);
    };

    $scope.addPath = function (path) {
        var path = new fabric.Path(path);

        canvas.add(path.set({ left: 5, top: 5 }));
    };




    $scope.addRect = function (attr) {
        var x = parseInt(attr.rx);
        var y = parseInt(attr.ry);
        var w = parseInt(attr.width);
        var h = parseInt(attr.height);

        var rect = new fabric.Rect({
            //id: $scope.elementId,
            //type: 'element_' + attr.shape,
            left: 10,
            top: 10,
            rx: x,
            ry: y,
            fill: '#89deae',
            width: 20,
            height: 20,
            opacity: 1,
            stroke: '#909ab2',
            strokeWidth: 2,
            //hasControls: false,
            borderColor: 'red',
            cornerColor: 'green',
            hasRotatingPoint: false,
            //cornerColor: 'green',
            transparentCorners: false,
            cornerSize: 6

        });
        canvas.add(rect);
        //var coord = getRandomLeftTop();

        /*
        var rect = new fabric.Rect({
            //id: $scope.elementId,
            //type: 'element_' + $scope.elementId,
            left: 10,//coord.left,
            top: 10, //coord.top,
            rx: 2,
            ry: 2,
            fill: '#89deae',//'#fcee21',//'#' + getRandomColor(),
            width: w,
            height: h,
            opacity: 1,
            stroke: '#909ab2',
            strokeWidth: 2
        });

        canvas.add(rect);
        */
        //rect.setGradient('fill', {
        //    x1: -rect.getWidth() / 2, y1: -rect.getWidth() / 2,
        //    x2: rect.getWidth() / 2, y2: rect.getHeight() / 2,
        //    colorStops: { '0': '#fff', '0.8': '#555', '1': '#222' }
        //});
        //var type = 'prompt_' + $scope.elementId;
        //var text = $scope.elementId + '\n';;
        //var left = rect.left + (rect.width) + 10;
        //var top = rect.top;
        //$scope.addPromptText(rect.id, type, text, left, top);

        //$scope.elementId++;
    };

    $scope.addCircle = function (radius, width, height) {
        var coord = getRandomLeftTop();

        var circle = new fabric.Circle({
            id: $scope.elementId,
            type: 'element_' + $scope.elementId,
            left: coord.left,
            top: coord.top,
            fill: '#fcee21', //+ getRandomColor(),
            radius: 25,
            opacity: 0.8,
            stroke: 'rgba(0,0,0,0.8)',
            strokeWidth: 1
        });

        canvas.add(circle);

        var type = 'prompt_' + $scope.elementId;
        var text = $scope.elementId + '\n';
        var left = circle.left + (circle.width * circle.scaleX) + 10;
        var top = circle.top;
        $scope.addPromptText(circle.id, type, text, left, top);

        $scope.elementId++;
    };


    /*
    加入資料元件
    */
    $scope.addDataElement = function (fileName, l, t) {

        fabric.loadSVGFromURL('assets/' + fileName + '.svg', function (objects, options) {
            var loadedObject = fabric.util.groupSVGElements(objects, options);
            loadedObject.set({
                left: l,
                top: t,
                index: 'dataElement_' + $scope.darwIndex
            }).setCoords();
            canvas.add(loadedObject);

            var type = 'prompt_' + $scope.darwIndex;
            var text = $scope.darwIndex + '\n';
            var left = loadedObject.left + (loadedObject.width * loadedObject.scaleX) + 10;
            var top = loadedObject.top;
            $scope.addPromptText(loadedObject.id, type, text, left, top);
            $scope.darwIndex++;
        });
    }
    /*
    加入繪圖元件
    */
    $scope.addDrawElement = function (attr) {
        var attr = attr;
        //轉換成數字
        var w = parseInt(attr.width);
        var h = parseInt(attr.height);


        var addRect = function (attr) {
            var x = parseInt(attr.rx);
            var y = parseInt(attr.ry);

            var rect = new fabric.Rect({
                //id: $scope.elementId,
                //type: 'element_' + attr.shape,
                left: 10,
                top: 10,
                rx: x,
                ry: y,
                fill: '#89deae',
                width: w,
                height: h,
                opacity: 1,
                stroke: '#909ab2',
                strokeWidth: 2,
                borderColor: 'red',
                cornerColor: 'green',
                hasRotatingPoint: false,
                transparentCorners: false,
                cornerSize: 6

            });
            canvas.add(rect);
        }

        var addCircle = function (attr) {

            var circle = new fabric.Circle({
                id: $scope.elementId,
                type: 'element_' + $scope.elementId,
                left: 50,
                top: 50,
                fill: attr.fill,//'#fcee21', //+ getRandomColor(),
                radius: attr.r,
                opacity: 1,
                stroke: '#909ab2',
                strokeWidth: 2,
                borderColor: 'red',
                cornerColor: 'green',
                hasRotatingPoint: false,
                transparentCorners: false,
                cornerSize: 6
            });

            canvas.add(circle);
        }

        switch (attr.shape) {
            case "circle":
                addCircle(attr);
                break;
            case "rect":
                addRect(attr);
                break;
            default:
                break;
        }


    }

    $scope.addDrawElement1 = function (fileName) {
        fabric.loadSVGFromURL('assets/' + fileName + '.svg', function (objects, options) {
            var loadedObject = fabric.util.groupSVGElements(objects, options);
            loadedObject.set({
                left: 100,
                top: 100,
                index: 'drawElement_' + $scope.darwIndex
            }).setCoords();
            canvas.add(loadedObject);

            var type = 'prompt_' + $scope.darwIndex;
            var text = $scope.darwIndex + '\n';
            var left = loadedObject.left + (loadedObject.width * loadedObject.scaleX) + 10;
            var top = loadedObject.top;
            $scope.addPromptText(loadedObject.id, type, text, left, top);
            $scope.darwIndex++;
        });


    };

    function addFElement(fileName, index) {
        fabric.loadSVGFromURL('assets/' + fileName + '.svg', function (objects, options) {
            var loadedObject = fabric.util.groupSVGElements(objects, options);
            loadedObject.set({
                left: 100,
                top: 100
            }).setCoords();
            canvas.add(loadedObject);

            var type = 'prompt_' + $scope.elementId;
            var text = $scope.elementId + '\n';
            var left = loadedObject.left + (loadedObject.width * loadedObject.scaleX) + 10;
            var top = loadedObject.top;
            $scope.addPromptText(loadedObject.id, type, text, left, top);
            $scope.elementId++;

        });

    };

    $scope.addSVG = function (fileName) {

        var coord = getRandomLeftTop();

        fabric.loadSVGFromURL('assets/' + fileName + '.svg', function (objects, options) {

            var loadedObject = fabric.util.groupSVGElements(objects, options);
            //loadedObject.scale(0.2);
            loadedObject.set({
                //id: $scope.elementId,
                //type: 'element_' + $scope.elementId,
                left: 5,
                top: 5,

                opacity: 0.9
            }).setCoords();

            canvas.add(loadedObject);

            //var type = 'prompt_' + $scope.elementId;
            //var text = $scope.elementId + '\n';
            //var left = loadedObject.left + (loadedObject.width * loadedObject.scaleX) + 10;
            //var top = loadedObject.top;
            // $scope.addPromptText(loadedObject.id, type, text, left, top);
            //$scope.elementId++;
        });


    };

    $scope.addPromptText = function (id, type, text, left, top) {

        var t = new fabric.Text(text, {
            id: id,
            type: type,
            left: left,
            top: top,
            fontSize: 16,
            originX: 'left',
            hasRotatingPoint: true,
            centerTransform: true,
            selectable: false
        });

        canvas.add(t);
    };

    $scope.addTriangle = function () {
        var coord = getRandomLeftTop();

        canvas.add(new fabric.Triangle({
            left: coord.left,
            top: coord.top,
            fill: '#' + getRandomColor(),
            width: 50,
            height: 50,
            opacity: 0.8
        }));
    };

    $scope.addLine = function () {
        var coord = getRandomLeftTop();

        canvas.add(new fabric.Line([50, 100, 200, 200], {
            left: coord.left,
            top: coord.top,
            stroke: '#' + getRandomColor()
        }));
    };

    $scope.addPolygon = function () {
        var coord = getRandomLeftTop();

        this.canvas.add(new fabric.Polygon([
          { x: 185, y: 0 },
          { x: 250, y: 100 },
          { x: 385, y: 170 },
          { x: 0, y: 245 }], {
              left: coord.left,
              top: coord.top,
              fill: '#' + getRandomColor()
          }));
    };


    $scope.addText = function () {
        var text = '500';

        var textSample = new fabric.Text(text.slice(0, getRandomInt(0, text.length)), {
            left: getRandomInt(350, 400),
            top: getRandomInt(350, 400),
            fontFamily: 'helvetica',
            angle: getRandomInt(-10, 10),
            fill: '#' + getRandomColor(),
            scaleX: 0.5,
            scaleY: 0.5,
            fontWeight: '',
            originX: 'left',
            hasRotatingPoint: true,
            centerTransform: true
        });

        canvas.add(textSample);
    };

    var addShape = function (shapeName) {

        console.log('adding shape', shapeName);

        var coord = getRandomLeftTop();

        fabric.loadSVGFromURL('assets/' + shapeName + '.svg', function (objects, options) {

            var loadedObject = fabric.util.groupSVGElements(objects, options);

            loadedObject.set({
                left: coord.left,
                top: coord.top,
                angle: getRandomInt(0, 0),
                opacity: 0.8
            }).setCoords();

            canvas.add(loadedObject);
        });
    };

    $scope.maybeLoadShape = function (e) {
        var $el = $(e.target).closest('button.shape');
        if (!$el[0]) {
            return;
        }
        var id = $el.prop('id'), match;
        if (match = /\d+$/.exec(id)) {
            addShape(match[0]);
        }
    };

    function addImage(imageName, minScale, maxScale) {
        var coord = getRandomLeftTop();

        fabric.Image.fromURL('../assets/' + imageName, function (image) {

            image.set({
                left: coord.left,
                top: coord.top
                //angle: getRandomInt(-10, 10)
            })
            .scale(getRandomNum(minScale, maxScale))
            .setCoords();

            canvas.add(image);
        });
    };
    function addbackimage(imageName) {

        fabric.Image.fromURL('../assets/' + imageName, function (image) {

            image.set({
                selectable: false
            }).setCoords();
            canvas.add(image);
        });
    }
    //加入提示圖檔
    function addPrompt(imgName, left, top) {
        fabric.Image.fromURL(imgName, function (image) {
            image.set({
                type: "promptImg",
                left: left,
                top: top,
                opacity: 1,
                hasControls: 0,
                visible: 0,
                text: 'propmt'
            });
            canvas.add(image);
        });
    };

    $scope.addImage1 = function () {
        addPrompt('../assets/prompt.png', 1050, 600);
    };

    $scope.addImage2 = function () {
        addImage('logo.png', 0.1, 1);
    };

    $scope.addImage3 = function () {
        addImage('printio.png', 0.5, 0.75);
    };

    $scope.addImage5 = function (imageName) {

        addbackimage(imageName);
    };
    $scope.confirmClear = function () {
        if (confirm('您確定清除?')) {
            canvas.clear();
        }
    };

    $scope.remove = function () {
        var activeObject = canvas.getActiveObject(),
            activeGroup = canvas.getActiveGroup();

        if (activeGroup) {
            var objectsInGroup = activeGroup.getObjects();
            canvas.discardActiveGroup();
            objectsInGroup.forEach(function (object) {
                canvas.remove(object);
            });
        }
        else if (activeObject) {
            console.log("remove");
            canvas.remove(activeObject);
        }
    };

    $scope.rasterize = function () {
        if (!fabric.Canvas.supports('toDataURL')) {
            alert('This browser doesn\'t provide means to serialize canvas to an image');
        }
        else {
            window.open(canvas.toDataURL('png'));
        }
    };

    $scope.rasterizeSVG = function () {
        window.open(
          'data:image/svg+xml;utf8,' +
          encodeURIComponent(canvas.toSVG()));
    };

    $scope.rasterizeJSON = function () {
        $scope.setConsoleJSON(JSON.stringify(canvas));
    };

    $scope.getSelected = function () {
        return canvas.getActiveObject();
    };

    $scope.removeSelected = function () {
        var activeObject = canvas.getActiveObject(),
            activeGroup = canvas.getActiveGroup();

        if (activeGroup) {
            var objectsInGroup = activeGroup.getObjects();
            canvas.discardActiveGroup();
            objectsInGroup.forEach(function (object) {
                canvas.remove(object);
                $scope.removeText(object);
            });
        }
        else if (activeObject) {
            $scope.removeText(activeObject);
            canvas.remove(activeObject);

        }
    };

    $scope.getHorizontalLock = function () {
        return getActiveProp('lockMovementX');
    };
    $scope.setHorizontalLock = function (value) {
        setActiveProp('lockMovementX', value);
    };

    $scope.getVerticalLock = function () {
        return getActiveProp('lockMovementY');
    };
    $scope.setVerticalLock = function (value) {
        setActiveProp('lockMovementY', value);
    };

    $scope.getScaleLockX = function () {
        return getActiveProp('lockScalingX');
    },
    $scope.setScaleLockX = function (value) {
        setActiveProp('lockScalingX', value);
    };

    $scope.getScaleLockY = function () {
        return getActiveProp('lockScalingY');
    };
    $scope.setScaleLockY = function (value) {
        setActiveProp('lockScalingY', value);
    };

    $scope.getRotationLock = function () {
        return getActiveProp('lockRotation');
    };
    $scope.setRotationLock = function (value) {
        setActiveProp('lockRotation', value);
    };

    $scope.getOriginX = function () {
        return getActiveProp('originX');
    };
    $scope.setOriginX = function (value) {
        setActiveProp('originX', value);
    };

    $scope.getOriginY = function () {
        return getActiveProp('originY');
    };
    $scope.setOriginY = function (value) {
        setActiveProp('originY', value);
    };

    $scope.sendBackwards = function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject) {
            canvas.sendBackwards(activeObject);
        }
    };

    $scope.sendToBack = function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject) {
            canvas.sendToBack(activeObject);
        }
    };

    $scope.bringForward = function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject) {
            canvas.bringForward(activeObject);
        }
    };

    $scope.bringToFront = function () {
        var activeObject = canvas.getActiveObject();
        if (activeObject) {
            canvas.bringToFront(activeObject);
        }
    };

    var pattern = new fabric.Pattern({
        source: '/assets/escheresque.png',
        repeat: 'repeat'
    });

    $scope.patternify = function () {
        var obj = canvas.getActiveObject();

        if (!obj) return;

        if (obj.fill instanceof fabric.Pattern) {
            obj.fill = null;
        }
        else {
            if (obj instanceof fabric.PathGroup) {
                obj.getObjects().forEach(function (o) { o.fill = pattern; });
            }
            else {
                obj.fill = pattern;
            }
        }
        canvas.renderAll();
    };

    $scope.clip = function () {
        var obj = canvas.getActiveObject();
        if (!obj) return;

        if (obj.clipTo) {
            obj.clipTo = null;
        }
        else {
            var radius = obj.width < obj.height ? (obj.width / 2) : (obj.height / 2);
            obj.clipTo = function (ctx) {
                ctx.arc(0, 0, radius, 0, Math.PI * 2, true);
            };
        }
        canvas.renderAll();
    };

    $scope.shadowify = function () {
        var obj = canvas.getActiveObject();
        if (!obj) return;

        if (obj.shadow) {
            obj.shadow = null;
        }
        else {
            obj.setShadow({
                color: 'rgba(0,0,0,0.3)',
                blur: 10,
                offsetX: 10,
                offsetY: 10
            });
        }
        canvas.renderAll();
    };

    $scope.gradientify = function () {
        var obj = canvas.getActiveObject();
        if (!obj) return;

        obj.setGradient('fill', {
            x1: 0,
            y1: 0,
            x2: (getRandomInt(0, 1) ? 0 : obj.width),
            y2: (getRandomInt(0, 1) ? 0 : obj.height),
            colorStops: {
                0: '#' + getRandomColor(),
                1: '#' + getRandomColor()
            }
        });
        canvas.renderAll();
    };

    $scope.execute = function () {
        if (!(/^\s+$/).test(consoleValue)) {
            eval(consoleValue);
        }
    };

    var consoleSVGValue = ''

    var consoleValue = '';

    var consoleJSONValue = (
      '{"objects":[{"type":"i-text","originX":"left","originY":"top","left":51,"top":282,"width":230.05,"height":235.94,"fill":"#333","stroke":null,"strokeWidth":1,"strokeDashArray":null,"strokeLineCap":"butt","strokeLineJoin":"miter","strokeMiterLimit":10,"scaleX":1,"scaleY":1,"angle":0,"flipX":false,"flipY":false,"opacity":1,"shadow":null,"visible":true,"clipTo":null,"backgroundColor":"","fillRule":"nonzero","globalCompositeOperation":"source-over","transformMatrix":null,"skewX":0,"skewY":0,"text":"lorem ipsum\ndolor\nsit Amet\nconsectetur","fontSize":40,"fontWeight":"normal","fontFamily":"Helvetica","fontStyle":"","lineHeight":1.16,"textDecoration":"","textAlign":"left","textBackgroundColor":"","styles":{"0":{"0":{"fill":"red","fontSize":20,"fontFamily":"Helvetica","fontWeight":"normal","fontStyle":""},"1":{"fill":"red","fontSize":30,"fontFamily":"Helvetica","fontWeight":"normal","fontStyle":""},"2":{"fill":"red","fontSize":40,"fontFamily":"Helvetica","fontWeight":"normal","fontStyle":""},"3":{"fill":"red","fontSize":50,"fontFamily":"Helvetica","fontWeight":"normal","fontStyle":""},"4":{"fill":"red","fontSize":60,"fontFamily":"Helvetica","fontWeight":"normal","fontStyle":""},"6":{"textBackgroundColor":"yellow"},"7":{"textBackgroundColor":"yellow"},"8":{"textBackgroundColor":"yellow"},"9":{"textBackgroundColor":"yellow","fontFamily":"Helvetica","fontSize":40,"fontWeight":"normal","fontStyle":""}},"1":{"0":{"textDecoration":"underline"},"1":{"textDecoration":"underline","fontFamily":"Helvetica","fontSize":40,"fontWeight":"normal","fontStyle":""},"2":{"fill":"green","fontStyle":"italic","textDecoration":"underline"},"3":{"fill":"green","fontStyle":"italic","textDecoration":"underline"},"4":{"fill":"green","fontStyle":"italic","textDecoration":"underline","fontFamily":"Helvetica","fontSize":40,"fontWeight":"normal"}},"2":{"0":{"fill":"blue","fontWeight":"bold"},"1":{"fill":"blue","fontWeight":"bold"},"2":{"fill":"blue","fontWeight":"bold","fontFamily":"Helvetica","fontSize":40,"fontStyle":""},"4":{"fontFamily":"Courier","textDecoration":"line-through"},"5":{"fontFamily":"Courier","textDecoration":"line-through"},"6":{"fontFamily":"Courier","textDecoration":"line-through"},"7":{"fontFamily":"Courier","textDecoration":"line-through","fontSize":40,"fontWeight":"normal","fontStyle":""}},"3":{"0":{"fontFamily":"Impact","fill":"#666","textDecoration":"line-through"},"1":{"fontFamily":"Impact","fill":"#666","textDecoration":"line-through"},"2":{"fontFamily":"Impact","fill":"#666","textDecoration":"line-through"},"3":{"fontFamily":"Impact","fill":"#666","textDecoration":"line-through"},"4":{"fontFamily":"Impact","fill":"#666","textDecoration":"line-through","fontSize":40,"fontWeight":"normal","fontStyle":""}}}},{"type":"i-text","originX":"left","originY":"top","left":486,"top":343,"width":124.53,"height":157.3,"fill":"#333","stroke":null,"strokeWidth":1,"strokeDashArray":null,"strokeLineCap":"butt","strokeLineJoin":"miter","strokeMiterLimit":10,"scaleX":1,"scaleY":1,"angle":0,"flipX":false,"flipY":false,"opacity":1,"shadow":null,"visible":true,"clipTo":null,"backgroundColor":"","fillRule":"nonzero","globalCompositeOperation":"source-over","transformMatrix":null,"skewX":0,"skewY":0,"text":"foo bar\nbaz\nquux","fontSize":40,"fontWeight":"normal","fontFamily":"Helvetica","fontStyle":"","lineHeight":1.16,"textDecoration":"","textAlign":"left","textBackgroundColor":"","styles":{"0":{"0":{"fill":"red"},"1":{"fill":"red"},"2":{"fill":"red","fontFamily":"Helvetica","fontSize":40,"fontWeight":"normal","fontStyle":""}},"2":{"0":{"fill":"blue"},"1":{"fill":"blue"},"2":{"fill":"blue"},"3":{"fill":"blue","fontFamily":"Helvetica","fontSize":40,"fontWeight":"normal","fontStyle":""}}}},{"type":"rect","originX":"left","originY":"top","left":317.5,"top":342,"width":50,"height":50,"fill":{"type":"linear","coords":{"x1":0,"y1":0,"x2":0,"y2":50},"colorStops":[{"offset":"0","color":"rgb(163,168,82)","opacity":1},{"offset":"1","color":"rgb(49,176,244)","opacity":1}],"offsetX":0,"offsetY":0},"stroke":null,"strokeWidth":1,"strokeDashArray":null,"strokeLineCap":"butt","strokeLineJoin":"miter","strokeMiterLimit":10,"scaleX":2.48,"scaleY":2.48,"angle":0,"flipX":false,"flipY":false,"opacity":1,"shadow":null,"visible":true,"clipTo":null,"backgroundColor":"","fillRule":"nonzero","globalCompositeOperation":"source-over","transformMatrix":null,"skewX":0,"skewY":0,"rx":0,"ry":0},{"type":"circle","originX":"left","originY":"top","left":401,"top":246,"width":80,"height":80,"fill":{"type":"linear","coords":{"x1":0,"y1":0,"x2":80,"y2":0},"colorStops":[{"offset":"0","color":"rgb(49,74,121)","opacity":1},{"offset":"1","color":"rgb(249,168,238)","opacity":1}],"offsetX":0,"offsetY":0},"stroke":null,"strokeWidth":1,"strokeDashArray":null,"strokeLineCap":"butt","strokeLineJoin":"miter","strokeMiterLimit":10,"scaleX":1,"scaleY":1,"angle":0,"flipX":false,"flipY":false,"opacity":0.5,"shadow":null,"visible":true,"clipTo":null,"backgroundColor":"","fillRule":"nonzero","globalCompositeOperation":"source-over","transformMatrix":null,"skewX":0,"skewY":0,"radius":40,"startAngle":0,"endAngle":6.283185307179586},{"type":"text","originX":"left","originY":"top","left":137,"top":32,"width":598.13,"height":367.02,"fill":"#dfea95","stroke":null,"strokeWidth":1,"strokeDashArray":null,"strokeLineCap":"butt","strokeLineJoin":"miter","strokeMiterLimit":10,"scaleX":0.5,"scaleY":0.5,"angle":6,"flipX":false,"flipY":false,"opacity":1,"shadow":null,"visible":true,"clipTo":null,"backgroundColor":"","fillRule":"nonzero","globalCompositeOperation":"source-over","transformMatrix":null,"skewX":0,"skewY":0,"text":"Lorem ipsum dolor sit amet,\nconsectetur adipisicing elit,\nsed do eiusmod tempor incididunt\nut labore et dolore magna aliqua.\nUt enim ad minim veniam,\nquis nostrud exercitation ullamco\nla","fontSize":40,"fontWeight":"","fontFamily":"helvetica","fontStyle":"","lineHeight":1.16,"textDecoration":" underline","textAlign":"left","textBackgroundColor":""},{"type":"path","originX":"center","originY":"center","left":561.5,"top":150.5,"width":183,"height":189,"fill":null,"stroke":"rgb(0, 0, 0)","strokeWidth":1,"strokeDashArray":null,"strokeLineCap":"round","strokeLineJoin":"round","strokeMiterLimit":10,"scaleX":1,"scaleY":1,"angle":0,"flipX":false,"flipY":false,"opacity":1,"shadow":null,"visible":true,"clipTo":null,"backgroundColor":"","fillRule":"nonzero","globalCompositeOperation":"source-over","transformMatrix":null,"skewX":0,"skewY":0,"path":[["M",538.5,95],["Q",538.5,95,539,95],["Q",539.5,95,538.75,95],["Q",538,95,531,99.5],["Q",524,104,522.5,105.5],["Q",521,107,519.5,109],["Q",518,111,517.5,112],["Q",517,113,515.5,117.5],["Q",514,122,514,124],["Q",514,126,514,127.5],["Q",514,129,514.5,130.5],["Q",515,132,515,132.5],["Q",515,133,516,133.5],["Q",517,134,517.5,135],["Q",518,136,519,136.5],["Q",520,137,520,137.5],["Q",520,138,521,138],["Q",522,138,523,138],["Q",524,138,524.5,138],["Q",525,138,525,138.5],["Q",525,139,526,139],["Q",527,139,528,139],["Q",529,139,529.5,139],["Q",530,139,530.5,139],["Q",531,139,531.5,139],["Q",532,139,533,138.5],["Q",534,138,535,137.5],["Q",536,137,536.5,136.5],["Q",537,136,538,135.5],["Q",539,135,539.5,134.5],["Q",540,134,540.5,133.5],["Q",541,133,541.5,132.5],["Q",542,132,542,131.5],["Q",542,131,542,130.5],["Q",542,130,542,129.5],["Q",542,129,542,128.5],["Q",542,128,541.5,128],["Q",541,128,540.5,127.5],["Q",540,127,539.5,127],["Q",539,127,538,127],["Q",537,127,535.5,127],["Q",534,127,533,127.5],["Q",532,128,530.5,129.5],["Q",529,131,528.5,133],["Q",528,135,527.5,136],["Q",527,137,527,138.5],["Q",527,140,527,141.5],["Q",527,143,527,146],["Q",527,149,527.5,150.5],["Q",528,152,528.5,155],["Q",529,158,530.5,161],["Q",532,164,533,165.5],["Q",534,167,534.5,168],["Q",535,169,536.5,172],["Q",538,175,539,176],["Q",540,177,541,178],["Q",542,179,543,179.5],["Q",544,180,544.5,181],["Q",545,182,546,182],["Q",547,182,548,182.5],["Q",549,183,549.5,183.5],["Q",550,184,551,184],["Q",552,184,553,184],["Q",554,184,555,184],["Q",556,184,558,183],["Q",560,182,562.5,179],["Q",565,176,565.5,174],["Q",566,172,566,171],["Q",566,170,566,169],["Q",566,168,566,167.5],["Q",566,167,565.5,166],["Q",565,165,564,164.5],["Q",563,164,562.5,163.5],["Q",562,163,561,163],["Q",560,163,559.5,163],["Q",559,163,558,163.5],["Q",557,164,556.5,164.5],["Q",556,165,555.5,166.5],["Q",555,168,554.5,170],["Q",554,172,553.5,172.5],["Q",553,173,553,175.5],["Q",553,178,553,179],["Q",553,180,553.5,180.5],["Q",554,181,555.5,184.5],["Q",557,188,558.5,191],["Q",560,194,562,196.5],["Q",564,199,565,200.5],["Q",566,202,567.5,203],["Q",569,204,570,205],["Q",571,206,572,207.5],["Q",573,209,576.5,211],["Q",580,213,582,213.5],["Q",584,214,585,214],["Q",586,214,588,214.5],["Q",590,215,592,214.5],["Q",594,214,597,212],["Q",600,210,601,208],["Q",602,206,602.5,204],["Q",603,202,603,199.5],["Q",603,197,603,195],["Q",603,193,602.5,192],["Q",602,191,601,190],["Q",600,189,599.5,189],["Q",599,189,598.5,189],["Q",598,189,597.5,189],["Q",597,189,596.5,189],["Q",596,189,595,190.5],["Q",594,192,593.5,192.5],["Q",593,193,592.5,194.5],["Q",592,196,591.5,198],["Q",591,200,591,202.5],["Q",591,205,591.5,207],["Q",592,209,593,211.5],["Q",594,214,595.5,216],["Q",597,218,598,219.5],["Q",599,221,599.5,221.5],["Q",600,222,601.5,223.5],["Q",603,225,606.5,226],["Q",610,227,613,227],["Q",616,227,617.5,227],["Q",619,227,622,227],["Q",625,227,628,225],["Q",631,223,634,220.5],["Q",637,218,638,216],["Q",639,214,640.5,212],["Q",642,210,642,207.5],["Q",642,205,642.5,201],["Q",643,197,643,192],["Q",643,187,642.5,185.5],["Q",642,184,641.5,182.5],["Q",641,181,640,180],["Q",639,179,639,178.5],["Q",639,178,638,177],["Q",637,176,637,175.5],["Q",637,175,636.5,175],["Q",636,175,635.5,175.5],["Q",635,176,635,176.5],["Q",635,177,635,177.5],["Q",635,178,635,179],["Q",635,180,635,181],["Q",635,182,635,183],["Q",635,184,635,185],["Q",635,186,636,186.5],["Q",637,187,637.5,187.5],["Q",638,188,639.5,189],["Q",641,190,641.5,190],["Q",642,190,643.5,190.5],["Q",645,191,647.5,191],["Q",650,191,651,191],["Q",652,191,653.5,191],["Q",655,191,658,191],["Q",661,191,666,190],["Q",671,189,672,188.5],["Q",673,188,674,187.5],["Q",675,187,675.5,186.5],["Q",676,186,676.5,185],["Q",677,184,677.5,183],["Q",678,182,678,181.5],["Q",678,181,678,179],["Q",678,177,678,174.5],["Q",678,172,676.5,169],["Q",675,166,673,162.5],["Q",671,159,668,154.5],["Q",665,150,661.5,146],["Q",658,142,653,138],["Q",648,134,644,131],["Q",640,128,638,127],["Q",636,126,634.5,125],["Q",633,124,629.5,123.5],["Q",626,123,625,123],["Q",624,123,623.5,123.5],["Q",623,124,623,124.5],["Q",623,125,623,126],["Q",623,127,622.5,127.5],["Q",622,128,622,130],["Q",622,132,622,132.5],["Q",622,133,622,133.5],["Q",622,134,622.5,135.5],["Q",623,137,623.5,138],["Q",624,139,624.5,139],["Q",625,139,625.5,139.5],["Q",626,140,626,140.5],["Q",626,141,626.5,141],["Q",627,141,627.5,141],["Q",628,141,628.5,141],["Q",629,141,630,141],["Q",631,141,631.5,141],["Q",632,141,633,141],["Q",634,141,636,140.5],["Q",638,140,640,138.5],["Q",642,137,643.5,135],["Q",645,133,645.5,131.5],["Q",646,130,646.5,128],["Q",647,126,647,123],["Q",647,120,647,116.5],["Q",647,113,647,111.5],["Q",647,110,646.5,107.5],["Q",646,105,643.5,99],["Q",641,93,639.5,90.5],["Q",638,88,631,78],["Q",624,68,622,66.5],["Q",620,65,617,63.5],["Q",614,62,611,61.5],["Q",608,61,606.5,61],["Q",605,61,604.5,61],["Q",604,61,601.5,61],["Q",599,61,598,62],["Q",597,63,596.5,63.5],["Q",596,64,595,66],["Q",594,68,594,69],["Q",594,70,593.5,71.5],["Q",593,73,593,74.5],["Q",593,76,593,77],["Q",593,78,593,80],["Q",593,82,594,84],["Q",595,86,595.5,86.5],["Q",596,87,597,87.5],["Q",598,88,598.5,88],["Q",599,88,599.5,88.5],["Q",600,89,600.5,89],["Q",601,89,601.5,89],["Q",602,89,603.5,88.5],["Q",605,88,606.5,87.5],["Q",608,87,609.5,85.5],["Q",611,84,612,82.5],["Q",613,81,613.5,80],["Q",614,79,614.5,78],["Q",615,77,615,74],["Q",615,71,615,68.5],["Q",615,66,613.5,62.5],["Q",612,59,610.5,57.5],["Q",609,56,608,55],["Q",607,54,605.5,53.5],["Q",604,53,601,51.5],["Q",598,50,594,48.5],["Q",590,47,586,47],["Q",582,47,576.5,46],["Q",571,45,566.5,45],["Q",562,45,558.5,45],["Q",555,45,553,45],["Q",551,45,549,45.5],["Q",547,46,546,48],["Q",545,50,544.5,50.5],["Q",544,51,543.5,53.5],["Q",543,56,542.5,58.5],["Q",542,61,542,63.5],["Q",542,66,542.5,68.5],["Q",543,71,544,73.5],["Q",545,76,545.5,77.5],["Q",546,79,547,79.5],["Q",548,80,549,80.5],["Q",550,81,550.5,81],["Q",551,81,552,81],["Q",553,81,554,81],["Q",555,81,557,80],["Q",559,79,561,76.5],["Q",563,74,563.5,71.5],["Q",564,69,564,67],["Q",564,65,564,63.5],["Q",564,62,563.5,61],["Q",563,60,561.5,58.5],["Q",560,57,559,55.5],["Q",558,54,557,53],["Q",556,52,553.5,50.5],["Q",551,49,548,47],["Q",545,45,542.5,44],["Q",540,43,537,42],["Q",534,41,528.5,39.5],["Q",523,38,519,38],["Q",515,38,511,38],["Q",507,38,505.5,38],["Q",504,38,503,38],["Q",502,38,501.5,39.5],["Q",501,41,500.5,42.5],["Q",500,44,499,46],["Q",498,48,498,50.5],["Q",498,53,497.5,55],["Q",497,57,497,58],["Q",497,59,496.5,60.5],["Q",496,62,495.5,63.5],["Q",495,65,495,67],["Q",495,69,495,70],["Q",495,71,495,73],["Q",495,75,495.5,77],["Q",496,79,497,80.5],["Q",498,82,500,84.5],["Q",502,87,503,87.5],["Q",504,88,504.5,88.5],["Q",505,89,506.5,90],["Q",508,91,511,92],["Q",514,93,514.5,93],["Q",515,93,516.5,93],["Q",518,93,519,93],["Q",520,93,521,93],["Q",522,93,523,93],["Q",524,93,525,92.5],["Q",526,92,527,91.5],["Q",528,91,528.5,91],["Q",529,91,529,90.5],["L",529,90]],"pathOffset":{"x":586.5,"y":132.5}}],"background":""}'
    );

    $scope.getConsoleJSON = function () {
        return consoleJSONValue;
    };
    $scope.setConsoleJSON = function (value) {
        consoleJSONValue = value;
    };
    $scope.getConsoleSVG = function () {
        return consoleSVGValue;
    };
    $scope.setConsoleSVG = function (value) {
        consoleSVGValue = value;
    };
    $scope.getConsole = function () {
        return consoleValue;
    };
    $scope.setConsole = function (value) {
        consoleValue = value;
    };

    $scope.loadSVGWithoutGrouping = function () {
        _loadSVGWithoutGrouping(consoleSVGValue);
    };
    $scope.loadSVG = function () {
        _loadSVG(consoleSVGValue);
    };

    var _loadSVG = function (svg) {
        fabric.loadSVGFromString(svg, function (objects, options) {
            var obj = fabric.util.groupSVGElements(objects, options);
            canvas.add(obj).centerObject(obj).renderAll();
            obj.setCoords();
        });
    };

    var _loadSVGWithoutGrouping = function (svg) {
        fabric.loadSVGFromString(svg, function (objects) {
            canvas.add.apply(canvas, objects);
            canvas.renderAll();
        });
    };

    $scope.saveJSON = function () {
        //_saveJSON(JSON.stringify(canvas));
        canvas.forEachObject(function (obj) {
            var type = obj.type;

            console.log(obj.left + ' ' + obj.top);
        });

    };

    var _saveJSON = function (json) {
        $scope.setConsoleJSON(json);
        //saveTextAsFile(json);
    };

    function saveTextAsFile(data) {
        var textToWrite = data;//document.getElementById("inputTextToSave").value;
        var textFileAsBlob = new Blob([textToWrite], { type: 'text/plain' });
        var fileNameToSaveAs = "draw.text";//document.getElementById("inputFileNameToSaveAs").value;

        var downloadLink = document.createElement("a");
        downloadLink.download = fileNameToSaveAs;
        downloadLink.innerHTML = "Download File";
        if (window.webkitURL != null) {
            // Chrome allows the link to be clicked
            // without actually adding it to the DOM.
            downloadLink.href = window.webkitURL.createObjectURL(textFileAsBlob);
        }
        else {
            // Firefox requires the link to be added to the DOM
            // before it can be clicked.
            downloadLink.href = window.URL.createObjectURL(textFileAsBlob);
            downloadLink.onclick = destroyClickedElement;
            downloadLink.style.display = "none";
            document.body.appendChild(downloadLink);
        }

        downloadLink.click();
    }

    $scope.loadJSON = function () {
        var value = '{"objects":[{"type":"path-group","originX":"left","originY":"top","left":5,"top":5,"width":48,"height":48,"fill":"","stroke":null,"strokeWidth":1,"strokeDashArray":null,"strokeLineCap":"butt","strokeLineJoin":"miter","strokeMiterLimit":10,"scaleX":1,"scaleY":1,"angle":0,"flipX":false,"flipY":false,"opacity":0.8,"shadow":null,"visible":true,"clipTo":null,"backgroundColor":"","fillRule":"nonzero","globalCompositeOperation":"source-over","transformMatrix":null,"paths":[{"type":"path","originX":"left","originY":"top","left":3,"top":8,"width":19,"height":8,"fill":"#000000","stroke":null,"strokeWidth":1,"strokeDashArray":null,"strokeLineCap":"butt","strokeLineJoin":"miter","strokeMiterLimit":10,"scaleX":1,"scaleY":1,"angle":0,"flipX":false,"flipY":false,"opacity":1,"shadow":null,"visible":true,"clipTo":null,"backgroundColor":"","fillRule":"nonzero","globalCompositeOperation":"source-over","transformMatrix":[2,0,0,2,0,0],"path":[["M",22,12],["l",-4,-4],["v",3],["H",3],["v",2],["h",15],["v",3],["Z"]],"pathOffset":{"x":12.5,"y":12}},{"type":"path","originX":"left","originY":"top","left":0,"top":0,"width":24,"height":24,"fill":"","stroke":null,"strokeWidth":1,"strokeDashArray":null,"strokeLineCap":"butt","strokeLineJoin":"miter","strokeMiterLimit":10,"scaleX":1,"scaleY":1,"angle":0,"flipX":false,"flipY":false,"opacity":1,"shadow":null,"visible":true,"clipTo":null,"backgroundColor":"","fillRule":"nonzero","globalCompositeOperation":"source-over","transformMatrix":[2,0,0,2,0,0],"path":[["M",0,0],["h",24],["v",24],["H",0],["Z"]],"pathOffset":{"x":12,"y":12}}]}],"background":""}';
        _loadJSON(value);

        //$scope.addRect(60,60);
    };

    var _loadJSON = function (json) {
        canvas.loadFromJSON(json, function () {
            canvas.renderAll();
        });
    };



    function initCustomization() {
        if (typeof Cufon !== 'undefined' && Cufon.fonts.delicious) {
            Cufon.fonts.delicious.offsetLeft = 75;
            Cufon.fonts.delicious.offsetTop = 25;
        }

        if (/(iPhone|iPod|iPad)/i.test(navigator.userAgent)) {
            fabric.Object.prototype.cornerSize = 30;
        }

        fabric.Object.prototype.transparentCorners = false;

        if (document.location.search.indexOf('guidelines') > -1) {
            initCenteringGuidelines(canvas);
            initAligningGuidelines(canvas);
        }
    }

    initCustomization();

    //addTexts();


    $scope.getFreeDrawingMode = function () {
        return canvas.isDrawingMode;
    };
    $scope.setFreeDrawingMode = function (value) {
        canvas.isDrawingMode = !!value;
        $scope.$$phase || $scope.$digest();
    };

    $scope.freeDrawingMode = 'Pencil';

    $scope.getDrawingMode = function () {
        return $scope.freeDrawingMode;
    };
    $scope.setDrawingMode = function (type) {
        $scope.freeDrawingMode = type;

        if (type === 'hline') {
            canvas.freeDrawingBrush = $scope.vLinePatternBrush;
        }
        else if (type === 'vline') {
            canvas.freeDrawingBrush = $scope.hLinePatternBrush;
        }
        else if (type === 'square') {
            canvas.freeDrawingBrush = $scope.squarePatternBrush;
        }
        else if (type === 'diamond') {
            canvas.freeDrawingBrush = $scope.diamondPatternBrush;
        }
        else if (type === 'texture') {
            canvas.freeDrawingBrush = $scope.texturePatternBrush;
        }
        else {
            canvas.freeDrawingBrush = new fabric[type + 'Brush'](canvas);
        }

        $scope.$$phase || $scope.$digest();
    };

    $scope.getDrawingLineWidth = function () {
        if (canvas.freeDrawingBrush) {
            return canvas.freeDrawingBrush.width;
        }
    };
    $scope.setDrawingLineWidth = function (value) {
        if (canvas.freeDrawingBrush) {
            canvas.freeDrawingBrush.width = parseInt(value, 10) || 1;
        }
    };

    $scope.getDrawingLineColor = function () {
        if (canvas.freeDrawingBrush) {
            return canvas.freeDrawingBrush.color;
        }
    };
    $scope.setDrawingLineColor = function (value) {
        if (canvas.freeDrawingBrush) {
            canvas.freeDrawingBrush.color = value;
        }
    };

    $scope.getDrawingLineShadowWidth = function () {
        if (canvas.freeDrawingBrush && canvas.freeDrawingBrush.shadow) {
            return canvas.freeDrawingBrush.shadow.blur || 1;
        }
        else {
            return 0
        }
    };
    $scope.setDrawingLineShadowWidth = function (value) {
        if (canvas.freeDrawingBrush) {
            var blur = parseInt(value, 10) || 1;
            if (blur > 0) {
                canvas.freeDrawingBrush.shadow = new fabric.Shadow({ blur: blur, offsetX: 10, offsetY: 10 });
            }
            else {
                canvas.freeDrawingBrush.shadow = null;
            }
        }
    };

    function initBrushes() {
        if (!fabric.PatternBrush) return;

        initVLinePatternBrush();
        initHLinePatternBrush();
        initSquarePatternBrush();
        initDiamondPatternBrush();
        initImagePatternBrush();
    }
    initBrushes();

    function initImagePatternBrush() {
        var img = new Image();
        img.src = '../assets/honey_im_subtle.png';

        $scope.texturePatternBrush = new fabric.PatternBrush(canvas);
        $scope.texturePatternBrush.source = img;
    }

    function initDiamondPatternBrush() {
        $scope.diamondPatternBrush = new fabric.PatternBrush(canvas);
        $scope.diamondPatternBrush.getPatternSrc = function () {

            var squareWidth = 10, squareDistance = 5;
            var patternCanvas = fabric.document.createElement('canvas');
            var rect = new fabric.Rect({
                width: squareWidth,
                height: squareWidth,
                angle: 45,
                fill: this.color
            });

            var canvasWidth = rect.getBoundingRectWidth();

            patternCanvas.width = patternCanvas.height = canvasWidth + squareDistance;
            rect.set({ left: canvasWidth / 2, top: canvasWidth / 2 });

            var ctx = patternCanvas.getContext('2d');
            rect.render(ctx);

            return patternCanvas;
        };
    }

    function initSquarePatternBrush() {
        $scope.squarePatternBrush = new fabric.PatternBrush(canvas);
        $scope.squarePatternBrush.getPatternSrc = function () {

            var squareWidth = 10, squareDistance = 2;

            var patternCanvas = fabric.document.createElement('canvas');
            patternCanvas.width = patternCanvas.height = squareWidth + squareDistance;
            var ctx = patternCanvas.getContext('2d');

            ctx.fillStyle = this.color;
            ctx.fillRect(0, 0, squareWidth, squareWidth);

            return patternCanvas;
        };
    }

    function initVLinePatternBrush() {
        $scope.vLinePatternBrush = new fabric.PatternBrush(canvas);
        $scope.vLinePatternBrush.getPatternSrc = function () {

            var patternCanvas = fabric.document.createElement('canvas');
            patternCanvas.width = patternCanvas.height = 10;
            var ctx = patternCanvas.getContext('2d');

            ctx.strokeStyle = this.color;
            ctx.lineWidth = 5;
            ctx.beginPath();
            ctx.moveTo(0, 5);
            ctx.lineTo(10, 5);
            ctx.closePath();
            ctx.stroke();

            return patternCanvas;
        };
    }

    function initHLinePatternBrush() {
        $scope.hLinePatternBrush = new fabric.PatternBrush(canvas);
        $scope.hLinePatternBrush.getPatternSrc = function () {

            var patternCanvas = fabric.document.createElement('canvas');
            patternCanvas.width = patternCanvas.height = 10;
            var ctx = patternCanvas.getContext('2d');

            ctx.strokeStyle = this.color;
            ctx.lineWidth = 5;
            ctx.beginPath();
            ctx.moveTo(5, 0);
            ctx.lineTo(5, 10);
            ctx.closePath();
            ctx.stroke();

            return patternCanvas;
        };
    }


}

//取得樣式
function getActiveStyle(styleName, object) {
    object = object || canvas.getActiveObject();
    if (!object) {
        return '';
    }
    return (object.getSelectionStyles && object.isEditing)
      ? (object.getSelectionStyles()[styleName] || '')
      : (object[styleName] || '');
};
//設定樣式
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

function getActiveProp(name) {
    var object = canvas.getActiveObject();
    if (!object) {
        return '';
    }
    return object[name] || '';
}

function setActiveProp(name, value) {
    var object = canvas.getActiveObject();
    if (!object) {
        return;
    }
    object.set(name, value).setCoords();
    canvas.renderAll();
}
