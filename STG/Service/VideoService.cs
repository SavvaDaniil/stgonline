using Microsoft.EntityFrameworkCore;
using STG.Component;
using STG.Data;
using STG.DTO;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class VideoService
    {
        private const int lengthOfRandomPathOfFile = 32;

        private ApplicationDbContext _dbc;
        public VideoService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<Video> findById(int id)
        {
            return await this._dbc.Videos.SingleOrDefaultAsync(p => p.id == id);
        }

        public async Task<List<Video>> listAll()
        {
            return await this._dbc.Videos.OrderByDescending(p => p.id).ToListAsync();
        }

        public async Task<IEnumerable<Video>> listAllEnum()
        {
            return await this._dbc.Videos.OrderByDescending(p => p.id).ToListAsync();
        }

        public async Task<Video> add(VideoNewDTO videoNewDTO)
        {
            Video video = new Video();
            video.name = videoNewDTO.name;
            video.dateOfAdd = DateTime.Now;
            video.hashPath = RandomComponent.RandomString(lengthOfRandomPathOfFile);

            this._dbc.Videos.Add(video);

            await this._dbc.SaveChangesAsync();

            return video;
        }

        public async Task<bool> update(VideoDTO videoDTO)
        {
            Video video = await findById(videoDTO.id);

            if (video == null) return false;

            video.name = video.name;

            await this._dbc.SaveChangesAsync();

            return true;
        }

        public async Task<bool> delete(int id)
        {
            Video video = await findById(id);
            this._dbc.Videos.Remove(video);
            await this._dbc.SaveChangesAsync();
            return true;
        }

        public async Task<Video> save(VideoDTO videoDTO)
        {
            Video video = await findById(videoDTO.id);
            if (video == null) return null;

            video.name = videoDTO.name;
            if(video.hashPath == null)
            {
                video.hashPath = RandomComponent.RandomString(lengthOfRandomPathOfFile);
            }
            video.duration = videoDTO.durationSeconds + videoDTO.durationMinutes * 60 + videoDTO.durationHours * 60 * 60;


            await this._dbc.SaveChangesAsync();

            return video;
        }








        private void updateRandomPathOfFiles(Video video)
        {

        }

        private string generateRandomPathForFilePath(int id)
        {
            return String.Empty;
        }

    }
}
