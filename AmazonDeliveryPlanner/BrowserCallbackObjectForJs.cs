namespace AmazonDeliveryPlanner
{
    // http://windowsapptutorials.com/wpf/call-c-sharp-javascript-using-cefsharp-wpf-app/
    // adapted from https://stackoverflow.com/a/66148896
    // alternative solution using OnBrowserJavascriptMessageReceived
    public class BrowserCallbackObjectForJs
    {
        public void openDriverWindow(string driverId)
        {
            // Note: The name of the C# function should start with a small letter.            
            // MessageBox.Show(driverId);
            GlobalContext.MainWindow.OpenDriverWindow(driverId);
        }
    }
}
