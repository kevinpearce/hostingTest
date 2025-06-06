using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace hostingTest.PlaywrightTests.Models;

public class DocumentsPage(IPage page, IConfiguration config)
{
    private readonly string _pagePath =
        config["TestSettings:DocumentsPagePath"] ?? throw new InvalidOperationException();

    public IPage Page => page;
    
    public async Task GoToPageAsync()
    {
        await page.GotoAsync(_pagePath);
    }
}