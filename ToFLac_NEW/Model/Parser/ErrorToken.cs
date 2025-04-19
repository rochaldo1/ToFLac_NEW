namespace ToFLac_NEW.Model.Parser
{
    public class ErrorToken
    {
        public string Message { get; set; }
        public int Line { get; set; }
        public int Index { get; set; }

        public ErrorToken(string message, int line, int index)
        {
            Message = message;
            Line = line;
            Index = index;
        }
    }
}