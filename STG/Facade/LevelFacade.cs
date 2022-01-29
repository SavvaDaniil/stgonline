using STG.Data;
using STG.Entities;
using STG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class LevelFacade
    {
        private ApplicationDbContext _dbc;
        public LevelFacade(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }

        public async Task<List<Level>> getListOfLevelFromStringListOfIds(string listOfIdOfLevels)
        {
            LevelService levelService = new LevelService(_dbc);
            List<Level> listAll = await levelService.listAll();

            if (listOfIdOfLevels == null) return null;
            listOfIdOfLevels = listOfIdOfLevels.Replace(@" ", @"");
            string[] arrayOfIdOfLevels = listOfIdOfLevels.Split(",");

            List<Level> listAllAnswer = new List<Level>();

            int id_of_level = 0;
            foreach (string idAsStr in arrayOfIdOfLevels)
            {
                try
                {
                    id_of_level = int.Parse(idAsStr);
                    foreach (Level level in listAll)
                    {
                        if (level.Id == id_of_level)
                        {
                            if (!listAllAnswer.Contains(level)) listAllAnswer.Add(level);
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }

            return listAllAnswer;
        }
    }
}
