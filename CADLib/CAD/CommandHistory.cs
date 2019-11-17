using System.Collections.Generic;

namespace CAD
{
    public class CommandHistory
    {
        //Points to current history entry
        internal int HistoryPointer = -1;
        //Contains history entries
        internal List<string> History = new List<string>();
        /**********************
         * HISTORY
         **********************/

        public void AddToHistory(string text)
        {
            if (History.Count == 0)
            {
                History.Add(text);
                HistoryPointer = History.Count;
                return;
            }
            if (text != History[History.Count - 1])
            {
                History.Add(text);
                HistoryPointer = History.Count;
            }
        }

        public string GetPreviousCommand()
        {
            string command = null;
            if (HistoryPointer > -1)
            {
                HistoryPointer--;
                if (HistoryPointer > -1)
                {
                    command = History[HistoryPointer];
                }
                else
                {
                    HistoryPointer++;
                }
            }
            return command;
        }

        public string GetNextCommand()
        {
            string command = null;
            if (HistoryPointer + 1 < History.Count)
            {
                HistoryPointer++;
                if (HistoryPointer + 1 <= History.Count)
                {
                    command = History[HistoryPointer];
                }
                else
                {
                    HistoryPointer--;
                }
            }
            return command;

            
        }
    }
}




