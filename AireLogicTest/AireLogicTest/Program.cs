using System.Threading.Tasks;

namespace AireLogicTestDanTimms
{
    class Program
    {
        //Program Entry
        static async Task Main()
        {
            //Create a new instance of our averager component and await the program's response from the input.
            LyricAverager lyricAverager = new LyricAverager();
            await lyricAverager.ProcessLyrics();
        }
    }
}
