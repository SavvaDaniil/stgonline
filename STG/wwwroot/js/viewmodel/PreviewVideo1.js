
class PreviewVideo1 {
    constructor(id, name, teacher, style, video, lessonType, level, active, posterSrc, teaserPath) {
        this._id = id;
        this._name = name;
        this._teacher = teacher;
        this._style = style;
        this._video = video;
        this._lessonType = lessonType;
        this._active = active;
        this._posterSrc = posterSrc;
        this._teaserPath = teaserPath;
    }
    setHtml(contentHtml) {
        this._contentHtml = contentHtml;
    }
}