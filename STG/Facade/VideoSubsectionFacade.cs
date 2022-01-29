using STG.Data;
using STG.DTO.Video;
using STG.Entities;
using STG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class VideoSubsectionFacade
    {
        private ApplicationDbContext _dbc;
        public VideoSubsectionFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<bool> add(VideosubsectionNewDTO videosubsectionNewDTO)
        {
            VideoService videoService = new VideoService(_dbc);
            Video video = await videoService.findById(videosubsectionNewDTO.id_of_video);
            if (video == null) return false;

            VideoSectionService videoSectionService = new VideoSectionService(_dbc);
            VideoSection videoSection = await videoSectionService.findById(videosubsectionNewDTO.id_of_section);
            if (videoSection == null) return false;

            videoService = null;
            videoSectionService = null;

            VideoSubsectionService videoSubsectionService = new VideoSubsectionService(_dbc);

            return await videoSubsectionService.add(videosubsectionNewDTO, video, videoSection);
        }



        public async Task<bool> update(VideoSubsectionDTO videoSubsectionDTO)
        {
            VideoSubsectionService videoSubsectionService = new VideoSubsectionService(_dbc);
            return await videoSubsectionService.update(videoSubsectionDTO);
        }

        public async Task<bool> delete(VideoSubsectionIdDTO videoSubsectionIdDTO)
        {
            VideoSubsectionService videoSubsectionService = new VideoSubsectionService(_dbc);
            return await videoSubsectionService.delete(videoSubsectionIdDTO.id);
        }
    }
}
