namespace ToFLac_NEW.Model.Parser
{
    public class ErrorToken
    {
        public int Line { get; set; }
        public int Index { get; set; }
        public string Message { get; set; }

        public ErrorToken(int line, int index, string message)
        {
            Line = line;
            Index = index;
            Message = message;
        }
    }
}