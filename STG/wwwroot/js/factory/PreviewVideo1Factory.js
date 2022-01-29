class PreviewVideo1Factory {

    //createAndGetHtml(1,"Maksim Bon", "", "", "", "", "", 0, null, null);

    createAndGetHtml(id, name, teacher, style, video, lessonType, level, active, posterSrc, teaserSrc) {
        let previewVideo1 = new PreviewVideo1(id, name, teacher, style, video, lessonType, level, active, posterSrc, teaserSrc);
        
        

        let imageBgd = document.createElement("img");
        imageBgd.className = "img-fluid";
        if (posterSrc !== null && typeof (posterSrc) !== "undefined") {
            imageBgd.src = posterSrc;
        } else {
            imageBgd.src = "/images/preview_default.jpg";
        }







        //contentHtml.appendChild(divIconControl);

        let divDescription = document.createElement("div");
        divDescription.className = "description";


        let pType = document.createElement("p");
        pType.className = "preview-video-lesson-type";
        pType.innerHTML = lessonType;
        divDescription.appendChild(pType);

        let pNameTitle = document.createElement("p");
        pNameTitle.className = "preview-video-title";
        pNameTitle.innerHTML = name;

        if (name != null && typeof (name) != "undefined") {
            if (name.length >= 15) {
                pNameTitle.classList.add("short");
            }
            if (name.length >= 20) {
                pNameTitle.classList.add("shorter");
            }
        }

        divDescription.appendChild(pNameTitle);
        /*
        let divName = document.createElement("div");
        divName.className = "name";
        divName.innerHTML = name;
        divDescription.appendChild(divName);

        if (style !== null && typeof (style) !== "undefined") {
            let spanStyle = document.createElement("span");
            spanStyle.innerHTML = style;
            let pStyle = document.createElement("p");
            pStyle.innerHTML = "Стиль: ";
            pStyle.appendChild(spanStyle);
            divDescription.appendChild(pStyle);
        }
        if (teacher !== null && typeof (teacher) !== "undefined") {
            let spanTeacher = document.createElement("span");
            spanTeacher.innerHTML = teacher;
            let pTeacher = document.createElement("p");
            pTeacher.innerHTML = "Преподаватель: ";
            pTeacher.appendChild(spanTeacher);
            divDescription.appendChild(pTeacher);
        }
        if (lessonType !== null && typeof (lessonType) !== "undefined") {
            let spanLessonType = document.createElement("span");
            spanLessonType.innerHTML = lessonType;
            let pLessonType = document.createElement("p");
            pLessonType.innerHTML = "Вид занятия: ";
            pLessonType.appendChild(spanLessonType);
            divDescription.appendChild(pLessonType);
        }
        */
        


        let divLeftMouseOut = document.createElement("div");
        divLeftMouseOut.className = "left mouse_out";
        divLeftMouseOut.id = "mouse_out" + id;

        //set level
        /*
        if (level !== null && typeof (level) !== "undefined") {
            let divLevel = document.createElement("div");
            divLevel.className = "level";
            divLevel.innerHTML = level;
            
            switch (level) {
                case "Beginners":
                    divLevel.classList.add("beginners");
                    break;
                case "Intermediate":
                    divLevel.classList.add("intermediate");
                    break;
                case "Advanced":
                    divLevel.classList.add("advanced");
                    break;
                default:
                    break;
            }
            divLeftMouseOut.appendChild(divLevel);
        }
        */


        //isActive
        if (active != true) {

            let divLock = document.createElement("div");
            divLock.className = "lock single";

            let iLock = document.createElement("i");
            iLock.setAttribute('aria-hidden', 'true');
            iLock.className = "fa fa-lock";

            divLock.appendChild(iLock);

            divLeftMouseOut.appendChild(divLock);
        } else {
            //let btnPlay = document.createElement("span");
            //divLock.classList.add("play");
            //divLock.innerHTML = "Play";
            //divLock.appendChild(btnPlay);
        }
       

        divLeftMouseOut.appendChild(divDescription);

        if (teacher !== null && typeof (teacher) !== "undefined") {
            let pTeacher = document.createElement("p");
            pTeacher.className = "preview-video-teacher";
            pTeacher.innerHTML = teacher;
            divLeftMouseOut.appendChild(pTeacher);
        }






        //divMouseOver START

        let iVolumeOff = document.createElement("i");
        iVolumeOff.setAttribute('aria-hidden', 'true');
        iVolumeOff.className = "fa fa-volume-up";

        let aVolumeOff = document.createElement("a");
        aVolumeOff.href = "#";
        aVolumeOff.title = "Выключить звук";
        aVolumeOff.id = "volumeOn_" + id;
        aVolumeOff.className = "hide";
        aVolumeOff.onclick = function () {
            videoControlSound(id, 0);
        }
        aVolumeOff.appendChild(iVolumeOff);


        let iVolumeOn = document.createElement("i");
        iVolumeOn.setAttribute('aria-hidden', 'true');
        iVolumeOn.className = "fa fa-volume-off";

        let aVolumeOn = document.createElement("a");
        aVolumeOn.href = "#";
        aVolumeOn.title = "Включить звук";
        aVolumeOn.id = "volumeOff_" + id;
        aVolumeOn.onclick = function () {
            videoControlSound(id, 1);
        }
        aVolumeOn.appendChild(iVolumeOn);

        let divIconControl = document.createElement("div");
        divIconControl.className = "icon-control";
        divIconControl.appendChild(aVolumeOn);
        divIconControl.appendChild(aVolumeOff);

            //inFavourite 
            let iHeartNot = document.createElement("i");
            iHeartNot.setAttribute('aria-hidden', 'true');
            iHeartNot.className = "fa fa-heart-o";
            let aFavourite = document.createElement("a");
            aFavourite.href = "#";
            aFavourite.title = "В избранное";
            aFavourite.appendChild(iHeartNot);
            let divIconControlFirst = document.createElement("div");
            divIconControlFirst.className = "icon-control first";
            divIconControlFirst.appendChild(aFavourite);

            //play button
            let iPlay = document.createElement("i");
            iPlay.setAttribute('aria-hidden', 'true');
            iPlay.className = "fa fa-lock";
            let divPlay = document.createElement("div");
            divPlay.className = "play";
            divPlay.appendChild(iPlay);
            divPlay.innerHTML += " Play";
            

        let divLeftMouseOver = document.createElement("div");
        divLeftMouseOver.className = "left mouse_over";
        divLeftMouseOver.id = "mouse_over" + id;
        divLeftMouseOver.appendChild(divPlay);
        divLeftMouseOver.appendChild(divIconControlFirst);
        divLeftMouseOver.appendChild(divIconControl);


        //divMouseOver END



        

        let divVideoContainer = document.createElement("div");
        divVideoContainer.className = "video-container";

        //isVideo
        //teaserSrc = "/uploads/lesson/1/teaser.mp4";
        /*
        if (teaserSrc !== null && typeof (teaserSrc) !== "undefined") {
            divVideoContainer.onmouseover = function () { control_video(id, 1); }
            divVideoContainer.onmouseout = function () { control_video(id, 0); }

            let videoBlock = document.createElement("video");
            if (posterSrc !== null && typeof (posterSrc) !== "undefined") {
                videoBlock.poster = posterSrc;
            } else {
                videoBlock.poster = "/images/preview_default.jpg";
            }
            videoBlock.id = "preview" + id;
            videoBlock.width = 300;
            videoBlock.height = 300;
            videoBlock.muted = true;
            videoBlock.preload = "none";
            let sourceForVideo = document.createElement("source");
            sourceForVideo.src = teaserSrc;
            sourceForVideo.type = 'video/mp4; codecs="avc1.42E01E, mp4a.40.2"';
            videoBlock.appendChild(sourceForVideo);

            divVideoContainer.appendChild(videoBlock);
        }
        */

        divVideoContainer.appendChild(divLeftMouseOut);
        divVideoContainer.appendChild(divLeftMouseOver);
        divVideoContainer.appendChild(imageBgd);

        let divPreviewVideo = document.createElement("div");
        divPreviewVideo.className = "preview_video_1 hide-opacity";
        divPreviewVideo.appendChild(divVideoContainer);

        imageBgd.onload = function () {
            divPreviewVideo.classList.remove("hide-opacity");
        }

        let aLink = document.createElement("a");
        //aLink.href = "/lesson/"+id;
        aLink.appendChild(divPreviewVideo);
        aLink.onclick = function () {
            globalCheckAccess(id);
        }
        /*
        let contentHtml = document.createElement("div");
        contentHtml.className = "col-12 col-lg-4 col-md-6 col-sm-12 row";
        contentHtml.appendChild(aLink);
        */

        return aLink;
    }


}