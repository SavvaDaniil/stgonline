class LessonFacade {


    async searchByFilter_LessonsFilterViewModel(_csrf, functionCallBack, offset = 0, id_of_level = 0){

        return $.ajax({
            method: 'POST',
            type: "POST",
            url:  "/api/lesson/search",
            data: form,
            cashe: false,
            processData: false,
            contentType: false,
            error:function(){
                return null;
            },
            success: function(html)
            {
                if (html["lessons"] !== null && typeof (html["lessons"]) !== "undefined") {
                    const previewVideo1Factory = new PreviewVideo1Factory();

                    const lessons = html["lessons"];
                    for (var i = 0; i < lessons.length; i++) {
                        document.getElementById("divListLessons").appendChild(
                            previewVideo1Factory.createAndGetHtml(
                                lessons[i]["id"],
                                lessons[i]["name"],
                                lessons[i]["teacherName"],
                                lessons[i]["styleName"],
                                lessons[i]["video"],
                                lessons[i]["lessonTypeName"],
                                lessons[i]["levelName"],
                                lessons[i]["active"],
                                lessons[i]["posterSrc"],
                                lessons[i]["teaserSrc"]
                            )
                        );
                    }
                    return 

                    
                } else {
                    functionCallBack(false, null);
                }
            }
        })
        .then(response => response.data);
    }
}