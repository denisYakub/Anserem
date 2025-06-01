using Anserem.Models;

namespace Anserem.Services
{
    public class EmailProcessor(
        IReadOnlySet<string> doms,
        IReadOnlyDictionary<string, string[]> exeptionEmails,
        IReadOnlyDictionary<string, string[]> substitutionEmails) : IEmailProcessor
    {
        public void Process(Email email)
        {
            var allEmails = 
                email.To
                .Concat(email.Copy);

            var allDomains = 
                allEmails
                .Select(GetDomain)
                .Where(domain => doms.Contains(domain))
                .Distinct()
                .ToList();

            var copySet = new HashSet<string>(email.Copy);

            foreach (var domain in allDomains)
            {
                var hasException = allEmails
                    .Any(e => exeptionEmails.TryGetValue(domain, out var exceptions)
                              && exceptions.Contains(e, StringComparer.OrdinalIgnoreCase));

                if (hasException)
                {
                    if (substitutionEmails.TryGetValue(domain, out var subsToRemove))
                    {
                        foreach (var sub in subsToRemove)
                            copySet.Remove(sub);
                    }
                    continue;
                }

                if (substitutionEmails.TryGetValue(domain, out var subsToAdd))
                {
                    foreach (var sub in subsToAdd)
                        copySet.Add(sub);
                }
            }

            email.Copy = copySet;
        }
        private static string GetDomain(string email)
        {
            var parts = email.Split('@');
            return parts.Length == 2 ? parts[1].ToLowerInvariant() : "";
        }
    }
}
