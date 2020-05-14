using ArnoldVinkCode;
using System.Configuration;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        public static AVTaskDetails vTask_Wallpaper = new AVTaskDetails();

        //Start all the background tasks
        void TasksBackgroundStart()
        {
            try
            {
                TaskStart_TimeMeWallpaper();
            }
            catch { }
        }

        //Stop all the background tasks
        public static async Task TasksBackgroundStop()
        {
            try
            {
                await AVActions.TaskStopLoop(vTask_Wallpaper);
            }
            catch { }
        }

        void TaskStart_TimeMeWallpaper()
        {
            try
            {
                if (ConfigurationManager.AppSettings["TimeMeWallpaper"] == "True")
                {
                    AVActions.TaskStartLoop(LoopCheckWallpaper, vTask_Wallpaper);
                }
            }
            catch { }
        }
    }
}