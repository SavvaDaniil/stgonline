using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO.Video;
using STG.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class VideoSectionService
    {
        private ApplicationDbContext _dbc;
        public VideoSectionService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public VideoSection findById(int id)
        {
            return _dbc.VideoSections.FirstOrDefault(p => p.id == id);
        }

        public bool add(VideoSectionNewDTO videoSectionNewDTO, Video video)
        {
            VideoSection videoSection = new VideoSection();
            videoSection.video = video;
            videoSection.name = videoSectionNewDTO.name;

            _dbc.VideoSections.Add(videoSection);
            _dbc.SaveChanges();
            videoSection.orderInList = videoSection.id;
            _dbc.SaveChanges();
            return true;
        }

        public List<VideoSection> listAllByVideo(Video video)
        {
            return this._dbc.VideoSections
                .Where(p => p.video == video)
                .OrderBy(p => p.orderInList)
                .ToList();
        }

        public bool delete(int id)
        {
            VideoSection videoSection = findById(id);
            if (videoSection == null) return false;
            _dbc.VideoSections.Remove(videoSection);
            _dbc.SaveChanges();
            return true;
        }

        public bool update(VideoSectionDTO videoSectionDTO)
        {
            VideoSection videoSection = findById(videoSectionDTO.id);
            if (videoSection == null) return false;

            videoSection.name = videoSectionDTO.name;
            _dbc.SaveChanges();
            return true;
        }

    }
}
