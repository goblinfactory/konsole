using System;

namespace Konsole.Remote
{
    public record RunArgs(string Name, int Port, string Key, Protocol Proto)
    {
        public static RunArgs Parse(string[] args)
        {
            try
            {
                var protocol = (Protocol)Enum.Parse(typeof(Protocol), args[3].ToLower());
                return new RunArgs(args[0], int.Parse(args[1]), args[2], protocol);
            }
            catch(Exception ex)
            {
                throw new ArgsException(ex.Message);
            }
        }
    }
}
