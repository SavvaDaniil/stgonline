using STG.Data;
using STG.DTO.Video;
using STG.Entities;
using STG.Service;
using STG.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class VideoSectionFacade
    {
        private ApplicationDbContext _dbc;
        public VideoSectionFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<bool> add(VideoSectionNewDTO videoSectionNewDTO)
        {
            VideoService videoService = new VideoService(_dbc);
            Video video = await videoService.findById(videoSectionNewDTO.id_of_video);
            if (video == null) return false;
            videoService = null;

            VideoSectionService videoSectionService = new VideoSectionService(_dbc);

            return await videoSectionService.add(videoSectionNewDTO, video);
        }


        public async Task<List<VideoSectionViewModel>> listAllByIdOfVideo(int id_of_video)
        {
            VideoService videoService = new VideoService(_dbc);
            Video video = await videoService.findById(id_of_video);
            if (video == null) return null;
            videoService = null;

            VideoSectionService videoSectionService = new VideoSectionService(_dbc);
            List<VideoSection> videosectionList = await videoSectionService.listAllByVideo(video);

            List<VideoSectionViewModel> videoSectionViewModelList = new List<VideoSectionViewModel>();
            VideoSubsectionService videoSubsectionService = new VideoSubsectionService(_dbc);
            List<VideoSubsection> videosubsectionListAll = await videoSubsectionService.listAllByVideo(video);

            foreach (VideoSection videoSection in videosectionList)
            {
                List<VideoSubsectionLiteViewModel> videosubsectionList = new List<VideoSubsectionLiteViewModel>();
                foreach (VideoSubsection videoSubsection in videosubsectionListAll)
                {
                    if (videoSubsection.videoSection.id == videoSection.id) videosubsectionList.Add(
                        new VideoSubsectionLiteViewModel(
                            videoSubsection.id,
                            videoSubsection.name,
                            videoSubsection.timingMinutes,
                            videoSubsection.timingSeconds
                        )
                    );
                }

                videoSectionViewModelList.Add(
                    new VideoSectionViewModel(
                        videoSection.id,
                        videoSection.name,
                        videosubsectionList
                    )
                );
            }

            video = null;
            videoSectionService = null;
            videoSubsectionService = null;
            videosectionList = null;
            videosubsectionListAll = null;

            return videoSectionViewModelList;
        }

        public async Task<bool> update(VideoSectionDTO videoSectionDTO)
        {
            VideoSectionService videoSectionService = new VideoSectionService(_dbc);
            return await videoSectionService.update(videoSectionDTO);
        }

        public async Task<bool> delete(VideoSectionIdDTO videoSectionIdDTO)
        {
            VideoSectionService videoSectionService = new VideoSectionService(_dbc);
            VideoSection videoSection = await videoSectionService.findById(videoSectionIdDTO.id);

            VideoSubsectionService videoSubsectionService = new VideoSubsectionService(_dbc);
            await videoSubsectionService.deleteAllByVideoSection(videoSection);

            return await videoSectionService.delete(videoSectionIdDTO.id);
        }
    }
}
