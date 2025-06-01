using Anserem.Models;
using Anserem.Services;

namespace AnseremTests
{
    [TestClass]
    public sealed class EmailProcessorTests
    {
        IEmailProcessor emailProcessor;

        [TestInitialize]
        public void Init()
        {
            var doms = new HashSet<string> { "tbank.ru", "alfa.com", "vtb.ru" };

            var exeptionEmails = new Dictionary<string, string[]>(3)
            {
                ["tbank.ru"] = ["i.ivanov@tbank.ru"],
                ["alfa.com"] = ["s.sergeev@alfa.com", "a.andreev@alfa.com"],
                ["vtb.ru"] = []
            };

            var substitutionEmails = new Dictionary<string, string[]>(3)
            {
                ["tbank.ru"] = ["t.tbankovich@tbank.ru", "v.veronickovna@tbank.ru"],
                ["alfa.com"] = ["v.vladislavovich@alfa.com"],
                ["vtb.ru"] = ["a.aleksandrov@vtb.ru"]
            };

            emailProcessor = new EmailProcessor(doms, exeptionEmails, substitutionEmails);
        }

        [TestMethod]
        public void Should_Add_SubstitutionAddress_When_DomainMatches_And_NoExceptions()
        {
            var inputTo = new List<string>() 
            { 
                "q.qweshnikov@batut.com", 
                "w.petrov@alfa.com"
            };
            var inputCopy = new List<string>()
            {
                "f.patit@buisness.com"
            };

            var email = new Email("", inputTo, inputCopy, "", "", "");

            emailProcessor.Process(email);

            var outputCopy = new List<string>()
            {
                "f.patit@buisness.com",
                "v.vladislavovich@alfa.com"
            };

            Assert.IsTrue(new HashSet<string>(email.Copy).SetEquals(outputCopy));
        }

        [TestMethod]
        public void Should_NotAdd_SubstitutionAddress_When_ExceptionPresent()
        {
            var inputTo = new List<string>()
            {
                "t.kogni@acl.com"
            };
            var inputCopy = new List<string>()
            {
                "i.ivanov@tbank.ru"
            };

            var email = new Email("", inputTo, inputCopy, "", "", "");

            emailProcessor.Process(email);

            var outputCopy = new List<string>()
            {
                "i.ivanov@tbank.ru"
            };

            Assert.IsTrue(new HashSet<string>(email.Copy).SetEquals(outputCopy));
        }

        [TestMethod]
        public void Should_Remove_SubstitutionAddresses_When_ExceptionInToOrCopy()
        {
            var inputTo = new List<string>()
            {
                "t.kogni@acl.com",
                "i.ivanov@tbank.ru"
            };
            var inputCopy = new List<string>()
            {
                "e.gras@tbank.ru",
                "t.tbankovich@tbank.ru",
                "v.veronickovna@tbank.ru"
            };

            var email = new Email("", inputTo, inputCopy, "", "", "");

            emailProcessor.Process(email);

            var outputCopy = new List<string>()
            {
                "e.gras@tbank.ru"
            };

            Assert.IsTrue(new HashSet<string>(email.Copy).SetEquals(outputCopy));
        }

        [TestMethod]
        public void Should_Leave_EmailsUnchanged_When_NoMatchingDomainsOrExceptions()
        {
            var inputTo = new List<string>()
            {
                "z.xcy@email.com"
            };
            var inputCopy = new List<string>()
            {
                "p.rivet@email.com"
            };

            var email = new Email("", inputTo, inputCopy, "", "", "");

            emailProcessor.Process(email);

            var outputCopy = new List<string>()
            {
                "p.rivet@email.com"
            };

            Assert.IsTrue(new HashSet<string>(email.Copy).SetEquals(outputCopy));
        }
    }
}
