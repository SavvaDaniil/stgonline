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

        public async Task<VideoSection> findById(int id)
        {
            return await _dbc.VideoSections.FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<bool> add(VideoSectionNewDTO videoSectionNewDTO, Video video)
        {
            VideoSection videoSection = new VideoSection();
            videoSection.video = video;
            videoSection.name = videoSectionNewDTO.name;

            await _dbc.VideoSections.AddAsync(videoSection);
            await _dbc.SaveChangesAsync();
            videoSection.orderInList = videoSection.id;
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<List<VideoSection>> listAllByVideo(Video video)
        {
            return await this._dbc.VideoSections
                .Where(p => p.video == video)
                .OrderBy(p => p.orderInList)
                .ToListAsync();
        }

        public async Task<bool> delete(int id)
        {
            VideoSection videoSection = await findById(id);
            if (videoSection == null) return false;
            _dbc.VideoSections.Remove(videoSection);
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> update(VideoSectionDTO videoSectionDTO)
        {
            VideoSection videoSection = await findById(videoSectionDTO.id);
            if (videoSection == null) return false;

            videoSection.name = videoSectionDTO.name;
            await _dbc.SaveChangesAsync();
            return true;
        }

    }
}
