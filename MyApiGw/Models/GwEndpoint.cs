namespace MyApiGw.Models
{
    public class GwEndpoint
    {
        private string _upstream;
        public string BasePath { get; set; }
        public string Upstream
        {
            get { return _upstream; }
            set
            {                
                _upstream = (value[^1].Equals('/')) ? value[0..^1] : value;
            }
        }
        public string Methods { get; set; }
    }
}

