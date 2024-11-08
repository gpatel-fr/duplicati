using System;
using System.Linq;
using System.Threading.Tasks;
using CoCoL;
using Duplicati.Library.Main.Database;

namespace Duplicati.Library.Main.Operation.Restore
{
    internal class FileProcessor
    {
        public static Task Run(LocalRestoreDatabase db)
        {
            return AutomationExtensions.RunTask(
            new
            {
                Input = Channels.filesToRestore.ForRead,
            },
            async self =>
            {
                // TODO preallocate the file size to avoid fragmentation / help the operating system / filesystem. Verify this in a benchmark - I think it relies on OS and filesystem.
                // using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None) { fs.SetLength(size); fs.Seek(0, SeekOrigin.Begin); }

                //while (true)
                {
                    var file = await self.Input.ReadAsync();
                    if (file == null)
                    {
                        //break;
                    }
                    Console.WriteLine($"Got file to restore: '{file.Path}', {file.Length} bytes, {file.Hash}");

                    var blocks = db.Connection.CreateCommand().ExecuteReaderEnumerable(@$"SELECT BlockID FROM BlocksetEntry WHERE BlocksetID = ""{file.BlocksetID}""").Select(x => x.GetInt64(0)).ToList();
                }
            });
        }
    }
}