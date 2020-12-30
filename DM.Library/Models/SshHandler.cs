using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Renci.SshNet;

namespace DM.Library
{
    public class SshHandler : IDisposable
    {
        protected ConnectionInfo info { get; set; }
        private const string _hashKey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEogIBAAKCAQEAiC3Wh8CxN+xb3KERsEXsR9l2K9zW4qAlAmCrCIcw1qsBWyGG
k+SSjc6ApkaYZAdKX5KaVsqbCJva2fljzXeWXgZiBeA0tac71bCTVuYHBpbSgdCf
YRcG3cSIfj1Pekoyx6CehuPG2fWRV+gszMZ8+mAx+6AfKYZMqwWpzwI9vXNudAjB
89ALSDVdCun1pxEE67r9xNmKlqTBoDuUrGH0VnVJSH2JpVwX99QzXP+zwh/3wsqr
KdkDaAh/I+Nqq8YvTh7hT6wq+IhhaNBpP0XAc7g87f29+15jhWC79UtqX3z5KQSU
N6t4s2gYDSBRHqzSJ9n54VoAalobm6glnfXW2QIBJQKCAQBcA0vDgjKNkd00NXqn
iTDyRtNOEcGgC1dGy7HVVG0bbKANJH2Gha8hi4dbl3u5NVvECRwszhW5tWpb7bIj
C5zzbBi33K3+Mrm55gKwVjwZNXl6TrDgvIgZTXD7WdvBVLqG4iXscGPDs8MR4hBu
r6CNf0Rd6LscDqKBZLEBfg4KYbySY/uuVMeZXWemryrDcEBnGntbFuBFQd1Gfx7p
SEbevysA8lXHlPGYWgWsQM2ub35b2cAv8cqNkCLQYyM+5YELdX2G0L+2B3FUDFoh
Vbp96uSYdrSkNZOAuCdTcOndmyZ9MfpoR3oCFoHXDHMbxlIR9bQPV6wvZocnyjkC
RCxhAoGBAO2a9Y6fPriRh/LcROsdXeLA/udxc8TPXR8msGrCt4ez5NxEw7sVZaQD
FALvPP/oPZicTZWs2cFovPO0WrGZ8cfL43GgYGf7hjzRCZa/iIMK6MCZzPXCYQAw
AkUMHXjyK3W+s1UlWeW3DdqkF/lt28MgQMjOXlaPEvVP6kgfjXVbAoGBAJK4vKpc
zCtOk9DHT9+6XuvPlOThELdMTI4v7b65Xiy1DxjjmtS6gGqOJnnz8idIXBTHgn4r
zJtWvedFXGYvn3VSjYzyAlVWAV4loA+qkEgfF68gn9FacKe/0wmCKqvJLnNpWTLe
Yqs2s88qwreR8zln5d7ny9P7hnK+7rxSF/bbAoGBAM1/GZAbBdAPKXgYcvTbIMQc
iW44m3nIGS+ywhck8cF4/Ujhhq+jzYbt58svxg3B8BVI6SeAvFQ+6JtrjLVG3vH8
ca5hMMiiLuh9cBOskb1xOACFBEorkims+wtIv4syTxm5r9rv3wvcllw65E1YGAK0
HFqkpJ3jhgSYJJFnZZXtAoGAE9PGd+MG1Wt7wkRyk9rqO4rBGAK9Edng1PG4WAs2
PWSTVmPyVBkz8rlDeEOPbRedvZ5rlIJ1mHOBckeks9zsCO966Yh82xl1znPJhZOW
9P1IY8Yjb1FpK2zzAUjxAnUh8+ukRST4k61yPpcTZOo1n/lIk751fYLa0Txlc2UK
KEcCgYEAplAmf/TTJytWg/DaOKCHt6HgKXTR2Yu0vAwtFL6Wz96iqDZItgBmQzkb
aeCdvzqxmIndaq831LuvZ8TMr9LZUkNrTihKQM+P0FwuuDpRPdi0ZrPQmcvigoX8
z4Fb5pKeFokChFo6zgBH83d34V21IAwANCOIYxRTc6qiWy8Bs8g=
-----END RSA PRIVATE KEY-----";

        public ConnectionInfo Connector
        {
            get
            {
                return this.info;
            }
        }

        public SshHandler(string ip, int port, string id, string pwd)
        {
            this.info = new ConnectionInfo(ip, port, id, new AuthenticationMethod[] {
                new PasswordAuthenticationMethod(id, pwd),
                new PrivateKeyAuthenticationMethod(id, new PrivateKeyFile[]
                {
                    new PrivateKeyFile(new MemoryStream(Encoding.UTF8.GetBytes(_hashKey)))
                })
            });

            this.info.Timeout = TimeSpan.FromSeconds(30);
        }



        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                disposedValue = true;
            }
        }

        ~SshHandler()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public static class SshHandlerExtend
    {
        public static string ChangeAccount(this SshClient client, string id, string pwd)
        {
            string result = String.Empty;

            using (var stream = client.CreateShellStream("xterm", 80, 24, 800, 600, 1024))
            {
                stream.WriteLine(String.Format("su - {0}", id));
                string prompt = stream.Expect(new Regex(@"([$#>:])"));
                if (prompt.Contains(":"))
                {
                    // Send password
                    stream.WriteLine(pwd);
                    prompt = stream.Expect(new Regex(@"[$#>]"));
                    result = prompt;
                }
            }

            return result;
        }

    }
}
