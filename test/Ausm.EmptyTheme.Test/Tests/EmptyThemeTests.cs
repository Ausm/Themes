using Xunit;
using System;
using System.Linq;
using Xunit.Abstractions;
using System.Threading.Tasks;
using ObjectStore.Test.Identity.Fixtures;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ObjectStore.Test.Identity
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
        public async Task TestSignInSuccess(string url, string expectedHash)
        {
            byte[] buffer = await _fixture.Client.GetByteArrayAsync(url);

            SHA1 sha = SHA1.Create();
            string hash = Convert.ToBase64String(sha.ComputeHash(buffer));

            Assert.Equal(expectedHash, hash);
        }
        #endregion


        #region MemberData definitions
        public static TheoryData<string, string> PagesPresent
        {
            get
            {
                TheoryData<string, string> returnValue = new TheoryData<string, string>();
                returnValue.Add("/css/bootstrap.min.css", "ZSfYvz4ek2i6uMe2D1a8Afo6/Wg=");
                returnValue.Add("/css/bootstrap-theme.min.css", "glZXU3T0MEdr3NSd6Yx3mQIpzjE=");
                returnValue.Add("/fonts/glyphicons-halflings-regular.eot", "hrb2K3hT5n0+Y19lEqWl78WOo8M=");
                returnValue.Add("/fonts/glyphicons-halflings-regular.svg", "3lGoSUGAptsHSvLe4jg/CjY8Wwg=");
                returnValue.Add("/fonts/glyphicons-halflings-regular.ttf", "RLwYUPVwlyJnsWmuGPHLBrYR/6I=");
                returnValue.Add("/fonts/glyphicons-halflings-regular.woff", "J45JqG5jTabyoC87R92dKo8mIQ8=");
                returnValue.Add("/fonts/glyphicons-halflings-regular.woff2", "yjW2l9mcrk0bYPLWD803dxmH6wc=");
                returnValue.Add("/js/bootstrap.min.js", "QwpEPXSDD+m+Ju/KQx9EjBs3QPk=");
                returnValue.Add("/Home/Index", "NxhgNDtDNocTtV/C3v0HSoULoiE=");
                return returnValue;
            }
        }
        #endregion
    }
}
