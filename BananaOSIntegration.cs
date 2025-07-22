using System.Text;
using UnityEngine;
using static WhoIsThatMonke.PublicVariablesGatherHere;

namespace BananaOS.Pages
{
    public class MainPageeeee : WatchPage
    {
        public override string Title => "WhoIsThatMonke";
        public override bool DisplayOnMainMenu => true;
        public override void OnPostModSetup() => selectionHandler.maxIndex = 4;

        public override string OnGetScreenContent()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<color=yellow>== WhoIsThatMonke ==</color>");
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(0, "<color=yellow> Settings </color>\n"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(1, "<color=blue> Platform Checker </color>"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(2, "<color=blue> Velocity Checker </color>"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(3, "<color=blue> FPS Checker </color>"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(4, "<color=blue> Color Code Spoofer </color>"));
            return stringBuilder.ToString();
        }

        public static string GetTheEnabled(bool varvar) => varvar ? "Enabled" : "Disabled";

        public override void OnButtonPressed(WatchButtonType buttonType)
        {
            switch (buttonType)
            {
                case WatchButtonType.Up:
                    selectionHandler.MoveSelectionUp();
                    break;

                case WatchButtonType.Down:
                    selectionHandler.MoveSelectionDown();
                    break;

                case WatchButtonType.Enter:
                    if (selectionHandler.currentIndex == 0)
                    {
                        SwitchToPage(typeof(SettingoPago));
                        return;
                    }
                    if (selectionHandler.currentIndex == 1)
                    {
                        isPlatformEnabled = !isPlatformEnabled;
                        SendNotification($"<align=center><size=5> Platform Checker is now: {GetTheEnabled(isPlatformEnabled)}");
                        return;
                    }
                    if (selectionHandler.currentIndex == 2)
                    {
                        isVelocityEnabled = !isVelocityEnabled;
                        SendNotification($"<align=center><size=5> Velocity Checker is now: {GetTheEnabled(isVelocityEnabled)}");
                        return;
                    }
                    if (selectionHandler.currentIndex == 3)
                    {
                        isFPSEnabled = !isFPSEnabled;
                        SendNotification($"<align=center><size=5> FPS Checker is now: {GetTheEnabled(isFPSEnabled)}");
                        return;
                    }
                    if (selectionHandler.currentIndex == 4)
                    {
                        isColorCodeEnabled = !isColorCodeEnabled;
                        SendNotification($"<align=center><size=5> Color Code Spoofer is now: {GetTheEnabled(isColorCodeEnabled)}");
                        return;
                    }
                    break;

                case WatchButtonType.Back:
                    ReturnToMainMenu();
                    break;
            }
        }
        public static void SendNotification(string message) => BananaNotifications.DisplayNotification(message, Color.yellow, Color.white, 0.8f);
    }

    public class SettingoPago : WatchPage
    {
        public override string Title => "Who cares?";
        public override bool DisplayOnMainMenu => false;
        public override void OnPostModSetup() => selectionHandler.maxIndex = 0;

        public override string OnGetScreenContent()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(0, "<color=blue> 255 Color Codes </color>"));
            return stringBuilder.ToString();
        }

        public override void OnButtonPressed(WatchButtonType buttonType)
        {
            switch (buttonType)
            {
                case WatchButtonType.Up:
                    selectionHandler.MoveSelectionUp();
                    break;

                case WatchButtonType.Down:
                    selectionHandler.MoveSelectionDown();
                    break;

                case WatchButtonType.Enter:
                    if (selectionHandler.currentIndex == 0)
                    {
                        twoFiftyFiveColorCodes = !twoFiftyFiveColorCodes;
                        MainPageeeee.SendNotification($"<align=center><size=5> 255 Color Codes are now: {MainPageeeee.GetTheEnabled(twoFiftyFiveColorCodes)}");
                        return;
                    }
                    break;

                case WatchButtonType.Back:
                    SwitchToPage(typeof(MainPageeeee));
                    break;
            }
        }
    }
}
