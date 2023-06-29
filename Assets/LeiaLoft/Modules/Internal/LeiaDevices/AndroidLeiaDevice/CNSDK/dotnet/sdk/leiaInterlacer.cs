#if UNITY_ANDROID
using System;
using System.Runtime.InteropServices;

using UnityEngine;

namespace Leia
{
    public class Interlacer
    {
        private const int kRenderCallbackEvent_Init = 1;
        private const int kRenderCallbackEvent_Draw = 2;

        private IntPtr _renderCallback;
        public Interlacer(SDK sdk)
        {
            int status = leiaSdkUnityRenderingPluginInitialize(sdk.GetNativePtr());
            if (status != 0)
            {
                throw new Exception("Failed to initialize: " + status);
            }

            _renderCallback = leiaSdkUnityRenderingPluginGetRenderEventFunc();
            if (_renderCallback == null)
            {
                throw new Exception("No render callback");
            }

            IssuePluginEvent(kRenderCallbackEvent_Init);
        }
        public void Dispose(SDK sdk)
        {
            leiaSdkUnityRenderingPluginRelease(sdk.GetNativePtr());
        }
        public bool SetInputViews(Texture[] viewTextures)
        {
            IntPtr[] nativePtrs = Array.ConvertAll(viewTextures, texture => texture.GetNativeTexturePtr());
            int error = leiaSdkUnityRenderingPluginSetInputViews(viewTextures.Length, nativePtrs);
            return error == 0;
        }
        public void SetOutput(RenderTexture output)
        {
            leiaSdkUnityRenderingPluginSetOutput(output.GetNativeTexturePtr(), output.width, output.height);
        }
        public void Render()
        {
            IssuePluginEvent(kRenderCallbackEvent_Draw);
        }
        private void IssuePluginEvent(int eventType)
        {
            GL.IssuePluginEvent(_renderCallback, eventType);
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct Config
        {
            public bool useCPVM;
        }
        public Config GetConfig()
        {
            Config config;
            leiaSdkUnityRenderingPluginGetConfig(out config);
            return config;
        }
        public void SetConfig(in Config config)
        {
            leiaSdkUnityRenderingPluginSetConfig(in config);
        }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        private const string SDK_DLL_NAME = @"leiaSDK-faceTrackingInApp.dll"; // TODO: dynamic switching
#else
        private const string SDK_DLL_NAME = @"leiaSDK";
#endif

        [DllImport(SDK_DLL_NAME)]
        private static extern int leiaSdkUnityRenderingPluginInitialize(IntPtr sdk);
        [DllImport(SDK_DLL_NAME)]
        private static extern int leiaSdkUnityRenderingPluginSetInputViews(int numViews, IntPtr[] nativePtrs);
        [DllImport(SDK_DLL_NAME)]
        private static extern void leiaSdkUnityRenderingPluginSetOutput(IntPtr nativePtr, int width, int height);
        [DllImport(SDK_DLL_NAME)]
        private static extern void leiaSdkUnityRenderingPluginGetConfig(out Config config);
        [DllImport(SDK_DLL_NAME)]
        private static extern void leiaSdkUnityRenderingPluginSetConfig(in Config config);
        [DllImport(SDK_DLL_NAME)]
        private static extern IntPtr leiaSdkUnityRenderingPluginGetRenderEventFunc();
        [DllImport(SDK_DLL_NAME)]
        private static extern void leiaSdkUnityRenderingPluginRelease(IntPtr sdk);
    }
}
#endif
