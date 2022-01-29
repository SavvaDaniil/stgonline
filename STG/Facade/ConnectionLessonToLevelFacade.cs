using STG.Data;
using STG.Entities;
using STG.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class ConnectionLessonToLevelFacade
    {
        private ApplicationDbContext _dbc;
        public ConnectionLessonToLevelFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }


        public async Task<bool> updateConnectionsOfLesson(Lesson lesson, List<Level> levels)
        {
            ConnectionLessonToLevelService connectionLessonToLevelService = new ConnectionLessonToLevelService(_dbc);

            if(levels == null)
            {
                await connectionLessonToLevelService.deleteAll(lesson);
                return true;
            }

            //сначала добавляем все, что есть, а что уже есть, не добавится
            foreach (Level level in levels)
            {
                await connectionLessonToLevelService.add(level, lesson);
            }

            List<ConnectionLessonToLevel> connectionsLessonToLevelsAlreadyConnected = await connectionLessonToLevelService.listAllByLesson(lesson);
            List<Level> levelsAlreadyConnected = new List<Level>();
            foreach (ConnectionLessonToLevel connectionLessonToLevelAlreadyExist in connectionsLessonToLevelsAlreadyConnected)
            {
                if (!levelsAlreadyConnected.Contains(connectionLessonToLevelAlreadyExist.level)) levelsAlreadyConnected.Add(connectionLessonToLevelAlreadyExist.level);
            }

            /*
            Level[] levelsForRemove = levels.ToArray().Except((connectionLessonToLevelAlreadyExist.ToArray()));

            foreach (Level level in levels)
            {
                foreach (ConnectionLessonToLevel connectionLessonToLevelAlreadyExist in connectionsLessonToLevelsAlreadyConnected)
                {

                }

            }
            */
            bool isAlreadyExistConnection = false;
            //теперь сравниваем все, что есть, и удаляем, если нет в пришедшем списке
            foreach (ConnectionLessonToLevel connectionLessonToLevelAlreadyExist in connectionsLessonToLevelsAlreadyConnected)
            {
                isAlreadyExistConnection = false;
                //System.Diagnostics.Debug.WriteLine("Рассматриваем connectionLessonToLevelAlreadyExist.Id: " + connectionLessonToLevelAlreadyExist.id);
                foreach (Level level in levels)
                {
                    //System.Diagnostics.Debug.WriteLine("connectionLessonToLevelAlreadyExist.level.Id: " + connectionLessonToLevelAlreadyExist.level.Id + " level.Id: " + level.Id);
                    if(connectionLessonToLevelAlreadyExist.level.Id == level.Id)
                    {
                        isAlreadyExistConnection = true;
                        continue;
                    }
                }
                if (isAlreadyExistConnection) continue;

                //раз его нет в новом списке, то удаляем
                //System.Diagnostics.Debug.WriteLine("Для удаления connectionLessonToLevelAlreadyExist.Id: " + connectionLessonToLevelAlreadyExist.id);
                await connectionLessonToLevelService.delete(connectionLessonToLevelAlreadyExist);
            }


            return true;
        }

        public async Task<List<Level>> getListOfLevel(Lesson lesson)
        {
            ConnectionLessonToLevelService connectionLessonToLevelService = new ConnectionLessonToLevelService(_dbc);
            List<ConnectionLessonToLevel> listAll = await connectionLessonToLevelService.listAllByLesson(lesson);
            List<Level> listAllAnswer = new List<Level>();

            foreach (ConnectionLessonToLevel connectionLessonToLevel in listAll)
            {
                if (connectionLessonToLevel.lesson == lesson)
                {
                    if (!listAllAnswer.Contains(connectionLessonToLevel.level)) listAllAnswer.Add(connectionLessonToLevel.level);
                }
            }
            return listAllAnswer;
        }


    }
}
