namespace Anserem.Models
{
    public class Email
    {
        public IEnumerable<string> To 
        {
            get
            {
                return _to;
            }
        }

        public IEnumerable<string> Copy 
        {
            get
            {
                return _copy;
            }
            set
            {
                _copy = new HashSet<string>(value);
            }
        }

        private string _from;
        private HashSet<string> _to;

        private HashSet<string> _copy;
        private string _blindCopy;

        private string _title;
        private string _body;

        public Email(
            string from, IEnumerable<string> to,
            IEnumerable<string> copy, string blindCopy,
            string title, string body)
        {
            _from = from;
            _to = [.. to];
            _copy = [.. copy];
            _blindCopy = blindCopy;
            _title = title;
            _body = body;
        }

    }
}
