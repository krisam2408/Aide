using Aide.ColorExtraction.DataTransfer;
using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using System.Globalization;
using TerminalWrapper;

namespace Aide.ColorExtraction.Tasks;

internal class ExtractHSBColorsTask : MainTask
{
    private const string Locale = "en-US";
    private readonly string m_outputDirectory;
    private const string m_filenamePattern = "adobe_hsb_b*.csv";
    
    public override string TaskName => "Extract HSB Colors";

    private readonly List<HSBValue> m_results = [];

    public ExtractHSBColorsTask(string outputPath)
    {
        m_outputDirectory = $"{outputPath}hsb/";
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {        
        await Terminal.WriteAsync("Opening WebDriver");

        int currentBrightness = CheckPreviousRecords();

        await Terminal.WriteAsync($"Extracting colors for Brightness {currentBrightness}");

        try
        {
            using (EdgeDriver driver = OpenNavigator())
            {
                await GetResults(driver, currentBrightness, cancelToken);
            }
        }
        catch (TaskCanceledException)
        {
            await Terminal.WriteAsync("Task canceled by user...");
        }
        finally
        {
            await Terminal.WriteAsync($"Done checking with {m_results.Count} entries...");

            string brgId = currentBrightness
                .ToString()
                .PadLeft(3, '0');

            string filename = m_filenamePattern
                .Replace("*", brgId);

            using (StreamWriter sw = new($"{m_outputDirectory}{filename}"))
            using (CsvWriter csv = new(sw, CultureInfo.GetCultureInfo(Locale)))
            {
                csv.WriteRecords(m_results);
            };
        }
    }

    private static EdgeDriver OpenNavigator()
    {
        EdgeDriverService service = EdgeDriverService.CreateDefaultService();
        service.EnableVerboseLogging = false;

        EdgeOptions options = new()
        {
            PageLoadStrategy = PageLoadStrategy.Normal,
        };
        options.AddArgument("--start-maximized");

        return new EdgeDriver(options);
    }

    private int CheckPreviousRecords()
    {
        m_results.Clear();

        string[] existingFiles = Directory
            .GetFiles(m_outputDirectory)
            .OrderByDescending(n => n)
            .ToArray();

        if (existingFiles.Length == 0)
            return 100;

        string lastFile = existingFiles.Last();
        using StreamReader sr = new(lastFile);
        using CsvReader csv = new(sr, CultureInfo.GetCultureInfo(Locale));
        HSBValue[] records = csv
            .GetRecords<HSBValue>()
            .OrderByDescending(c => c.Brightness)
            .ThenByDescending(c => c.Saturation)
            .ThenBy(c => c.Hue)
            .ToArray();

        HSBValue lastValue = records.Last();
        int currBrg = lastValue.Brightness;
        lastValue++;

        if (lastValue.Brightness < currBrg)
            return lastValue.Brightness;

        m_results.AddRange(records);
        return currBrg;
    }

    private async Task GetResults(EdgeDriver driver, int currentBrightness, CancellationToken token)
    {
        #region Setting up
        await driver.Navigate()
            .GoToUrlAsync("https://color.adobe.com/create/color-wheel");

        await Terminal.WriteAsync("Successful navigation...");
        await Terminal.WriteAsync("Waiting 4 seconds...");

        await Task.Delay(4000, token);

        driver.ExecuteScript("document.body.style.zoom='70%'");

        IWebElement? boardingPanel = GetElement(driver, By.ClassName("OnBoardingTourDialog__tourDialogModal___utC65"));
        if(boardingPanel is not null)
        {
            IWebElement? closePanel = GetElement(driver, By.ClassName("OnBoardingTourDialog__closeButton___U4osK"));
            closePanel?.Click();
        }

        await Terminal.WriteAsync("Setting HSB environment...");

        IWebElement? spectrumDropdown = GetElement(driver, By.CssSelector(".ColorModeSelector__modeSelector___cCjCm"));
        spectrumDropdown?.Click();

        await Task.Delay(16, token);
        
        int hsbTries = 5;
        do
        {
            IWebElement[] spectrumItems = GetElements(driver, By.CssSelector("span.spectrum-Menu-itemLabel span"));
            foreach (IWebElement li in spectrumItems)
            {
                string liText = li.Text;
                if (liText.Contains("HSB"))
                {
                    li.Click();
                    hsbTries = -1;
                    await Task.Delay(16, token);
                    break;
                }
            }
            hsbTries--;
        } while (hsbTries > 0);

        PressEnd(driver);

        int sliderTries = 20;
        do
        {
            IWebElement[] sliderButtons = GetElements(driver, By.CssSelector(".spectrum-Button--primary.Swatch__slidersAndLabels___Eu5av"));
        
            if(sliderButtons.Length == 0)
            {
                PressEnd(driver);
                sliderTries--;
                continue;
            }

            if(ClickInteractable(sliderButtons))
            {
                await Task.Delay(8,token);
                break;
            }

            sliderTries--;

        }while(sliderTries > 0);

        int hexTries = 16;
        IWebElement? hex = null;
        do
        {
            IWebElement[] hexInputs = GetElements(driver, By.CssSelector(".Colorwheel__swatchDisplay___WKyUI:first-child input[type=text].HexInputField__hexInputField___cmU7v"));

            if(hexInputs.Length == 0)
            {
                PressEnd(driver);
                hexTries--;
                continue;
            }

            hex = hexInputs[0];
            hexTries = -1;

        }while(hexTries > 0);

        if(hex is null)
        {
            await Terminal.WriteAsync("Hex input not found...");
            return;
        }

        IWebElement[] valueInputs = GetElements(driver, By.CssSelector(".Colorwheel__swatchDisplay___WKyUI:first-child input[type=number]"));
        
        if (valueInputs.Length == 0)
        {
            await Terminal.WriteAsync("Hex inputs not found...");
            return;
        }

        IWebElement hue = valueInputs[0];
        IWebElement sat = valueInputs[1];
        IWebElement brg = valueInputs[2];
        #endregion

        token.ThrowIfCancellationRequested();

        #region Extract Colors
        int sb = currentBrightness;
        int ss = 100;
        int sh = 0;

        if (m_results.Count > 0)
        {
            HSBValue last = m_results.Last();
            last++;
            sb = last.Brightness;
            ss = last.Saturation;
            sh = last.Hue;
        }

        for(int s = ss; s >= 0; s--)
        {
            for(int h = sh; h < 360; h++)
            {
                token.ThrowIfCancellationRequested();

                SetValue(hue, h.ToString());
                SetValue(sat, s.ToString());
                SetValue(brg, sb.ToString());

                token.ThrowIfCancellationRequested();

                await Task.Delay(8, token);

                token.ThrowIfCancellationRequested();

                string hexValue = GetValue(hex);

                if(!string.IsNullOrWhiteSpace(hexValue))
                {
                    HSBValue value = new()
                    {
                        Hue = h,
                        Saturation = s,
                        Brightness = sb,
                        Hex = hexValue
                    };

                    m_results.Add(value);
                }
            }
            sh = 0;
        }
        #endregion
    }

    private static IWebElement? GetElement(EdgeDriver driver, By by)
    {
        try
        {
            IWebElement element = driver.FindElement(by);
            return element;
        }
        catch(NoSuchElementException)
        {
            return null;
        }
    }

    private static IWebElement[] GetElements(EdgeDriver driver, By by)
    {
        try
        {
            IWebElement[] elements = driver.FindElements(by).ToArray();
            return elements;
        }
        catch (NoSuchElementException)
        {
            return [];
        }
        catch(InvalidSelectorException)
        {
            return [];
        }
    }

    private static bool ClickInteractable(IWebElement[] elements)
    {
        foreach (IWebElement element in elements)
        {
            try
            {
                element.Click();
                return true;
            }
            catch (ElementNotInteractableException)
            {
                continue;
            }
        }
        return false;
    }

    private static void SetValue(IWebElement element, string value)
    {
        try
        {
            element.Clear();
            element.SendKeys(value);
        }
        catch(WebDriverException) { }
    }

    private static string GetValue(IWebElement element)
    {
        try
        {
            string? value = element.GetAttribute("value");

            if (string.IsNullOrWhiteSpace(value))
                return "";

            return value;
        }
        catch(WebDriverException)
        {
            return "";
        }
    }

    private static void PressEnd(EdgeDriver driver)
    {
        Actions actions = new(driver);
        Actions windowEnd = actions.SendKeys(Keys.End);
        windowEnd.Perform();
    }
}
