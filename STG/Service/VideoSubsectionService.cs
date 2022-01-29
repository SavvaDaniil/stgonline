using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO.Video;
using STG.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class VideoSubsectionService
    {
        private ApplicationDbContext _dbc;
        public VideoSubsectionService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public VideoSubsection findById(int id)
        {
            return _dbc.VideoSubsections.FirstOrDefault(p => p.id == id);
        }

        public async Task<List<VideoSubsection>> listAllByVideo(Video video)
        {
            return await _dbc.VideoSubsections
                .Where(p => p.video == video)
                .OrderBy(p => p.orderInList)
                .ToListAsync();
        }

        public async Task<List<VideoSubsection>> listAllByVideoSection(VideoSection videoSection)
        {
            return await _dbc.VideoSubsections
                .Where(p => p.videoSection == videoSection)
                .OrderBy(p => p.orderInList)
                .ToListAsync();
        }

        public async Task<List<VideoSubsection>> listAllByVideoAndVideoSection(Video video, VideoSection videoSection)
        {
            return await _dbc.VideoSubsections
                .Where(p => p.video == video)
                .Where(p => p.videoSection == videoSection)
                .OrderBy(p => p.orderInList)
                .ToListAsync();
        }

        public async Task<bool> add(VideosubsectionNewDTO videosubsectionNewDTO, Video video, VideoSection videoSection)
        {
            VideoSubsection videoSubsection = new VideoSubsection();

            videoSubsection.video = video;
            videoSubsection.videoSection = videoSection;
            videoSubsection.name = videosubsectionNewDTO.name;

            await _dbc.VideoSubsections.AddAsync(videoSubsection);
            await _dbc.SaveChangesAsync();
            videoSubsection.orderInList = videoSubsection.id;
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> delete(int id)
        {
            VideoSubsection videoSubsection = findById(id);
            if (videoSubsection == null) return false;
            _dbc.VideoSubsections.Remove(videoSubsection);
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> deleteAllByVideoSection(VideoSection videoSection)
        {
            List<VideoSubsection> videoSubsections = await listAllByVideoSection(videoSection);
            foreach(VideoSubsection videoSubsection in videoSubsections)
            {
                _dbc.VideoSubsections.Remove(videoSubsection);
            }

            return false;
        }

        public async Task<bool> update(VideoSubsectionDTO videoSubsectionDTO)
        {
            VideoSubsection videoSubsection = findById(videoSubsectionDTO.id);
            if (videoSubsection == null) return false;
            switch (videoSubsectionDTO.field){
                case "name":
                    videoSubsection.name = videoSubsectionDTO.value;
                    break;
                case "timing_minutes":
                    videoSubsection.timingMinutes = int.Parse(videoSubsectionDTO.value);
                    break;
                case "timing_seconds":
                    videoSubsection.timingSeconds = int.Parse(videoSubsectionDTO.value);
                    break;
                default:
                    return false;
            }
            await _dbc.SaveChangesAsync();

            return true;
        }

        /*

            public function deleteAllByIdOfSection(int $id_of_section): bool{

                Videosubsection::deleteAll("id_of_section = :id_of_section", ["id_of_section" => $id_of_section]);
                return true;
            }


            public function update(VideosubsectionDTO $videosubsectionDTO): bool {
                $videosubsection = $this -> findById($videosubsectionDTO -> id);
                if($videosubsection == null)return false;
        
                switch($videosubsectionDTO -> field){
                    case "name":
                        $videosubsection->name = $videosubsectionDTO->value;
                break;
                    case "timing_minutes":
                        $videosubsection->timing_minutes = $videosubsectionDTO->value;
                break;
                    case "timing_seconds":
                        $videosubsection->timing_seconds = $videosubsectionDTO->value;
                break;
                default:
                        return false;
            }

                return $videosubsection -> save();
            }
          */
    }
}
