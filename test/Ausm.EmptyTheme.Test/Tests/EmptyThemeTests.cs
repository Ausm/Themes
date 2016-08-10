using Xunit;
using System;
using Xunit.Abstractions;
using System.Threading.Tasks;
using Ausm.EmptyTheme.Test.Fixtures;
using System.Security.Cryptography;

namespace Ausm.EmptyTheme.Test
{
    public class EmptyThemeTests : IClassFixture<TestServerFixture>
    {
        #region Fields
        ITestOutputHelper _output;
        TestServerFixture _fixture;
        #endregion

        #region Constructor
        public EmptyThemeTests(ITestOutputHelper output, TestServerFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }
        #endregion

        #region Tests
        [Theory, MemberData(nameof(PagesPresent))]
        public async Task TestSignInSuccess(string url, string expectedSubstring)
        {
            string content = await _fixture.Client.GetStringAsync(url);

            if (expectedSubstring == null)
                return;

            _output.WriteLine(content);

            Assert.Contains(expectedSubstring, content);
        }
        #endregion


        #region MemberData definitions
        public static TheoryData<string, string> PagesPresent
        {
            get
            {
                TheoryData<string, string> returnValue = new TheoryData<string, string>();
                returnValue.Add("/css/bootstrap.min.css", "* Bootstrap v3");
                returnValue.Add("/css/bootstrap-theme.min.css", "* Bootstrap v3");
                returnValue.Add("/fonts/glyphicons-halflings-regular.eot", null);
                returnValue.Add("/fonts/glyphicons-halflings-regular.svg", null);
                returnValue.Add("/fonts/glyphicons-halflings-regular.ttf", null);
                returnValue.Add("/fonts/glyphicons-halflings-regular.woff", null);
                returnValue.Add("/fonts/glyphicons-halflings-regular.woff2", null);
                returnValue.Add("/js/bootstrap.min.js", "* Bootstrap v3");
                returnValue.Add("/Home/Index", "<title>Home</title>");
                return returnValue;
            }
        }
        #endregion
    }
}
