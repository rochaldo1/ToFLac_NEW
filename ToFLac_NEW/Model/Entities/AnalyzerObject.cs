namespace ToFLac_NEW.Model
{
    public class AnalyzerObject
    {
        public int Code { get; set; }
        public int Index { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Token { get; set; }
        public string CurrentToken { get; set; }

        public AnalyzerObject(int code, int index, int start, int end, string token, string currentToken)
        {
            Code = code;
            Index = index;
            Start = start;
            End = end;
            Token = token;
            CurrentToken = currentToken;
        }
    }
}
