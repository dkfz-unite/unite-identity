using Unite.Post;
using Unite.Post.Mails;

namespace Unite.Tests;

[TestClass]
public sealed class MailClientTests
{
    [TestMethod]
    public void TestMethod1()
    {
        var mail = new PasswordReset { Host ="unite.com", Token = "4b087c52-0ed6-4354-9914-938b67fbb40e" };
        
        var data = MailClient.GetData(mail);
        var template = MailClient.GetTemplate(typeof(PasswordReset));
        var rendered = MailClient.RenderTemplate(template, data);

        Assert.IsTrue(rendered.Contains("unite.com"));
        Assert.IsTrue(rendered.Contains("unite.com/reset-confirm/4b087c52-0ed6-4354-9914-938b67fbb40e"));
    }
}
