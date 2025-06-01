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
    private readonly string m_outputPath;
    
    public override string TaskName => "Extract HSB Colors";

    List<HSBValue> m_results = [];

    public ExtractHSBColorsTask(string outputPath)
    {
        m_outputPath = $"{outputPath}adobe_hsv.csv";
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {        
        await Terminal.WriteAsync("Opening WebDriver");

        CheckPreviousRecords();

        try
        {
            using(EdgeDriver driver = OpenNavigator())
            {
                await GetResults(driver, cancelToken);
            }
        }
        catch (TaskCanceledException)
        {
            await Terminal.WriteAsync("Task canceled by user...");
        }
        finally
        {
            await Terminal.WriteAsync($"Done checking with {m_results.Count} entries...");

            using (StreamWriter sw = new(m_outputPath))
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

    private void CheckPreviousRecords()
    {
        m_results = [];

        if(File.Exists(m_outputPath))
        {
            using StreamReader sr = new(m_outputPath);
            using CsvReader csv = new(sr, CultureInfo.GetCultureInfo(Locale));
            HSBValue[] records = csv
                .GetRecords<HSBValue>()
                .ToArray();

            records = records
                .OrderBy(c => c.Hue)
                .ThenByDescending(c => c.Saturation)
                .ThenByDescending(c => c.Brightness)
                .ToArray();

            m_results.AddRange(records);
        }
    }

    private async Task GetResults(EdgeDriver driver, CancellationToken token)
    {
        #region Setting up
        await driver.Navigate()
            .GoToUrlAsync("https://color.adobe.com/create/color-wheel");

        await Terminal.WriteAsync("Successful navigation...");
        await Terminal.WriteAsync("Waiting 4 seconds...");

        await Task.Delay(4000, token);

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

        Actions actions = new(driver);
        Actions downArrow = actions.SendKeys(Keys.ArrowDown);

        int sliderTries = 20;
        do
        {
            IWebElement[] sliderButtons = GetElements(driver, By.CssSelector(".spectrum-Button--primary.Swatch__slidersAndLabels___Eu5av"));
        
            if(sliderButtons.Length == 0)
            {
                PerformThrice(downArrow);
                continue;
            }

            if(ClickInteractable(sliderButtons))
            {
                sliderTries = -1;
            }

            sliderTries--;

        }while(sliderTries > 0);

        int hexTries = 16;
        IWebElement? hex = null;
        do
        {
            IWebElement[] hexInputs = GetElements(driver, By.CssSelector("input[type=text].HexInputField__hexInputField___cmU7v"));

            if(hexInputs.Length == 0)
            {
                PerformThrice(downArrow);
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
        int sb = 100;
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

        for(int b = sb; b >= 0; b--)
        {
            for(int s = ss; s >= 0; s--)
            {
                for(int h = sh; h < 360; h++)
                {
                    token.ThrowIfCancellationRequested();

                    SetValue(hue, h.ToString());
                    SetValue(sat, s.ToString());
                    SetValue(brg, b.ToString());

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
                            Brightness = b,
                            Hex = hexValue
                        };

                        m_results.Add(value);
                    }
                }
                sh = 0;
            }
            ss = 100;
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

    private static void PerformThrice(Actions action)
    {
        for (int i = 0; i < 3; i++)
            action.Perform();
    }
}
