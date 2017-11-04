# mouse-clicker
This simple program is able to keep sending mouse clicks to a window using Win API. This program specifically targets to PC game Stronghold Crusader.

## Why Stronghold Crusader?
Selling items in this game is very tedious. Especially when you have a lot of stuff. Therefore you need to do roughly 20 clicks to sell stones, 24 clicks to create 24 soldiers etc.

## How to use
Simply compile the project and run. Keeping this program running you'll start Stronghold Crusader. Then in the game you just need to push down middle mouse button (usually wheel) when and where you want to perform mouse clicks. Till you keep the middle mouse button down the program will be sending mouse click into Stronghold Crusder. Simply put, if you want to create all 24 soldiers, just hold the middle mouse button over selected unit and all available soldiers will be created.

## How it works
Program keeps running in an infinite loop and checks whether middle mouse button is down. If it is, it sends a mouse click.
```cs
static void Main(string[] args)
{
    // We will be infinitely making click if needed
    while (true)
    {
        if (CheckClick())
            DoMouseClick(1);

        Thread.Sleep(1);
    }
}
```
So how middle mouse button is checked? Simply:
```cs
private static bool CheckClick()
{
    // Was middle mouse button clicked?
    short status = GetAsyncKeyState((int)Keys.MButton);
    
    return status == -32767 || status == -32768;
}
```

And how is the mouse click sent to Stronghold Crusader?
```cs
private static void DoMouseClick(int count)
{
    // Get cursor's position
    GetCursorPos(out Point lpPoint);

    // Find window where we want to send mouse click
    IntPtr parentWindow = FindWindow(null, "Crusader");

    // Prepare click argument
    IntPtr lParam = (IntPtr)((lpPoint.Y << 16) | lpPoint.X);

    for (int i = 0; i < count; ++i)
    {
        // Send button push down message
        PostMessage(parentWindow, 0x0201, (IntPtr)1, lParam);
        Thread.Sleep(5);

        // Send button released message
        PostMessage(parentWindow, 0x0202, IntPtr.Zero, lParam);
        Thread.Sleep(5);
    }
}
```

We are leveraging from Win API calls here. Please note that application is identified by string (window name). This name you can find out by [WinLister](http://www.nirsoft.net/utils/winlister.html).

