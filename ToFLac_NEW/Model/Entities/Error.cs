namespace ToFLac_NEW.Model
{
    public class Error
    {
        public int Index { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Message { get; set; }

        public Error(int index, int start, int end, string message)
        {
            Index = index;
            Start = start;
            End = end;
            Message = message;
        }
    }
}
