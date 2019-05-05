var floormattype = "topinsert";
var contentdiv;
var currentstate;

function setupFloormat(type, content) {

    floormattype = type;  
    contentdiv = content;   

    if (floormattype=="overlay") {
        content.append("<a href='javascript:void(0)' class='closebtn' onclick='hideFloormat()'>&times;</a>");

        var $floormat = $("<div id='floormat' class='overlay'></div>");
        var $floormatcontent = $("<div id='floormatcontent'></div>");
        $("body").append($floormat);
        $floormat.append($floormatcontent);
        $floormatcontent.append(content);
    } else if (floormattype == "topinsert") {
        var $floormat = $("<div id='floormat' class='topinsert'></div>");
        var $floormatcontent = $("<div id='floormatcontent' class='contentwrapper'></div>");
        $("body").prepend($floormat);
        $floormat.append($floormatcontent);
        $floormatcontent.append(content);
    } else if (floormattype = "zoomin") {
        content.append("<a href='javascript:void(0)' class='closebtn' onclick='hideFloormat()'>&times;</a>");
        var $floormatoverlay = $("<div id='floormatoverlay' class='overlay-zoomin-start-bg' style='z-index:1;'></div>");
        var $floormat = $("<div id='floormat' style='height:100%; width:100%; overflow-y: auto; position:fixed; top:0px; left:0px; z-index:100;'></div>");
        var $floormatcontent = $("<div id='floormatcontent' class='overlay-zoomin-start'></div>");
        $("body").append($floormatoverlay);
        $("body").append($floormat);
        $floormat.append($floormatcontent);
        //$("body").append($floormatcontentwrapper);
        $floormatcontent.append(content);
    }
};

/* Reference URL: http://stackoverflow.com/questions/15191058/css-rotation-cross-browser-with-jquery-animate */
$.fn.animateRotate = function (angle, duration, easing, complete) {
    var args = $.speed(duration, easing, complete);
    var step = args.step;
    return this.each(function (i, e) {
        args.complete = $.proxy(args.complete, e);
        args.step = function (now) {
            $.style(e, 'transform', 'rotate(' + now + 'deg)');
            if (step) return step.apply(e, arguments);
        };

        $({ deg: 0 }).animate({ deg: angle }, args);
    });
};

function showFloormat(backgroundimagepath) {
    currentstate = "open";
    if (floormattype == "overlay") {
        $("#floormat").height("100%");
        /*
        $("#floormatcontent").animateRotate(10, {
            duration: 1337,
            easing: 'linear',
            complete: function () {},
            step: function () { }
        });
        */
    } else if (floormattype == "topinsert") {
        $("#floormat").css("transition", "2s");
        if (contentdiv.height() < $(window).height())
            $("#floormat").height($(window).height());
        else
            $("#floormat").height(contentdiv.height());
    } else if (floormattype == "zoomin") {
        $("#floormat").toggleClass("overlay-zoomin-ready-bg");
        $("#floormatcontent").toggleClass("overlay-zoomin-ready");
    }

    if (backgroundimagepath != null && backgroundimagepath != "") { 
        $("#floormat").css("background-image", "url('" + backgroundimagepath + "')"); 
        $("#floormat").css("background-size", "cover");
    }
}

function hideFloormat() {
    currentstate = "close";
    if (floormattype == "overlay") {
        $("#floormat").height("0%");
    } else if (floormattype == "zoomin") {
        $("#floormat").toggleClass("overlay-zoomin-close-bg");
        $("#floormatcontent").toggleClass("overlay-zoomin-close");
    } if (floormattype = "topinsert") {
        $("#floormat").css("transition", "2s");
        $("#floormat").height(0);
    }
}

$(window).resize(function () {
    if(currentstate == "open") {
        if (floormattype == "topinsert") { 
            $("#floormat").css("transition", "none");
            if (contentdiv.height() < $(window).height())
                $("#floormat").height($(window).height());
            else
                $("#floormat").height(contentdiv.height());
        }
    }
})

