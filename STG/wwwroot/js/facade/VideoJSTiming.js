

class VideoJSTiming {

    divTimingControl(value){
        if(value){
            document.getElementsByClassName("div_timing")[0].classList.add("active");
            document.getElementsByClassName("btn_open_timing")[0].classList.remove("active");
        } else {
            document.getElementsByClassName("div_timing")[0].classList.remove("active");
            document.getElementsByClassName("btn_open_timing")[0].classList.add("active");
        }
    }


    bulidBtnTiming(player){
        var btnForOpenTiming = player.addChild('button');
        btnForOpenTiming.addClass("btn_open_timing");
        btnForOpenTiming.addClass("active");
        var elementDom = btnForOpenTiming.el();
        var htmlContent = '<p>Тайминг</p>';
        elementDom.innerHTML = htmlContent;

        btnForOpenTiming.on('click', function(){
            var videoJSTiming = new VideoJSTiming();
            videoJSTiming.divTimingControl(true);
        });
    }


    buildTiming(player, name_of_lesson, jsonViewModel){
        
        var divForTiming = player.addChild('button');
        divForTiming.addClass("div_timing");
        var elementDom = divForTiming.el();
        elementDom.innerHTML = this.buildTimingHeader(name_of_lesson) + this.divTimingList(jsonViewModel);
        
    }

    buildTimingMobile(el, jsonViewModel) {
        el.innerHTML = "";
        el.innerHTML = this.divTimingList(jsonViewModel);
    }

    buildTimingHeader(name_of_lesson){
        return '<div class="divTimingHeader"><div><h5>Тайминг</h5><button type="button" onclick="closeDivTiming()"><i class="fa fa-arrow-right" aria-hidden="true"></i></button></div></div>';
    }

    divTimingList(jsonViewModel){
        var contentHtml = "";

        if(jsonViewModel["videosectionList"] !== null && typeof(jsonViewModel["videosectionList"]) !== "undefined"){
            var videosectionList = jsonViewModel["videosectionList"];
            contentHtml = "<ul>";
            var contentLi;
            var newTimeLine = 0;
            var timing_minutes_str = "";
            var timing_seconds_str = "";

            for(var i = 0; i < videosectionList.length; i++){
                contentLi = '<li class="section">' + videosectionList[i]["name"];

                if(videosectionList[i]["videosubsectionList"] !== null && typeof(videosectionList[i]["videosubsectionList"]) !== "undefined"){
                    if(videosectionList[i]["videosubsectionList"].length > 0){
                        var videosubsectionList = videosectionList[i]["videosubsectionList"];
                        contentLi += '<ul>';
                        for (var s = 0; s < videosubsectionList.length; s++) {
                            newTimeLine = videosubsectionList[s]["timing_minutes"] * 60 + videosubsectionList[s]["timing_seconds"];
                            timing_minutes_str = ((videosubsectionList[s]["timing_minutes"]).toString().length == 1 ? "0" + videosubsectionList[s]["timing_minutes"] : videosubsectionList[s]["timing_minutes"]);
                            timing_seconds_str = ((videosubsectionList[s]["timing_seconds"]).toString().length == 1 ? "0" + videosubsectionList[s]["timing_seconds"] : videosubsectionList[s]["timing_seconds"]);

                            contentLi += '<li><a href="#" onclick="changeTimecode(' + newTimeLine + ')"><i class="fa fa-arrow-right" aria-hidden="true"></i> '
                                + timing_minutes_str + ':'
                                + timing_seconds_str + ' - '
                                + videosubsectionList[s]["name"] + '</a></li>';
                        }
                        contentLi += '</ul>';
                    }
                }

                contentLi += '</li>';

                contentHtml += contentLi;
            }
            contentHtml += "</ul>";
        }

        return contentHtml;
    }


}