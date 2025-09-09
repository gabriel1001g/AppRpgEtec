using Android.App;
using Android.Runtime;

namespace AppRpgEtec
{
    [Application(UsesCleartextTraffic = true)] //habilita o trafego de dados
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
