using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptTool.CryptoLib
{
    public enum CryptState
    {
        Null,
        Compress,
        Decompress,
        Encrypt,
        Decrypt,
        Finish,
        Cancel,
        Error
    }

    public class TaskStateChangedEventArgs: EventArgs
    {
        public CryptState CryptState { get; set; }
        public string Description { get; set; }
        public int TaskID { get; set; }
        public int CurrentNumber { get; set; }
        public int TotalNumber { get; set; }
    }

    public delegate void TaskStateChangedEventHandler(object sender, TaskStateChangedEventArgs e);
}
