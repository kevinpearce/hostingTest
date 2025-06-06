using hostingTest.PlaywrightTests.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace hostingTest.PlaywrightTests;

public class DocumentsPageTests : PageTest
{
    private IConfigurationRoot _config;
    private IPage _documentsPage;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }
    
    [SetUp]
    public async Task SetUp()
    {
        var documentsPage = new DocumentsPage(await Browser.NewPageAsync(), _config);
        await documentsPage.GoToPageAsync();

        _documentsPage = documentsPage.Page;
    }
    
    [Test]
    public async Task HomePageTitle_IsDisplayed()
    {
        await Expect(_documentsPage)
            .ToHaveTitleAsync("Documents");
    }
    
    [Test]
    public async Task DocumentsBlurb_IsDisplayed()
    {
        await Expect(_documentsPage.Locator("pre.pre-padding"))
            .ToContainTextAsync("Please find below all documents/forms related to our organization");
    }

    [Test]
    public async Task DocumentsTable_IsPresent()
    {
        await Expect(_documentsPage.Locator("table.table-bordered"))
            .ToBeVisibleAsync();
    }

    [Test]
    public async Task AtLeastOneDocumentRow_IsPresent()
    {
        await _documentsPage.WaitForSelectorAsync("tbody tr");
        var rows = await _documentsPage.Locator("tbody tr").CountAsync();
        
        Assert.That(rows, Is.GreaterThan(0));
    }

    [Test]
    public async Task DownloadLink_ExistsForEachDocument()
    {
        await _documentsPage.WaitForSelectorAsync("tbody tr");
        var rowCount = await _documentsPage.Locator("tbody tr").CountAsync();
        
        await Expect(_documentsPage.Locator("a.btn.btn-primary[download]"))
            .ToHaveCountAsync(rowCount);
    }
    
    [TestCase("Sign Up Form", "ExampleForm1.txt")]
    [TestCase("Membership Renewal Form", "ExampleForm2.txt")]
    public async Task DownloadLink_ForSignUpForm_TriggersDownload(string documentName, string expectedFilename)
    {
        await _documentsPage.WaitForSelectorAsync("tbody tr");
        var row = _documentsPage.Locator("tbody tr", new PageLocatorOptions { HasTextString = documentName });
        await Expect(row).ToBeVisibleAsync();

        var downloadTask = _documentsPage.WaitForDownloadAsync();
        await row.Locator("a.btn.btn-primary[download]").ClickAsync();
        var download = await downloadTask;

        Assert.Multiple(() =>
        {
            Assert.That(download, Is.Not.Null);
            Assert.That(download.SuggestedFilename, Is.EqualTo(expectedFilename));
        });
    }
    
    [Test]
    public async Task DocumentsTable_HasCorrectColumnHeaders()
    {
        var headers = _documentsPage.Locator("table.table-bordered thead th");
        
        await Expect(headers.Nth(0)).ToHaveTextAsync("File Name");
        await Expect(headers.Nth(1)).ToHaveTextAsync("Actions");
    }
    
    [TestCase(375, 667, TestName = "Mobile_iPhone8")]
    [TestCase(768, 1024, TestName = "Tablet_iPad")]
    [TestCase(1440, 900, TestName = "Desktop_HD")]
    public async Task DocumentsTable_IsVisibleOnVariousViewports(int width, int height)
    {
        await _documentsPage.SetViewportSizeAsync(width, height);
        await Expect(_documentsPage.Locator("table.table-bordered")).ToBeVisibleAsync();
    }
}