using System;

namespace WhoIsThatMonke
{
    public class PublicVariablesGatherHere
    {
        // Backing fields
            // Module bools
            private static bool _isPlatformEnabled = Plugin.Instance.PlatformCheckerEnabled.Value;
            private static bool _isColorCodeEnabled = Plugin.Instance.ColorCodeSpooferEnabled.Value;
            private static bool _isVelocityEnabled = Plugin.Instance.VelocityCheckerEnabled.Value;
            private static bool _isFPSEnabled = Plugin.Instance.FPSCheckerEnabled.Value;

            // Module setting bools
            private static bool _twoFiftyFiveColorCodes = Plugin.Instance.TwoFiveFiveColorCodesEnabled.Value;

        // Central event
        public static event Action BoolChangedButOnlyTheGoodOnes;

        public static bool isPlatformEnabled
        {
            get => _isPlatformEnabled;
            set
            {
                if (_isPlatformEnabled != value)
                {
                    _isPlatformEnabled = value;
                    Plugin.Instance.PlatformCheckerEnabled.Value = value;
                    Plugin.Instance.cfg.Save();
                    BoolChangedButOnlyTheGoodOnes?.Invoke();
                }
            }
        }

        public static bool isColorCodeEnabled
        {
            get => _isColorCodeEnabled;
            set
            {
                if (_isColorCodeEnabled != value)
                {
                    _isColorCodeEnabled = value;
                    Plugin.Instance.ColorCodeSpooferEnabled.Value = value;
                    Plugin.Instance.cfg.Save();
                    BoolChangedButOnlyTheGoodOnes?.Invoke();
                }
            }
        }

        public static bool isVelocityEnabled
        {
            get => _isVelocityEnabled;
            set
            {
                if (_isVelocityEnabled != value)
                {
                    _isVelocityEnabled = value;
                    Plugin.Instance.VelocityCheckerEnabled.Value = value;
                    Plugin.Instance.cfg.Save();
                    BoolChangedButOnlyTheGoodOnes?.Invoke();
                }
            }
        }

        public static bool isFPSEnabled
        {
            get => _isFPSEnabled;
            set
            {
                if (_isFPSEnabled != value)
                {
                    _isFPSEnabled = value;
                    Plugin.Instance.FPSCheckerEnabled.Value = value;
                    Plugin.Instance.cfg.Save();
                    BoolChangedButOnlyTheGoodOnes?.Invoke();
                }
            }
        }

        public static bool twoFiftyFiveColorCodes
        {
            get => _twoFiftyFiveColorCodes;
            set
            {
                if (_twoFiftyFiveColorCodes != value)
                {
                    _twoFiftyFiveColorCodes = value;
                    Plugin.Instance.TwoFiveFiveColorCodesEnabled.Value = value;
                    Plugin.Instance.cfg.Save();
                    BoolChangedButOnlyTheGoodOnes?.Invoke();
                }
            }
        }
    }
}
