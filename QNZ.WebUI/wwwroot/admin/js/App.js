$(function() {

    var resetwidth = function () {

        var winwidth = $(window).width();
        // alert(winwidth)


        if (winwidth <= 768) {
            $('#rightcol').css({ 'width': winwidth + "px" })
        } else {
            $('#rightcol').css({ 'width': (winwidth - 170) + "px" })
        }
    };

    resetwidth();

    $(window).resize(function() {
        resetwidth();
    });

    $('.mainmenu li.active').closest('li.down-nav').addClass('nav-open');
    var pid = $('.mainmenu li.active').attr("data-parent");
    $('.mainmenu li[data-parent=' + pid + ']').fadeIn();

    //查子项数量
    $.each($(".down-nav>a"), function(index, value) {
        var id = $(value).attr("data-id");
        var licount = $('.mainmenu li[data-parent=' + id + ']').length;
        var html = '<i class="iconfont icon-right haschild"></i>';
        $(value).append(html);
    });

    $(".down-nav>a").click(function(e) {
        e.preventDefault();

        $(this).next(".submenu").slideToggle();
        var li = $(this).closest('li');
        li.toggleClass('nav-open');

    });


    $('#closemenu').on('click', function (e) {
        $(this).toggleClass("openav");
        var marginLeft = $('#rightcol').css("margin-left");    
        if (!$(this).hasClass("openav")) {
            opennav();
        } else {
            closenav();
        }

        e.preventDefault();
    });

    var closenav = function () {
        var winwidth = $(window).width();
        $('#rightcol').animate({ 'marginLeft': '-170' }, 'fast');
        $('#rightcol').css({ 'width': winwidth + "px" });
        $('#wrapper').addClass("nonav");

    };
    var opennav = function () {
        var winwidth = $(window).width();
        $('#rightcol').animate({ 'marginLeft': '0' }, 'fast');
        $('#rightcol').css({ 'width': (winwidth - 170) + "px" });
        $('#wrapper').removeClass("nonav");

    };



    $('a.expand').click(function(e) {
        $(this).closest('.box').addClass('box-fixed');
        $(this).hide();
        $(this).next('a').show()
        e.preventDefault();
    });
    $('a.compress').click(function(e) {
        $(this).closest('.box').removeClass('box-fixed');
        $(this).hide();
        $(this).prev('a').show()
        e.preventDefault();
    });
});



var Common = {
    //消息提示
    ShowBox: function(status, message, title) {
        switch (status) {
            case 1:
                toastr.success(message, title)
                break;
            case 2:
                toastr.error(message, title)
                break;
            case 3:
                toastr.info(message, title)
                break;
            case 4:
                toastr.warning(message, title)
        }
    },
    ShowBoxWithFunc: function(data, title, func) {

        switch (data.status) {
            case 1:
                toastr.success(data.message, title)
                func();
                break;
            case 2:
                toastr.error(data.message, title)
                break;
            case 3:
                toastr.info(data.message, title)
                break;
            case 4:
                toastr.warning(data.message, title)
        }
    },
    ShowBoxWithFuncBack: function(data, title, func) {

        switch (data.status) {
            case 1:
                toastr.success(data.message, title);
                func(data.id, data.data);
                break;
            case 2:
                toastr.error(data.message, title);
                break;
            case 3:
                toastr.info(data.message, title);
                break;
            case 4:
                toastr.warning(data.message, title);
        }
    },
    SubmitBack: function(data, title, container) {

        switch (data.status) {
            case 1:
                toastr.success(data.message, title)
                if (container !== undefined)
                    container.html(data.Data)
                break;
            case 2:
                toastr.error(data.message, title)
                break;
            case 3:
                toastr.info(data.message, title)
                break;
            case 4:
                toastr.warning(data.message, title)
        }
    },
    PageSizeSet: function(url, title, pageSize, func) { //分页设置

        $.post(url, { pageSize: pageSize }, function(data) {

            switch (data.status) {
                case 1:
                    //toastr.success(data.message, title);
                    func();
                    break;
                case 2:
                    toastr.error(data.message, title);
                    break;
                case 3:
                    toastr.info(data.message, title);
                    break;
                case 4:
                    toastr.warning(data.message, title);
                    break;
            }
        });
    },

    SingleActionWithFunc: function(url, title, that, func) { //真假值修改操作

        $.post(url, $("#anti-form").serialize(), function(data) {

            switch (data.status) {
                case 1:
                    toastr.success(data.message, title);
                    func(that);
                    break;
                case 2:
                    toastr.error(data.message, title);
                    break;
                case 3:
                    toastr.info(data.message, title);
                    break;
                case 4:
                    toastr.warning(data.message, title);
                    break;
            }
        });
    },
    SingleActionWithFuncBack: function(url, title, that, func) { //真假值修改操作

        $.post(url, $("#anti-form").serialize(), function(data) {

            switch (data.status) {
                case 1:
                    toastr.success(data.message, title);
                    func(that, data.Data);
                    break;
                case 2:
                    toastr.error(data.message, title);
                    break;
                case 3:
                    toastr.info(data.message, title);
                    break;
                case 4:
                    toastr.warning(data.message, title);
                    break;
            }
        });
    },

    SingleAction: function(url, title, isTips) { //真假值修改操作
        $.post(url, $("#anti-form").serialize(), function(data) {
            if (!isTips)
                return;

            switch (data.status) {
                case 1:
                    toastr.success(data.message, title);
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                    break;
                case 2:
                    toastr.error(data.message, title);
                    break;
                case 3:
                    toastr.info(data.message, title);
                    break;
                case 4:
                    toastr.warning(data.message, title);
                    break;
            }
        });
    },



    DeleteItem: function(url, title, that) { //删除

        $.post(url, $("#anti-form").serialize(), function(data) {
            switch (data.status) {
                case 1:
                    toastr.success(data.message, title);
                    that.closest('.item-container').remove();
                    break;
                case 2:
                    toastr.error(data.message, title);
                    break;
                case 3:
                    toastr.info(data.message, title);
                    break;
                case 4:
                    toastr.warning(data.message, title);
                    break;
            }
        });
    }


};

var singleEelFinder = {
    percent: 70,
    baseUrl: "/plugins/elFinder/elfinder-single.html",
    selectActionFunction: null,
    elFinderCallback: function(fileUrl) {
        this.selectActionFunction(fileUrl);
    },
    open: function() {
        var w = 800,
            h = 600; // default sizes
        if (window.screen) {
            w = window.screen.width * this.percent / 100;
            h = window.screen.height * this.percent / 100;
        }
        var x = screen.width / 2 - w / 2;
        var y = screen.height / 2 - h / 2;

        window.open(this.baseUrl, "_blank", 'height=' + h + ',width=' + w + ',left=' + x + ',top=' + y);
    }
};



var QNZ = {
    ImagesUploadHandler: function (blobInfo, success, failure) {
        var xhr, formData;

        xhr = new XMLHttpRequest();
        xhr.withCredentials = false;
        xhr.open('POST', '/Admin/Files/ImageUpload');

        xhr.onload = function () {
            var json;

            if (xhr.status != 200) {
                failure('HTTP Error: ' + xhr.status);
                return;
            }

            json = JSON.parse(xhr.responseText);

            if (!json || typeof json.location != 'string') {
                failure('Invalid JSON: ' + xhr.responseText);
                return;
            }

            success(json.location);
        };


        var description = '';

        jQuery(tinymce.activeEditor.dom.getRoot()).find('img').not('.loaded-before').each(
            function () {
                description = $(this).attr("alt");
                $(this).addClass('loaded-before');
            });

        formData = new FormData();
        formData.append('file', blobInfo.blob(), blobInfo.filename());
        formData.append('description', description); //found now))

        xhr.send(formData);
    },


    percent: 70,
    baseUrl: "/Admin/QNZFinder/SingleFinder",
    selectActionFunction: null,
    elFinderCallback: function (fileUrl) {
        this.selectActionFunction(fileUrl);
    },
    open: function () {
        var w = 800,
            h =600; // default sizes
        if (window.screen) {
            w = window.screen.width * this.percent / 100;
            h = window.screen.height * this.percent / 100;
        }
        var x = screen.width / 2 - w / 2;
        var y = screen.height / 2 - h / 2;

        window.open(this.baseUrl, "_blank", 'height=' + h + ',width=' + w + ',left=' + x + ',top=' + y);
    },

    FilePickerCallback2: function (callback, value, meta) {
        // Provide file and text for the link dialog
        // if (meta.filetype == 'file') {
        //   callback('mypage.html', {text: 'My text'});
        // }

        // // Provide image and alt text for the image dialog
        //if (meta.filetype == 'image') {

        //   callback('myimage.jpg', {alt: 'My alt text'});
        // }

        // // Provide alternative source and posted for the media dialog
        // if (meta.filetype == 'media') {
        //   callback('movie.mp4', {source2: 'alt.ogg', poster: 'image.jpg'});
        // }
        var finderUrl = '/Admin/QNZFinder/FinderForTinyMce';
        tinyMCE.activeEditor.windowManager.openUrl({
            url: finderUrl,
            title: 'QNZFinder 1.0 文件管理',
            width: 960,
            height: 700
            // onMessage: function (api, data) {
            //     if (data.mceAction === 'FileSelected') {
            //        callback(data.url);
            //        api.close();
            //    }
            //}
        });

        window.addEventListener('message', function (event) {
            var data = event.data;
            callback(data.content);
        });

    }

};

//var singleEelFinder = {
   
//};