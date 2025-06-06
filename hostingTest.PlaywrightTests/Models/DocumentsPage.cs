using Microsoft.Playwright;

namespace hostingTest.PlaywrightTests.Models;

public class DocumentsPage(IPage page)
{
    private const string PagePath = "https://kevinpearce.github.io/hostingTest/documents";

    public IPage Page => page;
    
    public async Task GoToPageAsync()
    {
        await page.GotoAsync(PagePath);
    }
}